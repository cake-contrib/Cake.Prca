namespace Cake.Prca
{
    using Core.IO;

    /// <summary>
    /// Settings for reading issues.
    /// </summary>
    public class ReadIssuesSettings : PrcaSettings
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ReadIssuesSettings"/> class.
        /// </summary>
        /// <param name="repositoryRoot">Root path of the repository.</param>
        public ReadIssuesSettings(DirectoryPath repositoryRoot)
            : base(repositoryRoot)
        {
        }

        /// <summary>
        /// Gets or sets the preferred format in which issue comments should be returned.
        /// </summary>
        public PrcaCommentFormat Format { get; set; } = PrcaCommentFormat.Undefined;
    }
}
