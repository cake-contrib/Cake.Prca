namespace Cake.Prca.Issues
{
    using System.Collections.Generic;

    /// <summary>
    /// Interface describing a provider for code analysis issues..
    /// </summary>
    public interface ICodeAnalysisProvider
    {
        /// <summary>
        /// Gets all code analysis issues.
        /// </summary>
        /// <returns>List of code analysis issues</returns>
        IEnumerable<ICodeAnalysisIssue> ReadIssues();
    }
}
