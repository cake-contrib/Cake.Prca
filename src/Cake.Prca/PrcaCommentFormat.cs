namespace Cake.Prca
{
    /// <summary>
    /// Possible format options for comments in the pull request system
    /// </summary>
    public enum PrcaCommentFormat
    {
        /// <summary>
        /// Undefined format.
        /// </summary>
        Undefined,

        /// <summary>
        /// Plain text.
        /// </summary>
        PlainText,

        /// <summary>
        /// Hypertext markup language.
        /// </summary>
        Html,

        /// <summary>
        /// Markdown syntax.
        /// </summary>
        Markdown
    }
}
