namespace Cake.Prca.Tests
{
    using System.Collections.Generic;
    using Core.Diagnostics;
    using Prca.Issues;
    using Prca.PullRequests;
    using Testing;

    public class PrcaFixture
    {
        public PrcaFixture()
        {
            this.Log = new FakeLog();
            this.Log.Verbosity = Verbosity.Normal;
            this.CodeAnalysisProvider = new FakeCodeAnalysisProvider(this.Log);
            this.PullRequestSystem = new FakePullRequestSystem(this.Log);
            this.Settings = new ReportCodeAnalysisIssuesToPullRequestSettings();
        }

        public FakeLog Log { get; set; }

        public FakeCodeAnalysisProvider CodeAnalysisProvider { get; set; }

        public FakePullRequestSystem PullRequestSystem { get; set; }

        public ReportCodeAnalysisIssuesToPullRequestSettings Settings { get; set; }

        public void RunOrchestrator()
        {
            var orchestrator = new Orchestrator(this.Log, this.CodeAnalysisProvider, this.PullRequestSystem, this.Settings);
            orchestrator.Run();
        }

        public IEnumerable<ICodeAnalysisIssue> FilterIssues(
            IEnumerable<ICodeAnalysisIssue> issues,
            IDictionary<ICodeAnalysisIssue, IEnumerable<IPrcaDiscussionComment>> issueComments)
        {
            var issueFilterer = new IssueFilterer(this.Log, this.PullRequestSystem, this.Settings);
            return issueFilterer.FilterIssues(issues, issueComments);
        }
    }
}
