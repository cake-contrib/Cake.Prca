namespace Cake.Prca.Tests
{
    using System.Collections.Generic;
    using Core.Diagnostics;
    using Prca.Issues;

    public class FakeCodeAnalysisProvider : CodeAnalysisProvider
    {
        private readonly List<ICodeAnalysisIssue> issues = new List<ICodeAnalysisIssue>();

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

        public new ReportIssuesToPullRequestSettings PrcaSettings => base.PrcaSettings;

        public PrcaCommentFormat Format { get; private set; }

        protected override IEnumerable<ICodeAnalysisIssue> InternalReadIssues(PrcaCommentFormat format)
        {
            this.Format = format;
            return this.issues;
        }
    }
}
