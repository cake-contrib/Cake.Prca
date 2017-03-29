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
        /// Can be <c>null</c> if issue is not related to a change in a file.
        /// </summary>
        FilePath AffectedFileRelativePath { get; }

        /// <summary>
        /// Gets the line in the file where the issues has occurred.
        /// Nothing if the issue affects the whole file or an asssembly.
        /// </summary>
        int? Line { get; }

        /// <summary>
        /// Gets the message of the code analysis issue.
        /// </summary>
        string Message { get; }

        /// <summary>
        /// Gets the priority of the message used to filter out issues if there are more issues than should be posted.
        /// Issues with a lower priority are filtered if there are more issues to post than is allowed.
        /// </summary>
        int Priority { get; }

        /// <summary>
        /// Gets the rule of the code analysis issue.
        /// </summary>
        string Rule { get; }

        /// <summary>
        /// Gets the type of the issue provider.
        /// </summary>
        string ProviderType { get; }
    }
}