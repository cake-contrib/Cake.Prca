namespace Cake.Prca
{
    using System.Collections.Generic;
    using Core;
    using Core.Annotations;
    using Core.IO;
    using Issues;
    using PullRequests;

    /// <summary>
    /// Contains functionality related to writing code analysis issues to pull requests.
    /// </summary>
    [CakeAliasCategory(CakeAliasConstants.MainCakeAliasCategory)]
    [CakeNamespaceImport("Cake.Prca.Issues")]
    [CakeNamespaceImport("Cake.Prca.PullRequests")]
    public static class PrcaAliases
    {
        /// <summary>
        /// Reports code analysis issues to pull requests.
        /// </summary>
        /// <param name="context">The context.</param>
        /// <param name="codeAnalysisProvider">The provider for code analysis issues.</param>
        /// <param name="pullRequestSystem">The pull request system.</param>
        /// <param name="repositoryRoot">Root path of the repository.</param>
        /// <returns>Information about the reported and written issues.</returns>
        /// <example>
        /// <para>Report code analysis issues reported as MsBuild warnings to a TFS pull request:</para>
        /// <code>
        /// <![CDATA[
        ///     ReportCodeAnalysisIssuesToPullRequest(
        ///         MsBuildCodeAnalysisFromFilePath(
        ///             @"C:\build\msbuild.log",
        ///             MsBuildXmlFileLoggerFormat),
        ///         TfsPullRequests(
        ///             new Uri("http://myserver:8080/tfs/defaultcollection/myproject/_git/myrepository"),
        ///             "refs/heads/feature/myfeature",
        ///             TfsAuthenticationNtlm()),
        ///         new DirectoryPath("c:\repo"));
        /// ]]>
        /// </code>
        /// </example>
        [CakeMethodAlias]
        public static PrcaResult ReportCodeAnalysisIssuesToPullRequest(
            this ICakeContext context,
            ICodeAnalysisProvider codeAnalysisProvider,
            IPullRequestSystem pullRequestSystem,
            DirectoryPath repositoryRoot)
        {
            context.NotNull(nameof(context));
            codeAnalysisProvider.NotNull(nameof(codeAnalysisProvider));
            pullRequestSystem.NotNull(nameof(pullRequestSystem));
            repositoryRoot.NotNull(nameof(repositoryRoot));

            return
                context.ReportCodeAnalysisIssuesToPullRequest(
                    codeAnalysisProvider,
                    pullRequestSystem,
                    new ReportCodeAnalysisIssuesToPullRequestSettings(repositoryRoot));
        }

        /// <summary>
        /// Reports code analysis issues to pull requests.
        /// </summary>
        /// <param name="context">The context.</param>
        /// <param name="codeAnalysisProviders">The list of provider for code analysis issues.</param>
        /// <param name="pullRequestSystem">The pull request system.</param>
        /// <param name="repositoryRoot">Root path of the repository.</param>
        /// <returns>Information about the reported and written issues.</returns>
        /// <example>
        /// <para>Report code analysis issues reported as MsBuild warnings to a TFS pull request:</para>
        /// <code>
        /// <![CDATA[
        ///     ReportCodeAnalysisIssuesToPullRequest(
        ///         new List<ICodeAnalysisProvider>
        ///         {
        ///             MsBuildCodeAnalysisFromFilePath(
        ///                 @"C:\build\msbuild.log",
        ///                 MsBuildXmlFileLoggerFormat),
        ///             InspectCodeFromFilePath(
        ///                 @"C:\build\inspectcode.log",
        ///                 MsBuildXmlFileLoggerFormat)
        ///         },
        ///         TfsPullRequests(
        ///             new Uri("http://myserver:8080/tfs/defaultcollection/myproject/_git/myrepository"),
        ///             "refs/heads/feature/myfeature",
        ///             TfsAuthenticationNtlm()),
        ///         new DirectoryPath("c:\repo"));
        /// ]]>
        /// </code>
        /// </example>
        [CakeMethodAlias]
        public static PrcaResult ReportCodeAnalysisIssuesToPullRequest(
            this ICakeContext context,
            IEnumerable<ICodeAnalysisProvider> codeAnalysisProviders,
            IPullRequestSystem pullRequestSystem,
            DirectoryPath repositoryRoot)
        {
            context.NotNull(nameof(context));
            pullRequestSystem.NotNull(nameof(pullRequestSystem));
            repositoryRoot.NotNull(nameof(repositoryRoot));

            // ReSharper disable once PossibleMultipleEnumeration
            codeAnalysisProviders.NotNullOrEmptyOrEmptyElement(nameof(codeAnalysisProviders));

            // ReSharper disable once PossibleMultipleEnumeration
            return
                context.ReportCodeAnalysisIssuesToPullRequest(
                    codeAnalysisProviders,
                    pullRequestSystem,
                    new ReportCodeAnalysisIssuesToPullRequestSettings(repositoryRoot));
        }

        /// <summary>
        /// Reports code analysis issues to pull requests using the specified settings.
        /// </summary>
        /// <param name="context">The context.</param>
        /// <param name="codeAnalysisProvider">The provider for code analysis issues.</param>
        /// <param name="pullRequestSystem">The pull request system.</param>
        /// <param name="settings">The settings.</param>
        /// <returns>Information about the reported and written issues.</returns>
        /// <example>
        /// <para>Report code analysis issues reported as MsBuild warnings to a TFS pull request and limit number of comments to ten:</para>
        /// <code>
        /// <![CDATA[
        ///     var settings =
        ///         new ReportCodeAnalysisIssuesToPullRequestSettings(new DirectoryPath("c:\repo"))
        ///         {
        ///             MaxIssuesToPost = 10
        ///         };
        ///
        ///     ReportCodeAnalysisIssuesToPullRequest(
        ///         MsBuildCodeAnalysisFromFilePath(
        ///             @"C:\build\msbuild.log",
        ///             MsBuildXmlFileLoggerFormat),
        ///         TfsPullRequests(
        ///             new Uri("http://myserver:8080/tfs/defaultcollection/myproject/_git/myrepository"),
        ///             "refs/heads/feature/myfeature",
        ///             TfsAuthenticationNtlm()),
        ///         settings));
        /// ]]>
        /// </code>
        /// </example>
        [CakeMethodAlias]
        public static PrcaResult ReportCodeAnalysisIssuesToPullRequest(
            this ICakeContext context,
            ICodeAnalysisProvider codeAnalysisProvider,
            IPullRequestSystem pullRequestSystem,
            ReportCodeAnalysisIssuesToPullRequestSettings settings)
        {
            context.NotNull(nameof(context));
            codeAnalysisProvider.NotNull(nameof(codeAnalysisProvider));
            pullRequestSystem.NotNull(nameof(pullRequestSystem));
            settings.NotNull(nameof(settings));

            return
                context.ReportCodeAnalysisIssuesToPullRequest(
                    new List<ICodeAnalysisProvider> { codeAnalysisProvider },
                    pullRequestSystem,
                    settings);
        }

        /// <summary>
        /// Reports code analysis issues to pull requests using the specified settings.
        /// </summary>
        /// <param name="context">The context.</param>
        /// <param name="codeAnalysisProviders">The list of provider for code analysis issues.</param>
        /// <param name="pullRequestSystem">The pull request system.</param>
        /// <param name="settings">The settings.</param>
        /// <returns>Information about the reported and written issues.</returns>
        /// <example>
        /// <para>Report code analysis issues reported as MsBuild warnings to a TFS pull request and limit number of comments to ten:</para>
        /// <code>
        /// <![CDATA[
        ///     var settings =
        ///         new ReportCodeAnalysisIssuesToPullRequestSettings(new DirectoryPath("c:\repo"))
        ///         {
        ///             MaxIssuesToPost = 10
        ///         };
        ///
        ///     ReportCodeAnalysisIssuesToPullRequest(
        ///         new List<ICodeAnalysisProvider>
        ///         {
        ///             MsBuildCodeAnalysisFromFilePath(
        ///                 @"C:\build\msbuild.log",
        ///                 MsBuildXmlFileLoggerFormat),
        ///             InspectCodeFromFilePath(
        ///                 @"C:\build\inspectcode.log",
        ///                 MsBuildXmlFileLoggerFormat)
        ///         },
        ///         TfsPullRequests(
        ///             new Uri("http://myserver:8080/tfs/defaultcollection/myproject/_git/myrepository"),
        ///             "refs/heads/feature/myfeature",
        ///             TfsAuthenticationNtlm()),
        ///         settings));
        /// ]]>
        /// </code>
        /// </example>
        [CakeMethodAlias]
        public static PrcaResult ReportCodeAnalysisIssuesToPullRequest(
            this ICakeContext context,
            IEnumerable<ICodeAnalysisProvider> codeAnalysisProviders,
            IPullRequestSystem pullRequestSystem,
            ReportCodeAnalysisIssuesToPullRequestSettings settings)
        {
            context.NotNull(nameof(context));
            pullRequestSystem.NotNull(nameof(pullRequestSystem));
            settings.NotNull(nameof(settings));

            // ReSharper disable once PossibleMultipleEnumeration
            codeAnalysisProviders.NotNullOrEmptyOrEmptyElement(nameof(codeAnalysisProviders));

            // ReSharper disable once PossibleMultipleEnumeration
            var orchestrator =
                new Orchestrator(
                    context.Log,
                    codeAnalysisProviders,
                    pullRequestSystem,
                    settings);
            return orchestrator.Run();
        }
    }
}
