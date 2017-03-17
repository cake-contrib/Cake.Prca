namespace Cake.Prca.Issues
{
    using System.Collections.Generic;
    using Core.Diagnostics;

    /// <summary>
    /// Base class for all code analysis provider implementations.
    /// </summary>
    public abstract class CodeAnalysisProvider : ICodeAnalysisProvider
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="CodeAnalysisProvider"/> class.
        /// </summary>
        /// <param name="log">The Cake log context.</param>
        protected CodeAnalysisProvider(ICakeLog log)
        {
            log.NotNull(nameof(log));

            this.Log = log;
        }

        /// <summary>
        /// Gets the Cake log context.
        /// </summary>
        protected ICakeLog Log { get; }

        /// <inheritdoc/>
        public abstract IEnumerable<ICodeAnalysisIssue> ReadIssues();
    }
}
