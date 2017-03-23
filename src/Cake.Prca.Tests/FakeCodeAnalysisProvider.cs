namespace Cake.Prca.Tests
{
    using System.Collections.Generic;
    using Core.Diagnostics;
    using Prca.Issues;

    public class FakeCodeAnalysisProvider : CodeAnalysisProvider
    {
        private readonly List<ICodeAnalysisIssue> issues = new List<ICodeAnalysisIssue>();
        private PrcaCommentFormat format;

        public FakeCodeAnalysisProvider(ICakeLog log)
            : base(log)
        {
        }

        public FakeCodeAnalysisProvider(ICakeLog log, IEnumerable<ICodeAnalysisIssue> issues)
            : base(log)
        {
            // ReSharper disable once PossibleMultipleEnumeration
            issues.NotNull(nameof(issues));

            // ReSharper disable once PossibleMultipleEnumeration
            this.issues.AddRange(issues);
        }

        public new ICakeLog Log => base.Log;

        public ReportCodeAnalysisIssuesToPullRequestSettings PrcaSettings { get; private set; }

        public PrcaCommentFormat Format => this.format;

        public override void Initialize(ReportCodeAnalysisIssuesToPullRequestSettings settings)
        {
            this.PrcaSettings = settings;
        }

        public override IEnumerable<ICodeAnalysisIssue> ReadIssues(PrcaCommentFormat format)
        {
            this.format = format;
            return this.issues;
        }
    }
}
