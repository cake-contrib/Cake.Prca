namespace Cake.Prca.PullRequests
{
    /// <summary>
    /// Description of a comments.
    /// </summary>
    public interface IPrcaDiscussionComment
    {
        /// <summary>
        /// Gets or sets the content of the comment.
        /// </summary>
        string Content { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the comment is deleted or not.
        /// </summary>
        bool IsDeleted { get; set; }
    }
}
