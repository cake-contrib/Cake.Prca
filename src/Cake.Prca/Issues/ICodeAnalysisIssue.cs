namespace Cake.Prca.Issues
{
    using Core.IO;

    /// <summary>
    /// Description of a code analysis issue.
    /// </summary>
    public interface ICodeAnalysisIssue
    {
        /// <summary>
        /// Gets the path to the file affacted by the issue.
        /// The path is relative to the repository root.
        /// </summary>
        FilePath AffectedFileRelativePath { get; }

        /// <summary>
        /// Gets the line in the file where the issues has occurred.
        /// </summary>
        int Line { get; }

        /// <summary>
        /// Gets the message of the code analysis issue.
        /// </summary>
        string Message { get; }

        /// <summary>
        /// Gets the priority of the message used to filter out issues if there are more issues than should be posted.
        /// </summary>
        int Priority { get; }

        /// <summary>
        /// Gets the rule of the code analysis issue.
        /// </summary>
        string Rule { get; }
    }
}