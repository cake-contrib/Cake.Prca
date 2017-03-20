namespace Cake.Prca
{
    /// <summary>
    /// Settings affecting how code analysis issues are reported to pull requests.
    /// </summary>
    public class ReportCodeAnalysisIssuesToPullRequestSettings
    {
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
