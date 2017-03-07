namespace Cake.Prca.Tests
{
    using System.Collections.Generic;
    using Core.Diagnostics;
    using Core.IO;
    using Prca.Issues;
    using PullRequests;

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
            discussionThreads.NotNull(nameof(discussionThreads));
            modifiedFiles.NotNull(nameof(modifiedFiles));

            this.discussionThreads.AddRange(discussionThreads);
            this.modifiedFiles.AddRange(modifiedFiles);
        }

        public IEnumerable<IPrcaDiscussionThread> ThreadsMarkedAsFixed => this.threadsMarkedAsFixed;

        public IEnumerable<ICodeAnalysisIssue> PostedIssues => this.postedIssues;

        public override IEnumerable<IPrcaDiscussionThread> FetchActiveDiscussionThreads(string commentSource)
        {
            return this.discussionThreads;
        }

        public override IEnumerable<FilePath> GetModifiedFilesInPullRequest()
        {
            return this.modifiedFiles;
        }

        public override void MarkThreadAsFixed(IPrcaDiscussionThread thread)
        {
            this.threadsMarkedAsFixed.Add(thread);
        }

        public override void PostDiscussionThreads(IEnumerable<ICodeAnalysisIssue> issues, string commentSource)
        {
            this.postedIssues.AddRange(issues);
        }
    }
}
