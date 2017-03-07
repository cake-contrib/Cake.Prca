namespace Cake.Prca.PullRequests.Tfs
{
    using System;
    using System.Linq;

    /// <summary>
    /// Describes the different parts of a repository URL.
    /// </summary>
    internal class RepositoryDescription
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="RepositoryDescription"/> class.
        /// </summary>
        /// <param name="repoUrl">Full URL of the repository, eg. <code>http://myserver:8080/tfs/defaultcollection/myproject/_git/myrepository</code></param>
        public RepositoryDescription(Uri repoUrl)
        {
            repoUrl.NotNull(nameof(repoUrl));

            var repoUrlString = repoUrl.ToString();
            var gitSeparator = new string[] { "/_git/" };
            var splitPath = repoUrl.AbsolutePath.Split(gitSeparator, StringSplitOptions.None);
            if (splitPath.Count() < 2)
            {
                throw new UriFormatException("No valid Git repository URL.");
            }

            var splitFirstPart = splitPath[0].Split(new char[] { '/' }, StringSplitOptions.RemoveEmptyEntries);
            if (splitFirstPart.Count() < 2)
            {
                throw new UriFormatException("No valid Git repository URL containing default collection and project name.");
            }

            var splitLastPart = splitPath[1].Split(new char[] { '/' }, StringSplitOptions.RemoveEmptyEntries);

            this.CollectionName = splitFirstPart.Reverse().Skip(1).Take(1).Single();
            this.CollectionUrl =
                new Uri(
                    repoUrlString.Substring(
                        0,
                        repoUrlString.IndexOf(this.CollectionName, StringComparison.OrdinalIgnoreCase) + this.CollectionName.Length));
            this.ProjectName = splitFirstPart.Last();
            this.RepositoryName = splitLastPart.First();
        }

        /// <summary>
        /// Gets the name of the Team Foundation Server collection.
        /// </summary>
        public string CollectionName { get; private set; }

        /// <summary>
        /// Gets the URL of the Team Foundation Server collection.
        /// </summary>
        public Uri CollectionUrl { get; private set; }

        /// <summary>
        /// Gets the name of the Team Foundation Server project.
        /// </summary>
        public string ProjectName { get; private set; }

        /// <summary>
        /// Gets the name of the Git repository.
        /// </summary>
        public string RepositoryName { get; private set; }
    }
}
