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
            : base (log)
        {
            this.issues.AddRange(issues);
        }

        public override IEnumerable<ICodeAnalysisIssue> ReadIssues()
        {
            return this.issues;
        }

        public new ICakeLog Log => base.Log;
    }
}
