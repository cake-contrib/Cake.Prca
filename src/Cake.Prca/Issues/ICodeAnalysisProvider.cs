namespace Cake.Prca.Issues
{
    using System.Collections.Generic;

    /// <summary>
    /// Interface describing a provider for code analysis issues..
    /// </summary>
    public interface ICodeAnalysisProvider
    {
        /// <summary>
        /// Initializes the code analysis provider.
        /// </summary>
        /// <param name="settings">General settings</param>
        /// <returns><c>true</c> if the initialization was successful, <c>false</c> otherwise.</returns>
        bool Initialize(PrcaSettings settings);

        /// <summary>
        /// Gets all code analysis issues.
        /// </summary>
        /// <param name="format">Preferred format of the comments.</param>
        /// <returns>List of code analysis issues</returns>
        IEnumerable<ICodeAnalysisIssue> ReadIssues(PrcaCommentFormat format);
    }
}
