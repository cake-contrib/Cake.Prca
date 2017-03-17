namespace Cake.Prca.PullRequests
{
    using System.Collections.Generic;
    using Core.IO;

    /// <summary>
    /// Description of a collection of comments relating to each other.
    /// </summary>
    public interface IPrcaDiscussionThread
    {
        /// <summary>
        /// Gets or sets the ID of the discussion thread.
        /// </summary>
        int Id { get; set; }

        /// <summary>
        /// Gets or sets if the thread is active or already fixed.
        /// </summary>
        PrcaDiscussionStatus Status { get; set; }

        /// <summary>
        /// Gets or sets the path to the file where the message should be posted.
        /// The path needs to be relative to the repository root.
        /// </summary>
        FilePath AffectedFileRelativePath { get; set; }

        /// <summary>
        /// Gets or sets a value used to decorate comments created by this addin.
        /// </summary>
        string CommentSource { get; set; }

        /// <summary>
        /// Gets all the comments of this thread.
        /// </summary>
        IList<IPrcaDiscussionComment> Comments { get; }
    }
}
