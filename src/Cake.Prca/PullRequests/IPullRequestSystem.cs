﻿namespace Cake.Prca.PullRequests
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
        /// Initializes the code analysis provider.
        /// </summary>
        /// <param name="settings">General settings</param>
        /// <returns><c>true</c> if the initialization was successful, <c>false</c> otherwise.</returns>
        bool Initialize(ReportIssuesToPullRequestSettings settings);

        /// <summary>
        /// Returns the preferred format for pull request comments.
        /// </summary>
        /// <returns>The preferred format for pull request comments</returns>
        PrcaCommentFormat GetPreferredCommentFormat();

        /// <summary>
        /// Returns a list of all active discussion threads.
        /// </summary>
        /// <param name="commentSource">Value used to indicate threads created by this addin.</param>
        /// <returns>List of all active discussion threads.</returns>
        IEnumerable<IPrcaDiscussionThread> FetchActiveDiscussionThreads(string commentSource);

        /// <summary>
        /// Marks a list of discussion threads as resolved.
        /// </summary>
        /// <param name="threads">Threads to mark as fixed.</param>
        void MarkThreadsAsFixed(IEnumerable<IPrcaDiscussionThread> threads);

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
