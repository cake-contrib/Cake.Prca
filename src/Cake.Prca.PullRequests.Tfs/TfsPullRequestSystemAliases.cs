namespace Cake.Prca.PullRequests.Tfs
{
    using System;
    using Core;
    using Core.Annotations;

    /// <summary>
    /// Contains functionality related to writing code analysis issues to Team Foundation Server or
    /// Visual Studio Team Services pull requests.
    /// </summary>
    [CakeAliasCategory(CakeAliasConstants.MainCakeAliasCategory)]
    [CakeNamespaceImport("Cake.Prca.PullRequestSystem.Tfs")]
    public static class TfsPullRequestSystemAliases
    {
        /// <summary>
        /// Gets an object for writing issues to Team Foundation Server or Visual Studio Team Services pull request
        /// in a specific repository and for a specific source branch.
        /// </summary>
        /// <param name="context">The context.</param>
        /// <param name="repositoryUrl">Full URL of the Git repository,
        /// eg. <code>http://myserver:8080/tfs/defaultcollection/myproject/_git/myrepository</code>.</param>
        /// <param name="sourceBranch">Branch for which the pull request is made.</param>
        /// <returns>Object for writing issues to Team Foundation Server or Visual Studio Team Services pull request.</returns>
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
        [CakeAliasCategory(CakeAliasConstants.PullRequestSystemCakeAliasCategory)]
        public static IPullRequestSystem TfsPullRequests(
            this ICakeContext context,
            Uri repositoryUrl,
            string sourceBranch)
        {
            context.NotNull(nameof(context));
            repositoryUrl.NotNull(nameof(repositoryUrl));
            sourceBranch.NotNullOrWhiteSpace(nameof(sourceBranch));

            return context.TfsPullRequests(new TfsPullRequestSettings(repositoryUrl, sourceBranch));
        }

        /// <summary>
        /// Gets an object for writing issues to Team Foundation Server or Visual Studio Team Services pull request
        /// in a specific repository and with a specific ID.
        /// </summary>
        /// <param name="context">The context.</param>
        /// <param name="repositoryUrl">Full URL of the Git repository,
        /// eg. <code>http://myserver:8080/tfs/defaultcollection/myproject/_git/myrepository</code>.</param>
        /// <param name="pullRequestId">ID of the pull request.</param>
        /// <returns>Object for writing issues to Team Foundation Server or Visual Studio Team Services pull request.</returns>
        /// <example>
        /// <para>Report code analysis issues reported as MsBuild warnings to a TFS pull request:</para>
        /// <code>
        /// <![CDATA[
        ///     ReportCodeAnalysisIssuesToPullRequest(
        ///         MsBuildCodeAnalysis(@"C:\build\msbuild.log", MsBuildXmlFileLoggerFormat),
        ///         TfsPullRequests(new Uri("http://myserver:8080/tfs/defaultcollection/myproject/_git/myrepository"), 5));
        /// ]]>
        /// </code>
        /// </example>
        [CakeMethodAlias]
        [CakeAliasCategory(CakeAliasConstants.PullRequestSystemCakeAliasCategory)]
        public static IPullRequestSystem TfsPullRequests(
            this ICakeContext context,
            Uri repositoryUrl,
            int pullRequestId)
        {
            context.NotNull(nameof(context));
            repositoryUrl.NotNull(nameof(repositoryUrl));

            return context.TfsPullRequests(new TfsPullRequestSettings(repositoryUrl, pullRequestId));
        }

        /// <summary>
        /// Gets an object for writing issues to Team Foundation Server or Visual Studio Team Services pull request
        /// using the specified settings.
        /// </summary>
        /// <param name="context">The context.</param>
        /// <param name="settings">Settings for accessing the pull request system.</param>
        /// <returns>Object for writing issues to Team Foundation Server or Visual Studio Team Services pull request.</returns>
        /// <example>
        /// <para>Report code analysis issues reported as MsBuild warnings to a TFS pull request:</para>
        /// <code>
        /// <![CDATA[
        ///     var pullRequestSettings =
        ///         new TfsPullRequestSettings(
        ///             new Uri("http://myserver:8080/tfs/defaultcollection/myproject/_git/myrepository"),
        ///             "refs/heads/feature/myfeature");
        ///
        ///     ReportCodeAnalysisIssuesToPullRequest(
        ///         MsBuildCodeAnalysis(@"C:\build\msbuild.log", MsBuildXmlFileLoggerFormat),
        ///         TfsPullRequests(pullRequestSettings));
        /// ]]>
        /// </code>
        /// </example>
        [CakeMethodAlias]
        [CakeAliasCategory(CakeAliasConstants.PullRequestSystemCakeAliasCategory)]
        public static IPullRequestSystem TfsPullRequests(
            this ICakeContext context,
            TfsPullRequestSettings settings)
        {
            context.NotNull(nameof(context));
            settings.NotNull(nameof(settings));

            return new TfsPullRequestSystem(context.Log, settings);
        }
    }
}
