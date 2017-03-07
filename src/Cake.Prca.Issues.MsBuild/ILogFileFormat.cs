namespace Cake.Prca.Issues.MsBuild
{
    using System.Collections.Generic;

    /// <summary>
    /// Definition of a MsBuild log file format.
    /// </summary>
    public interface ILogFileFormat
    {
        /// <summary>
        /// Gets all code analysis issues.
        /// </summary>
        /// <param name="settings">Settings to use.</param>
        /// <returns>List of code analysis issues</returns>
        IEnumerable<ICodeAnalysisIssue> ReadIssues(MsBuildCodeAnalysisSettings settings);
    }
}
