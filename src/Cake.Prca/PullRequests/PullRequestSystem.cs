namespace Cake.Prca.PullRequests
{
    using System;
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

        /// <summary>
        /// Gets general settings.
        /// Is set after <see cref="Initialize"/> was called from the core addin.
        /// </summary>
        protected ReportCodeAnalysisIssuesToPullRequestSettings PrcaSettings { get; private set; }

        /// <inheritdoc/>
        public virtual bool Initialize(ReportCodeAnalysisIssuesToPullRequestSettings settings)
        {
            settings.NotNull(nameof(settings));

            this.PrcaSettings = settings;

            return true;
        }

        /// <inheritdoc/>
        public virtual PrcaCommentFormat GetPreferredCommentFormat()
        {
            return PrcaCommentFormat.PlainText;
        }

        /// <inheritdoc/>
        public IEnumerable<IPrcaDiscussionThread> FetchActiveDiscussionThreads(string commentSource)
        {
            if (this.PrcaSettings == null)
            {
                throw new InvalidOperationException("Initialize needs to be called first.");
            }

            return this.InternalFetchActiveDiscussionThreads(commentSource);
        }

        /// <inheritdoc/>
        public IEnumerable<FilePath> GetModifiedFilesInPullRequest()
        {
            if (this.PrcaSettings == null)
            {
                throw new InvalidOperationException("Initialize needs to be called first.");
            }

            return this.InternalGetModifiedFilesInPullRequest();
        }

        /// <inheritdoc/>
        public void MarkThreadsAsFixed(IEnumerable<IPrcaDiscussionThread> threads)
        {
            if (this.PrcaSettings == null)
            {
                throw new InvalidOperationException("Initialize needs to be called first.");
            }

            this.InternalMarkThreadsAsFixed(threads);
        }

        /// <inheritdoc/>
        public void PostDiscussionThreads(IEnumerable<ICodeAnalysisIssue> issues, string commentSource)
        {
            if (this.PrcaSettings == null)
            {
                throw new InvalidOperationException("Initialize needs to be called first.");
            }

            this.InternalPostDiscussionThreads(issues, commentSource);
        }

        /// <summary>
        /// Returns a list of all active discussion threads.
        /// Compared to <see cref="FetchActiveDiscussionThreads"/> it is safe to access <see cref="PrcaSettings"/> from this method.
        /// </summary>
        /// <param name="commentSource">Value used to indicate threads created by this addin.</param>
        /// <returns>List of all active discussion threads.</returns>
        protected abstract IEnumerable<IPrcaDiscussionThread> InternalFetchActiveDiscussionThreads(string commentSource);

        /// <summary>
        /// Returns a list of all files modified in a pull request.
        /// Compared to <see cref="GetModifiedFilesInPullRequest"/> it is safe to access <see cref="PrcaSettings"/> from this method.
        /// </summary>
        /// <returns>List of all files modified in a pull request.</returns>
        protected abstract IEnumerable<FilePath> InternalGetModifiedFilesInPullRequest();

        /// <summary>
        /// Marks a list of discussion threads as resolved.
        /// Compared to <see cref="MarkThreadsAsFixed"/> it is safe to access <see cref="PrcaSettings"/> from this method.
        /// </summary>
        /// <param name="threads">Threads to mark as fixed.</param>
        protected abstract void InternalMarkThreadsAsFixed(IEnumerable<IPrcaDiscussionThread> threads);

        /// <summary>
        /// Posts discussion threads for all issues which need to be posted.
        /// Compared to <see cref="PostDiscussionThreads"/> it is safe to access <see cref="PrcaSettings"/> from this method.
        /// </summary>
        /// <param name="issues">Issues which need to be posted.</param>
        /// <param name="commentSource">Value used to decorate comments created by this addin.</param>
        protected abstract void InternalPostDiscussionThreads(IEnumerable<ICodeAnalysisIssue> issues, string commentSource);
    }
}
