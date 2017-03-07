namespace Cake.Prca.PullRequests.Tfs
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.Linq;
    using System.Threading;
    using Core.Diagnostics;
    using Core.IO;
    using Issues;
    using Microsoft.TeamFoundation.SourceControl.WebApi;
    using Microsoft.VisualStudio.Services.Common;
    using Microsoft.VisualStudio.Services.WebApi;

    /// <summary>
    /// Class for writing issues to Team Foundation Server or Visual Studio Team Services pull requests.
    /// </summary>
    public sealed class TfsPullRequestSystem : PullRequestSystem, IDisposable
    {
        private readonly RepositoryDescription repositoryDescription;
        private readonly VssConnection connection;
        private readonly GitHttpClient gitClient;
        private readonly GitPullRequest pullRequest;

        private readonly List<GitPullRequestCommentThread> cachedDiscussionThreads = new List<GitPullRequestCommentThread>();

        /// <summary>
        /// Initializes a new instance of the <see cref="TfsPullRequestSystem"/> class.
        /// Connects to the TFS server using NTLM authentication.
        /// </summary>
        /// <param name="log">The Cake log context.</param>
        /// <param name="settings">Settings for accessing TFS.</param>
        public TfsPullRequestSystem(ICakeLog log, TfsPullRequestSettings settings)
            : base(log)
        {
            settings.NotNull(nameof(settings));

            this.repositoryDescription = new RepositoryDescription(settings.RepositoryUrl);

            this.Log.Verbose(
                "Repository information:\n  CollectionName: {0}\n  CollectionUrl: {1}\n  ProjectName: {2}\n  RepositoryName: {3}",
                this.repositoryDescription.CollectionName,
                this.repositoryDescription.CollectionUrl,
                this.repositoryDescription.ProjectName,
                this.repositoryDescription.RepositoryName);

            this.connection = new VssConnection(this.repositoryDescription.CollectionUrl, new VssCredentials());

            this.gitClient = this.connection.GetClient<GitHttpClient>();
            if (this.gitClient == null)
            {
                throw new PrcaException("Could not retrieve the GitHttpClient object");
            }

            if (settings.PullRequestId.HasValue)
            {
                this.Log.Verbose("Read pull request with ID {0}", settings.PullRequestId.Value);
                this.pullRequest =
                    this.gitClient.GetPullRequestAsync(
                        this.repositoryDescription.ProjectName,
                        this.repositoryDescription.RepositoryName,
                        settings.PullRequestId.Value).Result;
            }
            else if (!string.IsNullOrWhiteSpace(settings.SourceBranch))
            {
                this.Log.Verbose("Read pull request for branch {0}", settings.SourceBranch);
                var pullRequestSearchCriteria =
                    new GitPullRequestSearchCriteria()
                    {
                        Status = PullRequestStatus.Active,
                        SourceRefName = settings.SourceBranch
                    };
                this.pullRequest =
                    this.gitClient.GetPullRequestsAsync(
                        this.repositoryDescription.ProjectName,
                        this.repositoryDescription.RepositoryName,
                        pullRequestSearchCriteria,
                        top: 1).Result.SingleOrDefault();
            }
            else
            {
                throw new ArgumentOutOfRangeException(
                    nameof(settings),
                    "Either PullRequestId or SourceBranch needs to be set");
            }

            if (this.pullRequest == null)
            {
                throw new PrcaException("Could not find pull request");
            }

            this.Log.Verbose(
                "Pull request information:\n  PullRequestId: {0}\n  RepositoryId: {1}\n  RepositoryName: {2}\n  SourceRefName: {3}",
                this.pullRequest.PullRequestId,
                this.pullRequest.Repository.Id,
                this.pullRequest.Repository.Name,
                this.pullRequest.SourceRefName);
        }

        /// <inheritdoc/>
        public override IEnumerable<IPrcaDiscussionThread> FetchActiveDiscussionThreads(string commentSource)
        {
            this.cachedDiscussionThreads.Clear();

            var request =
                this.gitClient.GetThreadsAsync(
                    this.pullRequest.Repository.Id,
                    this.pullRequest.PullRequestId,
                    null,
                    null,
                    null,
                    CancellationToken.None);
            var threads = request.Result;

            var threadList = new List<IPrcaDiscussionThread>();
            foreach (var thread in threads)
            {
                if (thread.IsCommentSource(commentSource) && thread.Status == CommentThreadStatus.Active)
                {
                    this.cachedDiscussionThreads.Add(thread);
                    threadList.Add(thread.ToPrcaDiscussionThread());
                }
            }

            this.Log.Verbose("Found {0} discussion thread(s)", threadList.Count);
            return threadList;
        }

        /// <inheritdoc/>
        public override IEnumerable<FilePath> GetModifiedFilesInPullRequest()
        {
            this.Log.Verbose("Computing the list of files changed in this pull request...");

            var targetVersionDescriptor = new GitTargetVersionDescriptor();
            targetVersionDescriptor.VersionType = GitVersionType.Commit;
            targetVersionDescriptor.Version = this.pullRequest.LastMergeSourceCommit.CommitId;

            var baseVersionDescriptor = new GitBaseVersionDescriptor();
            baseVersionDescriptor.VersionType = GitVersionType.Commit;
            baseVersionDescriptor.Version = this.pullRequest.LastMergeTargetCommit.CommitId;

            var commitDiffs = this.gitClient.GetCommitDiffsAsync(
                this.repositoryDescription.ProjectName,
                this.repositoryDescription.RepositoryName,
                true, // bool? diffCommonCommit
                null, // int? top
                null, // int? skip
                baseVersionDescriptor,
                targetVersionDescriptor,
                null, // object userState
                CancellationToken.None).Result;

            if (!commitDiffs.ChangeCounts.Any())
            {
                return new List<FilePath>();
            }

            this.Log.Verbose(
                "Found {0} changed file(s) in the pull request",
                commitDiffs.Changes.Count());

            return
                from change in commitDiffs.Changes
                where
                    change != null &&
                    !change.Item.IsFolder
                select
                    new FilePath(change.Item.Path);
        }

        /// <inheritdoc/>
        public override void MarkThreadAsFixed(IPrcaDiscussionThread thread)
        {
            thread.NotNull(nameof(thread));

            var threads = this.cachedDiscussionThreads.Where(x => x.Id == thread.Id);

            if (!threads.Any())
            {
                throw new PrcaException(
                    string.Format(
                        CultureInfo.InvariantCulture,
                        "Thread with ID {0} not found",
                        thread.Id));
            }

            if (threads.Count() != 1)
            {
                throw new PrcaException(
                    string.Format(
                        CultureInfo.InvariantCulture,
                        "Found more than one thread with ID {0}",
                        thread.Id));
            }

            var newThread = new GitPullRequestCommentThread();
            newThread.Status = CommentThreadStatus.Fixed;

            this.gitClient.UpdateThreadAsync(
                newThread,
                this.pullRequest.Repository.Id,
                this.pullRequest.PullRequestId,
                threads.Single().Id,
                null,
                CancellationToken.None).Wait();
        }

        /// <inheritdoc/>
        public override void PostDiscussionThreads(IEnumerable<ICodeAnalysisIssue> issues, string commentSource)
        {
            issues.NotNull(nameof(issues));

            var threads = this.CreateDiscussionThreads(issues, commentSource);

            if (!threads.Any())
            {
                this.Log.Verbose("No threads to post");
                return;
            }

            foreach (var thread in threads)
            {
                // TODO Result handling?
                this.gitClient.CreateThreadAsync(
                    thread,
                    this.pullRequest.Repository.Id,
                    this.pullRequest.PullRequestId,
                    null,
                    CancellationToken.None).Wait();
            }

            this.Log.Information("Posted {0} discussion threads", threads.Count());
        }

        /// <inheritdoc/>
        public void Dispose()
        {
            if (this.gitClient != null)
            {
                this.gitClient.Dispose();
            }

            if (this.connection != null)
            {
                this.connection.Dispose();
            }
        }

        private static void AddCodeFlowProperties(
           ICodeAnalysisIssue issue,
           int iterationId,
           int changeTrackingId,
           PropertiesCollection properties)
        {
            issue.NotNull(nameof(issue));
            properties.NotNull(nameof(properties));

            properties.Add("Microsoft.VisualStudio.Services.CodeReview.ItemPath", issue.AffectedFileRelativePath.ToString());
            properties.Add("Microsoft.VisualStudio.Services.CodeReview.Right.StartLine", issue.Line);
            properties.Add("Microsoft.VisualStudio.Services.CodeReview.Right.EndLine", issue.Line);
            properties.Add("Microsoft.VisualStudio.Services.CodeReview.Right.StartOffset", 0);
            properties.Add("Microsoft.VisualStudio.Services.CodeReview.Right.EndOffset", 1);
            properties.Add("Microsoft.VisualStudio.Services.CodeReview.FirstComparingIteration", iterationId);
            properties.Add("Microsoft.VisualStudio.Services.CodeReview.SecondComparingIteration", iterationId);
            properties.Add("Microsoft.VisualStudio.Services.CodeReview.ChangeTrackingId", changeTrackingId);
        }

        private IEnumerable<GitPullRequestCommentThread> CreateDiscussionThreads(
            IEnumerable<ICodeAnalysisIssue> issues,
            string commentSource)
        {
            issues.NotNull(nameof(issues));

            this.Log.Verbose("Creating new discussion threads");
            var result = new List<GitPullRequestCommentThread>();
            var discussionId = -1;

            // Code flow properties
            var iterationId = 0;
            GitPullRequestIterationChanges changes = null;

            if (this.pullRequest.CodeReviewId > 0)
            {
                iterationId = this.GetCodeFlowLatestIterationId();
                changes = this.GetCodeFlowChanges(iterationId);
            }

            foreach (var issue in issues)
            {
                this.Log.Information(
                    "Creating a discussion comment for the issue at line {0} from {1}",
                    issue.Line,
                    issue.AffectedFileRelativePath);

                var newThread = new GitPullRequestCommentThread()
                {
                    Status = CommentThreadStatus.Active
                };

                var discussionComment = new Comment();
                discussionComment.CommentType = CommentType.System;
                discussionComment.IsDeleted = false;
                discussionComment.Content = issue.Message;

                this.AddThreadProperties(newThread, changes, issue, iterationId, commentSource);

                newThread.Comments = new List<Comment>() { discussionComment };
                result.Add(newThread);
                discussionId--;
            }

            return result;
        }

        private void AddThreadProperties(
            GitPullRequestCommentThread thread,
            GitPullRequestIterationChanges changes,
            ICodeAnalysisIssue issue,
            int iterationId,
            string commentSource)
        {
            thread.NotNull(nameof(thread));
            changes.NotNull(nameof(changes));
            issue.NotNull(nameof(issue));

            var properties = new PropertiesCollection();

            if (this.pullRequest.CodeReviewId > 0)
            {
                var changeTrackingId =
                    this.TryGetCodeFlowChangeTrackingId(changes, issue.AffectedFileRelativePath);
                if (changeTrackingId < 0)
                {
                    return;
                }

                AddCodeFlowProperties(issue, iterationId, changeTrackingId, properties);
            }
            else
            {
                throw new NotSupportedException("Legacy code reviews are not supported.");
            }

            // A VSTS UI extension will recognize this and format the comments differently.
            properties.Add("CodeAnalysisThreadType", "CodeAnalysisIssue");

            thread.Properties = properties;

            // Add a custom property to be able to distinguish all comments created this way.
            thread.SetCommentSource(commentSource);
        }

        private int GetCodeFlowLatestIterationId()
        {
            var request =
                this.gitClient.GetPullRequestIterationsAsync(
                    this.pullRequest.Repository.Id,
                    this.pullRequest.PullRequestId,
                    null,
                    null,
                    CancellationToken.None);
            var iterations = request.Result;

            if (iterations == null)
            {
                throw new PrcaException("Could not retrieve the iterations");
            }

            var iterationId = iterations.Max(x => x.Id.HasValue ? x.Id.Value : -1);
            this.Log.Verbose("Dermined iteration id: {0}", iterationId);
            return iterationId;
        }

        private GitPullRequestIterationChanges GetCodeFlowChanges(int iterationId)
        {
            var request =
                this.gitClient.GetPullRequestIterationChangesAsync(
                    this.pullRequest.Repository.Id,
                    this.pullRequest.PullRequestId,
                    iterationId,
                    null,
                    null,
                    null,
                    null,
                    CancellationToken.None);
            var changes = request.Result;

            if (changes != null)
            {
                this.Log.Verbose("Change count: {0}", changes.ChangeEntries.Count());
            }

            return changes;
        }

        private int TryGetCodeFlowChangeTrackingId(GitPullRequestIterationChanges changes, FilePath path)
        {
            changes.NotNull(nameof(changes));
            path.NotNull(nameof(path));

            var change = changes.ChangeEntries.Where(x => x.Item.Path == path.ToString());
            if ((change == null) || (change.Count() != 1))
            {
                this.Log.Warning(
                    "Cannot post a comment for the file {0} because no changes could be found",
                    path);
                return -1;
            }

            var changeTrackingId = change.Single().ChangeTrackingId;
            this.Log.Verbose(
                "Determined ChangeTrackingId of {0} for {1}",
                changeTrackingId,
                path);
            return changeTrackingId;
        }
    }
}
