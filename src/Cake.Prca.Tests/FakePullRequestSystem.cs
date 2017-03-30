namespace Cake.Prca.Tests
{
    using System.Collections.Generic;
    using Core.Diagnostics;
    using Core.IO;
    using Prca.Issues;
    using Prca.PullRequests;

    public class FakePullRequestSystem : PullRequestSystem
    {
        private readonly List<IPrcaDiscussionThread> discussionThreads = new List<IPrcaDiscussionThread>();
        private readonly List<FilePath> modifiedFiles = new List<FilePath>();
        private readonly List<IPrcaDiscussionThread> threadsMarkedAsFixed = new List<IPrcaDiscussionThread>();
        private readonly List<ICodeAnalysisIssue> postedIssues = new List<ICodeAnalysisIssue>();

        public FakePullRequestSystem(ICakeLog log)
            : base(log)
        {
        }

        public FakePullRequestSystem(
            ICakeLog log,
            IEnumerable<IPrcaDiscussionThread> discussionThreads,
            IEnumerable<FilePath> modifiedFiles)
            : base(log)
        {
            // ReSharper disable once PossibleMultipleEnumeration
            discussionThreads.NotNull(nameof(discussionThreads));

            // ReSharper disable once PossibleMultipleEnumeration
            modifiedFiles.NotNull(nameof(modifiedFiles));

            // ReSharper disable once PossibleMultipleEnumeration
            this.discussionThreads.AddRange(discussionThreads);

            // ReSharper disable once PossibleMultipleEnumeration
            this.modifiedFiles.AddRange(modifiedFiles);
        }

        public new ICakeLog Log => base.Log;

        public new ReportCodeAnalysisIssuesToPullRequestSettings PrcaSettings => base.PrcaSettings;

        public IEnumerable<IPrcaDiscussionThread> ThreadsMarkedAsFixed => this.threadsMarkedAsFixed;

        public IEnumerable<ICodeAnalysisIssue> PostedIssues => this.postedIssues;

        public PrcaCommentFormat CommentFormat { get; set; } = PrcaCommentFormat.PlainText;

        public override PrcaCommentFormat GetPreferredCommentFormat()
        {
            return this.CommentFormat;
        }

        protected override IEnumerable<IPrcaDiscussionThread> InternalFetchActiveDiscussionThreads(string commentSource)
        {
            return this.discussionThreads;
        }

        protected override IEnumerable<FilePath> InternalGetModifiedFilesInPullRequest()
        {
            return this.modifiedFiles;
        }

        protected override void InternalMarkThreadsAsFixed(IEnumerable<IPrcaDiscussionThread> threads)
        {
            // ReSharper disable once PossibleMultipleEnumeration
            threads.NotNull(nameof(threads));

            // ReSharper disable once PossibleMultipleEnumeration
            this.threadsMarkedAsFixed.AddRange(threads);
        }

        protected override void InternalPostDiscussionThreads(IEnumerable<ICodeAnalysisIssue> issues, string commentSource)
        {
            // ReSharper disable once PossibleMultipleEnumeration
            issues.NotNull(nameof(issues));

            // ReSharper disable once PossibleMultipleEnumeration
            this.postedIssues.AddRange(issues);
        }
    }
}
