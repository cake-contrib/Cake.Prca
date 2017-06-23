namespace Cake.Prca
{
    using System.Collections.Generic;
    using Core;
    using Core.Annotations;
    using Core.IO;
    using Issues;

#pragma warning disable SA1601 // Partial elements must be documented
    public static partial class PrcaAliases
#pragma warning restore SA1601 // Partial elements must be documented
    {
        /// <summary>
        /// Reads issues from a single issue provider.
        /// </summary>
        /// <param name="context">The context.</param>
        /// <param name="issueProvider">The provider for issues.</param>
        /// <param name="repositoryRoot">Root path of the repository.</param>
        /// <returns>Issues reported by issue provider.</returns>
        /// <example>
        /// <para>Read issues reported as MsBuild warnings:</para>
        /// <code>
        /// <![CDATA[
        ///     ReportIssuesToPullRequest(
        ///         MsBuildCodeAnalysisFromFilePath(
        ///             @"C:\build\msbuild.log",
        ///             MsBuildXmlFileLoggerFormat),
        ///         new DirectoryPath("c:\repo")));
        /// ]]>
        /// </code>
        /// </example>
        [CakeMethodAlias]
        public static IEnumerable<ICodeAnalysisIssue> ReadIssues(
            this ICakeContext context,
            ICodeAnalysisProvider issueProvider,
            DirectoryPath repositoryRoot)
        {
            context.NotNull(nameof(context));
            issueProvider.NotNull(nameof(issueProvider));
            repositoryRoot.NotNull(nameof(repositoryRoot));

            return
                context.ReadIssues(
                    issueProvider,
                    new ReadIssuesSettings(repositoryRoot));
        }

        /// <summary>
        /// Reads issues from issue providers.
        /// </summary>
        /// <param name="context">The context.</param>
        /// <param name="issueProviders">The list of provider for issues.</param>
        /// <param name="repositoryRoot">Root path of the repository.</param>
        /// <returns>Issues reported by all issue providers.</returns>
        /// <example>
        /// <para>Read code analysis issues reported as MsBuild warnings and issues reported by JetBrains inspect code:</para>
        /// <code>
        /// <![CDATA[
        ///     var issues = ReadIssues(
        ///         new List<ICodeAnalysisProvider>
        ///         {
        ///             MsBuildCodeAnalysisFromFilePath(
        ///                 @"C:\build\msbuild.log",
        ///                 MsBuildXmlFileLoggerFormat),
        ///             InspectCodeFromFilePath(
        ///                 @"C:\build\inspectcode.log",
        ///                 MsBuildXmlFileLoggerFormat)
        ///         },
        ///         new DirectoryPath("c:\repo")));
        /// ]]>
        /// </code>
        /// </example>
        [CakeMethodAlias]
        public static IEnumerable<ICodeAnalysisIssue> ReadIssues(
            this ICakeContext context,
            IEnumerable<ICodeAnalysisProvider> issueProviders,
            DirectoryPath repositoryRoot)
        {
            context.NotNull(nameof(context));
            repositoryRoot.NotNull(nameof(repositoryRoot));

            // ReSharper disable once PossibleMultipleEnumeration
            issueProviders.NotNullOrEmptyOrEmptyElement(nameof(issueProviders));

            // ReSharper disable once PossibleMultipleEnumeration
            return
                context.ReadIssues(
                    issueProviders,
                    new ReadIssuesSettings(repositoryRoot));
        }

        /// <summary>
        /// Reads issues from a single issue provider using the specified settings.
        /// </summary>
        /// <param name="context">The context.</param>
        /// <param name="issueProvider">The provider for issues.</param>
        /// <param name="settings">The settings.</param>
        /// <returns>Issues reported by issue provider.</returns>
        /// <example>
        /// <para>Read issues reported as MsBuild warnings and format comments in Markdown:</para>
        /// <code>
        /// <![CDATA[
        ///     var settings =
        ///         new ReadIssuesSettings(new DirectoryPath("c:\repo"))
        ///         {
        ///             Format = PrcaCommentFormat.Markdown
        ///         };
        ///
        ///     ReportIssuesToPullRequest(
        ///         MsBuildCodeAnalysisFromFilePath(
        ///             @"C:\build\msbuild.log",
        ///             MsBuildXmlFileLoggerFormat),
        ///         settings));
        /// ]]>
        /// </code>
        /// </example>
        [CakeMethodAlias]
        public static IEnumerable<ICodeAnalysisIssue> ReadIssues(
            this ICakeContext context,
            ICodeAnalysisProvider issueProvider,
            ReadIssuesSettings settings)
        {
            context.NotNull(nameof(context));
            issueProvider.NotNull(nameof(issueProvider));
            settings.NotNull(nameof(settings));

            return
                context.ReadIssues(
                    new List<ICodeAnalysisProvider> { issueProvider },
                    settings);
        }

        /// <summary>
        /// Reads issues from issue providers using the specified settings.
        /// </summary>
        /// <param name="context">The context.</param>
        /// <param name="issueProviders">The list of provider for issues.</param>
        /// <param name="settings">The settings.</param>
        /// <returns>Issues reported by all issue providers.</returns>
        /// <example>
        /// <para>Read code analysis issues reported as MsBuild warnings and issues reported by JetBrains inspect code
        /// with comments formatted as Markdown:</para>
        /// <code>
        /// <![CDATA[
        ///     var settings =
        ///         new ReadIssuesSettings(new DirectoryPath("c:\repo"))
        ///         {
        ///             Format = PrcaCommentFormat.Markdown
        ///         };
        ///
        ///     var issues = ReadIssues(
        ///         new List<ICodeAnalysisProvider>
        ///         {
        ///             MsBuildCodeAnalysisFromFilePath(
        ///                 @"C:\build\msbuild.log",
        ///                 MsBuildXmlFileLoggerFormat),
        ///             InspectCodeFromFilePath(
        ///                 @"C:\build\inspectcode.log",
        ///                 MsBuildXmlFileLoggerFormat)
        ///         },
        ///         settings));
        /// ]]>
        /// </code>
        /// </example>
        [CakeMethodAlias]
        public static IEnumerable<ICodeAnalysisIssue> ReadIssues(
            this ICakeContext context,
            IEnumerable<ICodeAnalysisProvider> issueProviders,
            ReadIssuesSettings settings)
        {
            context.NotNull(nameof(context));
            settings.NotNull(nameof(settings));

            // ReSharper disable once PossibleMultipleEnumeration
            issueProviders.NotNullOrEmptyOrEmptyElement(nameof(issueProviders));

            // ReSharper disable once PossibleMultipleEnumeration
            var issueReader =
                new IssueReader(context.Log, issueProviders, settings);

            return issueReader.ReadIssues(settings.Format);
        }
    }
}
