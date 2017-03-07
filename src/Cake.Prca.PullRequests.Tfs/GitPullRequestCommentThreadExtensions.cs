namespace Cake.Prca.PullRequests.Tfs
{
    using System;
    using System.Linq;
    using Core.IO;
    using Microsoft.TeamFoundation.SourceControl.WebApi;

    /// <summary>
    /// Extensions for <see cref="GitPullRequestCommentThread"/>.
    /// </summary>
    internal static class GitPullRequestCommentThreadExtensions
    {
        private const string CommentSourcePropertyName = "CommentSource";

        /// <summary>
        /// Converts a <see cref="GitPullRequestCommentThread"/> from TFS to a <see cref="IPrcaDiscussionThread"/> as used in this addin.
        /// </summary>
        /// <param name="thread">TFS thread to convert.</param>
        /// <returns>Converted thread.</returns>
        public static IPrcaDiscussionThread ToPrcaDiscussionThread(this GitPullRequestCommentThread thread)
        {
            thread.NotNull(nameof(thread));

            return new PrcaDiscussionThread(
                thread.Id,
                thread.Status.ToPrcaDiscussionStatus(),
                new FilePath(thread.ThreadContext.FilePath),
                thread.Comments.Select(x => x.ToPrcaDiscussionComment()))
            {
                CommentSource = thread.GetCommentSource(),
            };
        }

        /// <summary>
        /// Gets the comment source value used to decorate comments created by this addin.
        /// </summary>
        /// <param name="thread">Thread to get the value from.</param>
        /// <returns>Comment source value.</returns>
        public static string GetCommentSource(this GitPullRequestCommentThread thread)
        {
            thread.NotNull(nameof(thread));

            return thread.Properties.GetValue(CommentSourcePropertyName, string.Empty);
        }

        /// <summary>
        /// Sets the comment source value used to decorate comments created by this addin.
        /// </summary>
        /// <param name="thread">Thread for which the value should be set.</param>
        /// <param name="value">Value to set as comment source.</param>
        public static void SetCommentSource(this GitPullRequestCommentThread thread, string value)
        {
            thread.NotNull(nameof(thread));

            if (thread.Properties == null)
            {
                throw new InvalidOperationException("Properties collection is not created.");
            }

            if (thread.Properties.ContainsKey(CommentSourcePropertyName))
            {
                thread.Properties[CommentSourcePropertyName] = value;
            }

            thread.Properties.Add(CommentSourcePropertyName, value);
        }

        /// <summary>
        /// Checks if the custom comment source value used to decorate comments created by this addin
        /// has a specific value.
        /// </summary>
        /// <param name="thread">Thread to check.</param>
        /// <param name="value">Value to check for.</param>
        /// <returns><c>True</c> if the value is identical, <c>False</c> otherwise.</returns>
        public static bool IsCommentSource(this GitPullRequestCommentThread thread, string value)
        {
            thread.NotNull(nameof(thread));

            return thread.GetCommentSource() == value;
        }
    }
}
