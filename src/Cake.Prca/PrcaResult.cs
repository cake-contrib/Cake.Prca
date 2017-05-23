namespace Cake.Prca
{
    using System.Collections.Generic;
    using Issues;

    /// <summary>
    /// Result from a pull request code analysis call.
    /// </summary>
    public class PrcaResult
    {
        private readonly List<ICodeAnalysisIssue> reportedIssues = new List<ICodeAnalysisIssue>();
        private readonly List<ICodeAnalysisIssue> postedIssues = new List<ICodeAnalysisIssue>();

        /// <summary>
        /// Initializes a new instance of the <see cref="PrcaResult"/> class.
        /// </summary>
        /// <param name="reportedIssues">Issues reported by the issue providers.</param>
        /// <param name="postedIssues">Issues posted to the pull request.</param>
        public PrcaResult(
            IEnumerable<ICodeAnalysisIssue> reportedIssues,
            IEnumerable<ICodeAnalysisIssue> postedIssues)
        {
            // ReSharper disable once PossibleMultipleEnumeration
            reportedIssues.NotNull(nameof(reportedIssues));

            // ReSharper disable once PossibleMultipleEnumeration
            postedIssues.NotNull(nameof(postedIssues));

            // ReSharper disable once PossibleMultipleEnumeration
            this.reportedIssues.AddRange(reportedIssues);

            // ReSharper disable once PossibleMultipleEnumeration
            this.postedIssues.AddRange(postedIssues);
        }

        /// <summary>
        /// Gets all issues reported by the issue providers.
        /// </summary>
        public IEnumerable<ICodeAnalysisIssue> ReportedIssues => this.reportedIssues.AsReadOnly();

        /// <summary>
        /// Gets the issues posted to the pull request.
        /// </summary>
        public IEnumerable<ICodeAnalysisIssue> PostedIssues => this.postedIssues.AsReadOnly();
    }
}
