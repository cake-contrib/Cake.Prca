namespace Cake.Prca.Issues
{
    using System;
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

        /// <summary>
        /// Gets general settings.
        /// Is set after <see cref="Initialize"/> was called from the core addin.
        /// </summary>
        protected ReportIssuesToPullRequestSettings PrcaSettings { get; private set; }

        /// <inheritdoc/>
        public virtual bool Initialize(ReportIssuesToPullRequestSettings settings)
        {
            settings.NotNull(nameof(settings));

            this.PrcaSettings = settings;

            return true;
        }

        /// <inheritdoc/>
        public IEnumerable<ICodeAnalysisIssue> ReadIssues(PrcaCommentFormat format)
        {
            if (this.PrcaSettings == null)
            {
                throw new InvalidOperationException("Initialize needs to be called first.");
            }

            return this.InternalReadIssues(format);
        }

        /// <summary>
        /// Gets all code analysis issues.
        /// Compared to <see cref="ReadIssues"/> it is safe to access <see cref="PrcaSettings"/> from this method.
        /// </summary>
        /// <param name="format">Preferred format of the comments.</param>
        /// <returns>List of code analysis issues</returns>
        protected abstract IEnumerable<ICodeAnalysisIssue> InternalReadIssues(PrcaCommentFormat format);
    }
}
