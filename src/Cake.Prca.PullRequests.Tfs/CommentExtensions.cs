namespace Cake.Prca.PullRequests.Tfs
{
    using Microsoft.TeamFoundation.SourceControl.WebApi;

    /// <summary>
    /// Extensions for <see cref="Comment"/>.
    /// </summary>
    internal static class CommentExtensions
    {
        /// <summary>
        /// Converts a <see cref="Comment"/> from TFS to a <see cref="IPrcaDiscussionComment"/> as used in this addin.
        /// </summary>
        /// <param name="comment">TFS comment to convert.</param>
        /// <returns>Converted comment.</returns>
        public static IPrcaDiscussionComment ToPrcaDiscussionComment(this Comment comment)
        {
            comment.NotNull(nameof(comment));

            return new PrcaDiscussionComment()
            {
                Content = comment.Content,
                IsDeleted = comment.IsDeleted
            };
        }
    }
}
