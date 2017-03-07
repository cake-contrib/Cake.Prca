namespace Cake.Prca.PullRequests
{
    /// <summary>
    /// Base class for a comment.
    /// </summary>
    public class PrcaDiscussionComment : IPrcaDiscussionComment
    {
        /// <inheritdoc/>
        public string Content { get; set; }

        /// <inheritdoc/>
        public bool IsDeleted { get; set; }
    }
}
