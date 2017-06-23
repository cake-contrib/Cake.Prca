namespace Cake.Prca
{
    using System.Collections.Generic;
    using System.Linq;
    using Core.Diagnostics;
    using Issues;

    /// <summary>
    /// Class for reading issues.
    /// </summary>
    internal class IssueReader
    {
        private readonly ICakeLog log;
        private readonly List<ICodeAnalysisProvider> issueProviders = new List<ICodeAnalysisProvider>();
        private readonly PrcaSettings settings;

        /// <summary>
        /// Initializes a new instance of the <see cref="IssueReader"/> class.
        /// </summary>
        /// <param name="log">Cake log instance.</param>
        /// <param name="issueProviders">List of issue providers to use.</param>
        /// <param name="settings">Settings to use.</param>
        public IssueReader(
            ICakeLog log,
            IEnumerable<ICodeAnalysisProvider> issueProviders,
            PrcaSettings settings)
        {
            log.NotNull(nameof(log));
            settings.NotNull(nameof(settings));

            // ReSharper disable once PossibleMultipleEnumeration
            issueProviders.NotNullOrEmptyOrEmptyElement(nameof(issueProviders));

            this.log = log;
            this.settings = settings;

            // ReSharper disable once PossibleMultipleEnumeration
            this.issueProviders.AddRange(issueProviders);
        }

        /// <summary>
        /// Read issues from issue providers.
        /// </summary>
        /// <param name="format">Preferred format for comments.</param>
        /// <returns>List of issues.</returns>
        public IEnumerable<ICodeAnalysisIssue> ReadIssues(PrcaCommentFormat format)
        {
            // Initialize issue providers and read issues.
            var issues = new List<ICodeAnalysisIssue>();
            foreach (var codeAnalysisProvider in this.issueProviders)
            {
                var providerName = codeAnalysisProvider.GetType().Name;
                this.log.Verbose("Initialize code analysis provider {0}...", providerName);
                if (codeAnalysisProvider.Initialize(this.settings))
                {
                    this.log.Verbose("Reading issues from {0}...", providerName);
                    var currentIssues = codeAnalysisProvider.ReadIssues(format).ToList();

                    this.log.Verbose(
                        "Found {0} issues using issue provider {1}...",
                        currentIssues.Count,
                        providerName);

                    issues.AddRange(currentIssues);
                }
                else
                {
                    this.log.Warning("Error initializing issue provider {0}.", providerName);
                }
            }

            return issues;
        }
    }
}
