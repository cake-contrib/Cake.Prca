namespace Cake.Prca.Tests
{
    using System.Collections.Generic;
    using Prca.Issues;
    using Shouldly;
    using Xunit;

    public class PrcaResultTests
    {
        public sealed class TheCtor
        {
            [Fact]
            public void Should_Throw_If_ReportedIssues_Is_Null()
            {
                // Given
                IEnumerable<ICodeAnalysisIssue> reportedIssues = null;
                IEnumerable<ICodeAnalysisIssue> postedIssues = new List<ICodeAnalysisIssue>();

                // When
                var result = Record.Exception(() => new PrcaResult(reportedIssues, postedIssues));

                // Then
                result.IsArgumentNullException("reportedIssues");
            }

            [Fact]
            public void Should_Throw_If_PostedIssues_Is_Null()
            {
                // Given
                IEnumerable<ICodeAnalysisIssue> reportedIssues = new List<ICodeAnalysisIssue>();
                IEnumerable<ICodeAnalysisIssue> postedIssues = null;

                // When
                var result = Record.Exception(() => new PrcaResult(reportedIssues, postedIssues));

                // Then
                result.IsArgumentNullException("postedIssues");
            }

            [Fact]
            public void Should_Set_ReportedIssues()
            {
                // Given
                IEnumerable<ICodeAnalysisIssue> reportedIssues = new List<ICodeAnalysisIssue>();
                IEnumerable<ICodeAnalysisIssue> postedIssues = new List<ICodeAnalysisIssue>();

                // When
                var result = new PrcaResult(reportedIssues, postedIssues);

                // Then
                result.ReportedIssues.ShouldBe(reportedIssues);
            }

            [Fact]
            public void Should_Set_PostedIssues()
            {
                // Given
                IEnumerable<ICodeAnalysisIssue> reportedIssues = new List<ICodeAnalysisIssue>();
                IEnumerable<ICodeAnalysisIssue> postedIssues = new List<ICodeAnalysisIssue>();

                // When
                var result = new PrcaResult(reportedIssues, postedIssues);

                // Then
                result.PostedIssues.ShouldBe(postedIssues);
            }
        }
    }
}
