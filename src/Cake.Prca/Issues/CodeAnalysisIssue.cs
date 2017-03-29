namespace Cake.Prca.Issues
{
    using System;
    using Core.IO;

    /// <summary>
    /// Base class for a code analysis issue.
    /// </summary>
    public class CodeAnalysisIssue : ICodeAnalysisIssue
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="CodeAnalysisIssue"/> class.
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
        /// <param name="providerType">The type of the issue provider.</param>
        public CodeAnalysisIssue(
            string filePath,
            int? line,
            string message,
            int priority,
            string rule,
            string providerType)
            : this(filePath, line, message, priority, rule, null, providerType)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="CodeAnalysisIssue"/> class.
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
        /// <param name="providerType">The type of the issue provider.</param>
        public CodeAnalysisIssue(
            string filePath,
            int? line,
            string message,
            int priority,
            string rule,
            Uri ruleUrl,
            string providerType)
        {
            line?.NotNegativeOrZero(nameof(line));
            message.NotNullOrWhiteSpace(nameof(message));
            rule.NotNull(nameof(rule));
            providerType.NotNullOrWhiteSpace(nameof(providerType));

            // File path needs to be relative to the repository root.
            if (!string.IsNullOrWhiteSpace(filePath))
            {
                if (!filePath.IsValidPath())
                {
                    throw new ArgumentException("Invalid path", nameof(filePath));
                }

                this.AffectedFileRelativePath = filePath;

                if (!this.AffectedFileRelativePath.IsRelative)
                {
                    throw new ArgumentOutOfRangeException(nameof(filePath), "File path needs to be relative to the repository root.");
                }
            }

            if (this.AffectedFileRelativePath == null && line.HasValue)
            {
                throw new ArgumentOutOfRangeException(nameof(line), "Cannot specify a line while not specifying a file.");
            }

            this.Line = line;
            this.Message = message;
            this.Priority = priority;
            this.Rule = rule;
            this.RuleUrl = ruleUrl;
            this.ProviderType = providerType;
        }

        /// <inheritdoc/>
        public FilePath AffectedFileRelativePath { get; }

        /// <inheritdoc/>
        public int? Line { get; }

        /// <inheritdoc/>
        public string Message { get; }

        /// <inheritdoc/>
        public int Priority { get; }

        /// <inheritdoc/>
        public string Rule { get; }

        /// <inheritdoc/>
        public Uri RuleUrl { get; }

        /// <inheritdoc/>
        public string ProviderType { get; }
    }
}
