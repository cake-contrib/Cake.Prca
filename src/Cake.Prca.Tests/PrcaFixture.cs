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
                new ReportIssuesToPullRequestSettings(
                    new Core.IO.DirectoryPath(@"c:\Source\Cake.Prca"));
        }

        public FakeLog Log { get; set; }

        public IList<FakeCodeAnalysisProvider> CodeAnalysisProviders { get; set; }

        public FakePullRequestSystem PullRequestSystem { get; set; }

        public PrcaSettings Settings { get; set; }

        public ReportIssuesToPullRequestSettings ReportIssuesToPullRequestSettings =>
            this.Settings as ReportIssuesToPullRequestSettings;

        public PrcaResult RunOrchestrator()
        {
            var orchestrator =
                new Orchestrator(
                    this.Log,
                    this.CodeAnalysisProviders,
                    this.PullRequestSystem,
                    this.ReportIssuesToPullRequestSettings);
            return orchestrator.Run();
        }

        public IEnumerable<ICodeAnalysisIssue> ReadIssues(PrcaCommentFormat format)
        {
            var issueReader = new IssueReader(this.Log, this.CodeAnalysisProviders, this.Settings);
            return issueReader.ReadIssues(format);
        }

        public IEnumerable<ICodeAnalysisIssue> FilterIssues(
            IEnumerable<ICodeAnalysisIssue> issues,
            IDictionary<ICodeAnalysisIssue, IEnumerable<IPrcaDiscussionComment>> issueComments)
        {
            this.PullRequestSystem?.Initialize(this.ReportIssuesToPullRequestSettings);

            var issueFilterer =
                new IssueFilterer(
                    this.Log,
                    this.PullRequestSystem,
                    this.ReportIssuesToPullRequestSettings);
            return issueFilterer.FilterIssues(issues, issueComments);
        }
    }
}
