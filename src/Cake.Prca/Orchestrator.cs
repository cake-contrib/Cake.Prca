namespace Cake.Prca
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Globalization;
    using System.Linq;
    using Core.Diagnostics;
    using Issues;
    using PullRequests;

    /// <summary>
    /// Class for writing code analysis issues to pull requests.
    /// </summary>
    internal class Orchestrator
    {
        private readonly ICakeLog log;
        private readonly List<ICodeAnalysisProvider> codeAnalysisProviders = new List<ICodeAnalysisProvider>();
        private readonly IPullRequestSystem pullRequestSystem;
        private readonly ReportCodeAnalysisIssuesToPullRequestSettings settings;

        /// <summary>
        /// Initializes a new instance of the <see cref="Orchestrator"/> class.
        /// </summary>
        /// <param name="log">Cake log instance.</param>
        /// <param name="codeAnalysisProviders">List of code analysis issue providers to use.</param>
        /// <param name="pullRequestSystem">Object for accessing pull request system.</param>
        /// <param name="settings">Settings.</param>
        public Orchestrator(
            ICakeLog log,
            IEnumerable<ICodeAnalysisProvider> codeAnalysisProviders,
            IPullRequestSystem pullRequestSystem,
            ReportCodeAnalysisIssuesToPullRequestSettings settings)
        {
            log.NotNull(nameof(log));
            pullRequestSystem.NotNull(nameof(pullRequestSystem));
            settings.NotNull(nameof(settings));

            // ReSharper disable once PossibleMultipleEnumeration
            codeAnalysisProviders.NotNullOrEmptyOrEmptyElement(nameof(codeAnalysisProviders));

            this.log = log;
            this.pullRequestSystem = pullRequestSystem;
            this.settings = settings;

            // ReSharper disable once PossibleMultipleEnumeration
            this.codeAnalysisProviders.AddRange(codeAnalysisProviders);
        }

        /// <summary>
        /// Runs the orchestrator.
        /// Posts new issues, ignoring duplicate comments and resolves comments that were open in an old iteration
        /// of the pull request.
        /// </summary>
        public void Run()
        {
            this.log.Verbose("Initialize pull request system...");
            this.pullRequestSystem.Initialize(this.settings);

            var format = this.pullRequestSystem.GetPreferredCommentFormat();
            this.log.Verbose("Pull request system prefers comments in {0} format.", format);

            var issues = new List<ICodeAnalysisIssue>();
            foreach (var codeAnalysisProvider in this.codeAnalysisProviders)
            {
                this.log.Verbose("Initialize code analysis provider {0}...", codeAnalysisProvider.GetType().Name);
                codeAnalysisProvider.Initialize(this.settings);

                this.log.Verbose("Reading issues from {0}...", codeAnalysisProvider.GetType().Name);
                var currentIssues = codeAnalysisProvider.ReadIssues(format).ToList();
                this.log.Verbose(
                    "Found {0} issues using issue provider {1}...",
                    currentIssues.Count,
                    codeAnalysisProvider.GetType().Name);
                issues.AddRange(currentIssues);
            }

            this.log.Information("Processing {0} new issues", issues.Count);

            this.PostAndResolveComments(issues);
        }

        /// <summary>
        /// Checks if file path from an <see cref="ICodeAnalysisIssue"/> and <see cref="IPrcaDiscussionThread"/>
        /// are matching.
        /// </summary>
        /// <param name="issue">Issue to check.</param>
        /// <param name="thread">Comment thread to check.</param>
        /// <returns><c>true</c> if both paths are matching or if both paths are set to <c>null</c>.</returns>
        private static bool FilePathsAreMatching(ICodeAnalysisIssue issue, IPrcaDiscussionThread thread)
        {
            return
                (issue.AffectedFileRelativePath == null && thread.AffectedFileRelativePath == null) ||
                (
                    issue.AffectedFileRelativePath != null &&
                    thread.AffectedFileRelativePath != null &&
                    thread.AffectedFileRelativePath.ToString() == issue.AffectedFileRelativePath.ToString());
        }

        /// <summary>
        /// Posts new issues, ignoring duplicate comments and resolves comments that were open in an old iteration
        /// of the pull request.
        /// </summary>
        /// <param name="issues">Issues to post.</param>
        private void PostAndResolveComments(IList<ICodeAnalysisIssue> issues)
        {
            issues.NotNull(nameof(issues));

            this.log.Information("Fetching existing threads and comments...");

            var existingThreads = this.pullRequestSystem.FetchActiveDiscussionThreads(this.settings.CommentSource).ToList();

            var issueComments = this.BuildIssueToCommentDictonary(issues, existingThreads);

            // Comments that were created by this logic but do not have corresponding issues can be marked as 'Resolved'
            this.ResolveExistingComments(existingThreads, issueComments);

            if (!issues.Any())
            {
                this.log.Information("No new issues were posted");
                return;
            }

            // Remove issues that cannot be posted
            var issueFilterer = new IssueFilterer(this.log, this.pullRequestSystem, this.settings);
            var remainingIssues = issueFilterer.FilterIssues(issues, issueComments).ToList();

            if (remainingIssues.Any())
            {
                var formattedMessages =
                    from issue in remainingIssues
                    select
                        string.Format(
                            CultureInfo.InvariantCulture,
                            "  Rule: {0} Line: {1} File: {2}",
                            issue.Rule,
                            issue.Line,
                            issue.AffectedFileRelativePath);

                this.log.Verbose(
                    "Posting {0} issue(s):\n{1}",
                    remainingIssues.Count,
                    string.Join(Environment.NewLine, formattedMessages));

                this.pullRequestSystem.PostDiscussionThreads(remainingIssues, this.settings.CommentSource);
            }
            else
            {
                this.log.Information("All issues were filtered. Nothing new to post.");
            }
        }

        /// <summary>
        /// Returns existing matching comments from the pull request for a list of issues.
        /// </summary>
        /// <param name="issues">Issues for which matching comments should be found.</param>
        /// <param name="existingThreads">Existing discussion threads on the pull request.</param>
        /// <returns>Dictionary containing issues and its associated matching comments on the pull request.</returns>
        private IDictionary<ICodeAnalysisIssue, IEnumerable<IPrcaDiscussionComment>> BuildIssueToCommentDictonary(
            IList<ICodeAnalysisIssue> issues,
            IList<IPrcaDiscussionThread> existingThreads)
        {
            issues.NotNull(nameof(issues));
            existingThreads.NotNull(nameof(existingThreads));

            var stopwatch = new Stopwatch();
            stopwatch.Start();

            var result = new Dictionary<ICodeAnalysisIssue, IEnumerable<IPrcaDiscussionComment>>();
            foreach (var issue in issues)
            {
                var matchingComments = this.GetMatchingComments(issue, existingThreads).ToList();

                if (matchingComments.Any())
                {
                    result.Add(issue, matchingComments);
                }
            }

            this.log.Verbose("Built a issue to comment dictionary in {0} ms", stopwatch.ElapsedMilliseconds);

            return result;
        }

        /// <summary>
        /// Returns all matching comments from discussion threads for an issue.
        /// Comments are considered matching if they fulfill all of the following conditions:
        /// * The thread is active.
        /// * The thread is for the same file.
        /// * The thread was created by the same logic, i.e. the same <code>commentSource</code>.
        /// * The comment contains the same content.
        /// </summary>
        /// <remarks>
        /// The line cannot be used since comments can move around.
        /// </remarks>
        /// <param name="issue">Issue for which the comments should be returned.</param>
        /// <param name="existingThreads">Existing discussion threads on the pull request.</param>
        /// <returns>Active comments for the issue.</returns>
        private IEnumerable<IPrcaDiscussionComment> GetMatchingComments(
            ICodeAnalysisIssue issue,
            IList<IPrcaDiscussionThread> existingThreads)
        {
            issue.NotNull(nameof(issue));
            existingThreads.NotNull(nameof(existingThreads));

            // Select threads that are active, that point to the same file and have been marked with the given comment source.
            var matchingThreads =
                (from thread in existingThreads
                where
                    thread != null &&
                    thread.Status == PrcaDiscussionStatus.Active &&
                    FilePathsAreMatching(issue, thread) &&
                    thread.CommentSource == this.settings.CommentSource
                select thread).ToList();

            if (matchingThreads.Any())
            {
                this.log.Verbose(
                    "Found {0} matching thread(s) for the issue at {1} line {2}",
                    matchingThreads.Count,
                    issue.AffectedFileRelativePath,
                    issue.Line);
            }

            var result = new List<IPrcaDiscussionComment>();
            foreach (var thread in matchingThreads)
            {
                // Select comments from this thread that are not deleted and that match the given message.
                var matchingComments =
                    (from comment in thread.Comments
                    where
                        comment != null &&
                        !comment.IsDeleted &&
                        comment.Content == issue.Message
                    select
                        comment).ToList();

                if (matchingComments.Any())
                {
                    this.log.Verbose(
                        "Found {0} matching comment(s) for the issue at {1} line {2}",
                        matchingComments.Count,
                        issue.AffectedFileRelativePath,
                        issue.Line);
                }

                result.AddRange(matchingComments);
            }

            return result;
        }

        /// <summary>
        /// Marks comment threads created by this logic but without active issues as resolved.
        /// </summary>
        /// <param name="existingThreads">Existing discussion threads on the pull request.</param>
        /// <param name="issueComments">Issues and their related comments.</param>
        private void ResolveExistingComments(
            IList<IPrcaDiscussionThread> existingThreads,
            IDictionary<ICodeAnalysisIssue, IEnumerable<IPrcaDiscussionComment>> issueComments)
        {
            existingThreads.NotNull(nameof(existingThreads));
            issueComments.NotNull(nameof(issueComments));

            if (!existingThreads.Any())
            {
                this.log.Verbose("No existings threads to resolve.");
                return;
            }

            var resolvedThreads =
                this.GetResolvedThreads(existingThreads, issueComments).ToList();

            this.log.Verbose("Mark {0} threads as fixed...", resolvedThreads.Count);
            this.pullRequestSystem.MarkThreadsAsFixed(resolvedThreads);
        }

        /// <summary>
        /// Returns threads that can be resolved.
        /// </summary>
        /// <param name="existingThreads">Existing discussion threads on the pull request.</param>
        /// <param name="issueComments">Issues and their related comments.</param>
        /// <returns>List of threads which can be resolved.</returns>
        private IEnumerable<IPrcaDiscussionThread> GetResolvedThreads(
            IList<IPrcaDiscussionThread> existingThreads,
            IDictionary<ICodeAnalysisIssue, IEnumerable<IPrcaDiscussionComment>> issueComments)
        {
            existingThreads.NotNull(nameof(existingThreads));
            issueComments.NotNull(nameof(issueComments));

            var currentComments = new HashSet<IPrcaDiscussionComment>(issueComments.Values.SelectMany(x => x));

            var result =
                existingThreads.Where(thread => !thread.Comments.Any(x => currentComments.Contains(x))).ToList();

            this.log.Verbose(
                "Found {0} existing thread(s) that do not match any new issue and can be resolved.",
                result.Count);

            return result;
        }
    }
}
