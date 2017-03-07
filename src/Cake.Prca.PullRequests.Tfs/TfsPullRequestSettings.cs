namespace Cake.Prca.PullRequests.Tfs
{
    using System;

    /// <summary>
    /// Settings for <see cref="TfsPullRequestSystem"/>.
    /// </summary>
    public class TfsPullRequestSettings
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="TfsPullRequestSettings"/> class.
        /// </summary>
        /// <param name="repositoryUrl">Full URL of the Git repository, eg. <code>http://myserver:8080/tfs/defaultcollection/myproject/_git/myrepository</code>.</param>
        /// <param name="sourceBranch">Branch for which the pull request is made.</param>
        public TfsPullRequestSettings(Uri repositoryUrl, string sourceBranch)
        {
            repositoryUrl.NotNull(nameof(repositoryUrl));
            sourceBranch.NotNullOrWhiteSpace(nameof(sourceBranch));

            this.RepositoryUrl = repositoryUrl;
            this.SourceBranch = sourceBranch;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="TfsPullRequestSettings"/> class.
        /// </summary>
        /// <param name="repositoryUrl">Full URL of the Git repository, eg. <code>http://myserver:8080/tfs/defaultcollection/myproject/_git/myrepository</code>.</param>
        /// <param name="pullRequestId">ID of the pull request.</param>
        public TfsPullRequestSettings(Uri repositoryUrl, int pullRequestId)
        {
            repositoryUrl.NotNull(nameof(repositoryUrl));

            this.RepositoryUrl = repositoryUrl;
            this.PullRequestId = pullRequestId;
        }

        /// <summary>
        /// Gets the full URL of the Git repository, eg. <code>http://myserver:8080/tfs/defaultcollection/myproject/_git/myrepository</code>.
        /// </summary>
        public Uri RepositoryUrl { get; private set; }

        /// <summary>
        /// Gets the branch for which the pull request is made.
        /// </summary>
        public string SourceBranch { get; private set; }

        /// <summary>
        /// Gets the ID of the pull request.
        /// </summary>
        public int? PullRequestId { get; private set; }
    }
}
