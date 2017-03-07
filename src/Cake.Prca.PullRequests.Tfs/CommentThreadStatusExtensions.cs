namespace Cake.Prca.PullRequests.Tfs
{
    using System;
    using Microsoft.TeamFoundation.SourceControl.WebApi;

    /// <summary>
    /// Extensions for <see cref="CommentThreadStatus"/> enumeration.
    /// </summary>
    internal static class CommentThreadStatusExtensions
    {
        /// <summary>
        /// Converts a <see cref="CommentThreadStatus"/> from TFS to a <see cref="PrcaDiscussionStatus"/> as used in this addin.
        /// </summary>
        /// <param name="status">TFS status to convert.</param>
        /// <returns>Converted status.</returns>
        public static PrcaDiscussionStatus ToPrcaDiscussionStatus(this CommentThreadStatus status)
        {
            switch (status)
            {
                case CommentThreadStatus.Unknown:
                    return PrcaDiscussionStatus.Unknown;
                case CommentThreadStatus.Active:
                case CommentThreadStatus.Pending:
                    return PrcaDiscussionStatus.Active;
                case CommentThreadStatus.Fixed:
                case CommentThreadStatus.WontFix:
                case CommentThreadStatus.Closed:
                case CommentThreadStatus.ByDesign:
                    return PrcaDiscussionStatus.Resolved;
                default:
                    throw new PrcaException("Unknown enumeration value");
            }
        }
    }
}
