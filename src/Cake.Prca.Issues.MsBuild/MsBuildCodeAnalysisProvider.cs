namespace Cake.Prca.Issues.MsBuild
{
    using System.Collections.Generic;
    using Core.Diagnostics;

    /// <summary>
    /// Provider for code analysis issues reported as MsBuild warnings.
    /// </summary>
    public class MsBuildCodeAnalysisProvider : CodeAnalysisProvider
    {
        private readonly MsBuildCodeAnalysisSettings settings;

        /// <summary>
        /// Initializes a new instance of the <see cref="MsBuildCodeAnalysisProvider"/> class.
        /// </summary>
        /// <param name="log">The Cake log context.</param>
        /// <param name="settings">Settings for reading the log file.</param>
        public MsBuildCodeAnalysisProvider(ICakeLog log, MsBuildCodeAnalysisSettings settings)
            : base(log)
        {
            settings.NotNull(nameof(settings));

            this.settings = settings;
        }

        /// <inheritdoc />
        public override IEnumerable<ICodeAnalysisIssue> ReadIssues()
        {
            return this.settings.Format.ReadIssues(this.settings);
        }
    }
}
