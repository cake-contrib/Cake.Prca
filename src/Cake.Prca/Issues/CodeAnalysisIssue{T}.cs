namespace Cake.Prca.Issues
{
    using System;

    /// <summary>
    /// Base class for a code analysis issue.
    /// </summary>
    /// <typeparam name="T">Type of the issue provider which has raised the issue.</typeparam>
    public class CodeAnalysisIssue<T> : CodeAnalysisIssue
        where T : ICodeAnalysisProvider
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="CodeAnalysisIssue{T}"/> class.
        /// </summary>
        /// <param name="filePath">The path to the file affacted by the issue.
        /// The path needs to be relative to the repository root.
        /// <c>null</c> or <see cref="string.Empty"/> if issue is not related to a change in a file.</param>
        /// <param name="line">The line in the file where the issues has occurred.
        /// Nothing if the issue affects the whole file or an asssembly.</param>
        /// <param name="message">The message of the code analysis issue.</param>
        /// <param name="priority">The priority of the message used to filter out issues if there are more issues than
        /// should be posted.</param>
        /// <param name="rule">The rule of the code analysis issue.</param>
        public CodeAnalysisIssue(
            string filePath,
            int? line,
            string message,
            int priority,
            string rule)
            : base(filePath, line, message, priority, rule, typeof(T).FullName)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="CodeAnalysisIssue{T}"/> class.
        /// </summary>
        /// <param name="filePath">The path to the file affacted by the issue.
        /// The path needs to be relative to the repository root.
        /// <c>null</c> or <see cref="string.Empty"/> if issue is not related to a change in a file.</param>
        /// <param name="line">The line in the file where the issues has occurred.
        /// Nothing if the issue affects the whole file or an asssembly.</param>
        /// <param name="message">The message of the code analysis issue.</param>
        /// <param name="priority">The priority of the message used to filter out issues if there are more issues than
        /// should be posted.</param>
        /// <param name="rule">The rule of the code analysis issue.</param>
        /// <param name="ruleUrl">The URL containing information about the failing rule.</param>
        public CodeAnalysisIssue(
            string filePath,
            int? line,
            string message,
            int priority,
            string rule,
            Uri ruleUrl)
            : base(filePath, line, message, priority, rule, ruleUrl, typeof(T).FullName)
        {
        }
    }
}
