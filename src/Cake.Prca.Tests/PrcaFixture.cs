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
            this.Log = new FakeLog { Verbosity = Verbosity.Normal };
            this.CodeAnalysisProviders = new List<FakeCodeAnalysisProvider> { new FakeCodeAnalysisProvider(this.Log) };
            this.PullRequestSystem = new FakePullRequestSystem(this.Log);
            this.Settings =
                new ReportCodeAnalysisIssuesToPullRequestSettings(
                    new Core.IO.DirectoryPath(@"c:\Source\Cake.Prca"));
        }

        public FakeLog Log { get; set; }

        public IList<FakeCodeAnalysisProvider> CodeAnalysisProviders { get; set; }

        public FakePullRequestSystem PullRequestSystem { get; set; }

        public ReportCodeAnalysisIssuesToPullRequestSettings Settings { get; set; }

        public void RunOrchestrator()
        {
            var orchestrator =
                new Orchestrator(
                    this.Log,
                    this.CodeAnalysisProviders,
                    this.PullRequestSystem,
                    this.Settings);
            orchestrator.Run();
        }

        public IEnumerable<ICodeAnalysisIssue> FilterIssues(
            IEnumerable<ICodeAnalysisIssue> issues,
            IDictionary<ICodeAnalysisIssue, IEnumerable<IPrcaDiscussionComment>> issueComments)
        {
            this.PullRequestSystem?.Initialize(this.Settings);

            var issueFilterer = new IssueFilterer(this.Log, this.PullRequestSystem, this.Settings);
            return issueFilterer.FilterIssues(issues, issueComments);
        }
    }
}
