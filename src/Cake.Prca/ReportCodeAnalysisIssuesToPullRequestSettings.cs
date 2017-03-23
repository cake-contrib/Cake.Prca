namespace Cake.Prca
{
    using System.IO;
    using Core.IO;

    /// <summary>
    /// Settings affecting how code analysis issues are reported to pull requests.
    /// </summary>
    public class ReportCodeAnalysisIssuesToPullRequestSettings
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ReportCodeAnalysisIssuesToPullRequestSettings"/> class.
        /// </summary>
        /// <param name="repositoryRoot">Root path of the repository.</param>
        public ReportCodeAnalysisIssuesToPullRequestSettings(DirectoryPath repositoryRoot)
        {
            repositoryRoot.NotNull(nameof(repositoryRoot));

            this.RepositoryRoot = repositoryRoot;
        }

        /// <summary>
        /// Gets the Root path of the repository.
        /// </summary>
        public DirectoryPath RepositoryRoot { get; private set; }

        /// <summary>
        /// Gets or sets the number of issues which should be posted at maximum.
        /// </summary>
        public int MaxIssuesToPost { get; set; } = 100;

        /// <summary>
        /// Gets or sets a value used to decorate comments created by this addin.
        /// Only comments with the same source will be resolved.
        /// </summary>
        public string CommentSource { get; set; } = "CakePrca";
    }
}
