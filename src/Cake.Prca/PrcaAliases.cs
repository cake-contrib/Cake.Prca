namespace Cake.Prca
{
    using Core;
    using Core.Annotations;
    using Issues;
    using PullRequests;

    /// <summary>
    /// Contains functionality related to writing code analysis issues to pull requests.
    /// </summary>
    [CakeAliasCategory(CakeAliasConstants.MainCakeAliasCategory)]
    public static class PrcaAliases
    {
        /// <summary>
        /// Reports code analysis issues to pull requests.
        /// </summary>
        /// <param name="context">The context.</param>
        /// <param name="codeAnalysisProvider">The provider for code analysis issues.</param>
        /// <param name="pullRequestSystem">The pull request system.</param>
        /// <example>
        /// <para>Report code analysis issues reported as MsBuild warnings to a TFS pull request:</para>
        /// <code>
        /// <![CDATA[
        ///     ReportCodeAnalysisIssuesToPullRequest(
        ///         MsBuildCodeAnalysis(@"C:\build\msbuild.log", MsBuildXmlFileLoggerFormat),
        ///         TfsPullRequests(new Uri("http://myserver:8080/tfs/defaultcollection/myproject/_git/myrepository"), "refs/heads/feature/myfeature"));
        /// ]]>
        /// </code>
        /// </example>
        [CakeMethodAlias]
        public static void ReportCodeAnalysisIssuesToPullRequest(
            this ICakeContext context,
            ICodeAnalysisProvider codeAnalysisProvider,
            IPullRequestSystem pullRequestSystem)
        {
            context.NotNull(nameof(context));
            codeAnalysisProvider.NotNull(nameof(codeAnalysisProvider));
            pullRequestSystem.NotNull(nameof(pullRequestSystem));

            context.ReportCodeAnalysisIssuesToPullRequest(
                codeAnalysisProvider,
                pullRequestSystem,
                new ReportCodeAnalysisIssuesToPullRequestSettings());
        }

        /// <summary>
        /// Reports code analysis issues to pull requests using the specified settings.
        /// </summary>
        /// <param name="context">The context.</param>
        /// <param name="codeAnalysisProvider">The provider for code analysis issues.</param>
        /// <param name="pullRequestSystem">The pull request system.</param>
        /// <param name="settings">The settings.</param>
        /// <example>
        /// <para>Report code analysis issues reported as MsBuild warnings to a TFS pull request and limit number of comments to ten:</para>
        /// <code>
        /// <![CDATA[
        ///     var settings =
        ///         new ReportCodeAnalysisIssuesToPullRequestSettings
        ///         {
        ///             MaxIssuesToPost = 10
        ///         };
        ///
        ///     ReportCodeAnalysisIssuesToPullRequest(
        ///         MsBuildCodeAnalysis(@"C:\build\msbuild.log", MsBuildXmlFileLoggerFormat),
        ///         TfsPullRequests(new Uri("http://myserver:8080/tfs/defaultcollection/myproject/_git/myrepository"), "refs/heads/feature/myfeature"));
        ///         settings);
        /// ]]>
        /// </code>
        /// </example>
        [CakeMethodAlias]
        public static void ReportCodeAnalysisIssuesToPullRequest(
            this ICakeContext context,
            ICodeAnalysisProvider codeAnalysisProvider,
            IPullRequestSystem pullRequestSystem,
            ReportCodeAnalysisIssuesToPullRequestSettings settings)
        {
            context.NotNull(nameof(context));
            codeAnalysisProvider.NotNull(nameof(codeAnalysisProvider));
            pullRequestSystem.NotNull(nameof(pullRequestSystem));
            settings.NotNull(nameof(settings));

            var orchestrator = new Orchestrator(context.Log, codeAnalysisProvider, pullRequestSystem, settings);
            orchestrator.Run();
        }
    }
}
