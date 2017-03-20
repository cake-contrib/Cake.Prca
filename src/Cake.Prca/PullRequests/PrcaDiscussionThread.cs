namespace Cake.Prca.PullRequests
{
    using System.Collections.Generic;
    using Core.IO;

    /// <summary>
    /// Base class for a collection of comments relating to each other.
    /// </summary>
    public class PrcaDiscussionThread : IPrcaDiscussionThread
    {
        private readonly List<IPrcaDiscussionComment> comments = new List<IPrcaDiscussionComment>();

        /// <summary>
        /// Initializes a new instance of the <see cref="PrcaDiscussionThread"/> class.
        /// </summary>
        /// <param name="id">ID of the discussion thread.</param>
        /// <param name="status">A value if the thread is active or already fixed.</param>
        /// <param name="affectedFileRelativePath">The path to the file where the message should be posted.
        /// The path needs to be relative to the repository root.</param>
        /// <param name="comments">All the comments of this thread.</param>
        public PrcaDiscussionThread(
            int id,
            PrcaDiscussionStatus status,
            FilePath affectedFileRelativePath,
            IEnumerable<IPrcaDiscussionComment> comments)
        {
            // ReSharper disable once PossibleMultipleEnumeration
            comments.NotNull(nameof(comments));
            affectedFileRelativePath.NotNull(nameof(affectedFileRelativePath));

            this.Id = id;
            this.Status = status;
            this.AffectedFileRelativePath = affectedFileRelativePath;

            // ReSharper disable once PossibleMultipleEnumeration
            this.comments.AddRange(comments);
        }

        /// <inheritdoc/>
        public int Id { get; set; }

        /// <inheritdoc/>
        public PrcaDiscussionStatus Status { get; set; }

        /// <inheritdoc/>
        public FilePath AffectedFileRelativePath { get; set; }

        /// <inheritdoc/>
        public IList<IPrcaDiscussionComment> Comments => this.comments;

        /// <inheritdoc/>
        public string CommentSource { get; set; }
    }
}
