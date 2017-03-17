namespace Cake.Prca.PullRequests
{
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using Core.IO;
    using Issues;

    /// <summary>
    /// Interface describing a pull request server.
    /// </summary>
    public interface IPullRequestSystem
    {
        /// <summary>
        /// Returns a list of all active discussion threads.
        /// </summary>
        /// <param name="commentSource">Value used to indicate threads created by this addin.</param>
        /// <returns>List of all active discussion threads.</returns>
        IEnumerable<IPrcaDiscussionThread> FetchActiveDiscussionThreads(string commentSource);

        /// <summary>
        /// Marks a discussion thread as resolved.
        /// </summary>
        /// <param name="thread">Thread to mark as fixed.</param>
        void MarkThreadAsFixed(IPrcaDiscussionThread thread);

        /// <summary>
        /// Returns a list of all files modified in a pull request.
        /// </summary>
        /// <returns>List of all files modified in a pull request.</returns>
        [SuppressMessage(
            "Microsoft.Design",
            "CA1024:UsePropertiesWhereAppropriate",
            Justification = "Most probably will make a remote call")]
        IEnumerable<FilePath> GetModifiedFilesInPullRequest();

        /// <summary>
        /// Posts discussion threads for all issues which need to be posted.
        /// </summary>
        /// <param name="issues">Issues which need to be posted.</param>
        /// <param name="commentSource">Value used to decorate comments created by this addin.</param>
        void PostDiscussionThreads(IEnumerable<ICodeAnalysisIssue> issues, string commentSource);
    }
}
