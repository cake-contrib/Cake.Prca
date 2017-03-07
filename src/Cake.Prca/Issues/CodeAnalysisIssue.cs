namespace Cake.Prca.Issues
{
    using Core.IO;

    /// <summary>
    /// Base class for a code analysis issue.
    /// </summary>
    public class CodeAnalysisIssue : ICodeAnalysisIssue
    {
        /// <inheritdoc/>
        public FilePath AffectedFileRelativePath { get; set; }

        /// <inheritdoc/>
        public int Line { get; set; }

        /// <inheritdoc/>
        public string Message { get; set; }

        /// <inheritdoc/>
        public int Priority { get; set; }

        /// <inheritdoc/>
        public string Rule { get; set; }
    }
}
