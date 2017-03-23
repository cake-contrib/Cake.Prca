﻿namespace Cake.Prca.PullRequests
{
    using System.Collections.Generic;
    using Core.Diagnostics;
    using Core.IO;
    using Issues;

    /// <summary>
    /// Base class for all pull request system implementations.
    /// </summary>
    public abstract class PullRequestSystem : IPullRequestSystem
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="PullRequestSystem"/> class.
        /// </summary>
        /// <param name="log">The Cake log context.</param>
        protected PullRequestSystem(ICakeLog log)
        {
            log.NotNull(nameof(log));

            this.Log = log;
        }

        /// <summary>
        /// Gets the Cake log context.
        /// </summary>
        protected ICakeLog Log { get; }

        /// <inheritdoc/>
        public abstract void Initialize(ReportCodeAnalysisIssuesToPullRequestSettings settings);

        /// <inheritdoc/>
        public virtual PrcaCommentFormat GetPreferredCommentFormat()
        {
            return PrcaCommentFormat.PlainText;
        }

        /// <inheritdoc/>
        public abstract IEnumerable<IPrcaDiscussionThread> FetchActiveDiscussionThreads(string commentSource);

        /// <inheritdoc/>
        public abstract IEnumerable<FilePath> GetModifiedFilesInPullRequest();

        /// <inheritdoc/>
        public abstract void MarkThreadsAsFixed(IEnumerable<IPrcaDiscussionThread> threads);

        /// <inheritdoc/>
        public abstract void PostDiscussionThreads(IEnumerable<ICodeAnalysisIssue> issues, string commentSource);
    }
}
