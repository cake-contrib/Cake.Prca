namespace Cake.Prca
{
    using Core.IO;

    /// <summary>
    /// Basic settings for all PRCA aliases.
    /// </summary>
    public class PrcaSettings
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="PrcaSettings"/> class.
        /// </summary>
        /// <param name="repositoryRoot">Root path of the repository.</param>
        public PrcaSettings(DirectoryPath repositoryRoot)
        {
            repositoryRoot.NotNull(nameof(repositoryRoot));

            this.RepositoryRoot = repositoryRoot;
        }

        /// <summary>
        /// Gets the Root path of the repository.
        /// </summary>
        public DirectoryPath RepositoryRoot { get; private set; }
    }
}
