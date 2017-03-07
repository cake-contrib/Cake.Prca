namespace Cake.Prca.Issues.MsBuild
{
    using Core;
    using Core.Annotations;
    using Core.IO;

    /// <summary>
    /// Contains functionality related to importing code analysis issues from MSBuild logs to write them to
    /// pull requests.
    /// </summary>
    [CakeAliasCategory(CakeAliasConstants.MainCakeAliasCategory)]
    [CakeNamespaceImport("Cake.Prca.CodeAnalysisProvider.MsBuild")]
    public static class MsBuildCodeAnalysisProviderAliases
    {
        /// <summary>
        /// <para>
        /// Gets an instance for the MsBuild log format as written by the <code>XmlFileLogger</code> class
        /// from MSBuild Extension Pack.
        /// </para>
        /// <para>
        /// You can add the logger to the MSBuildSettings like this:
        /// <code>
        /// var settings = new MsBuildSettings()
        ///     .WithLogger(
        ///         Context.Tools.Resolve("MSBuild.ExtensionPack.Loggers.dll").FullPath,
        ///         "XmlFileLogger",
        ///         string.Format(
        ///             "logfile=\"{0}\";invalidCharReplacement=_;verbosity=Detailed;encoding=UTF-8",
        ///             @"C:\build\msbuild.log")
        ///     )
        /// </code>
        /// </para>
        /// <para>
        /// In order to use the above logger, include the following in your build.cake file to download and
        /// install from NuGet.org:
        /// <code>
        /// #tool "nuget:?package=MSBuild.Extension.Pack"
        /// </code>
        /// </para>
        /// </summary>
        /// <param name="context">The context.</param>
        /// <returns>Instance for the MsBuild log format.</returns>
        [CakePropertyAlias]
        [CakeAliasCategory(CakeAliasConstants.CodeAnalysisProviderCakeAliasCategory)]
        public static ILogFileFormat MsBuildXmlFileLoggerFormat(
            this ICakeContext context)
        {
            context.NotNull(nameof(context));

            return new XmlFileLoggerFormat(context.Log);
        }

        /// <summary>
        /// Gets an instance of a provider for code analysis issues reported as MsBuild warnings using a log file from disk.
        /// </summary>
        /// <param name="context">The context.</param>
        /// <param name="logFilePath">Path to the the MsBuild log file.
        /// The log file needs to be in the format as defined by the <paramref name="format"/> parameter.</param>
        /// <param name="format">Format of the provided MsBuild log file.</param>
        /// <param name="repositoryRoot">Root path of the repository.</param>
        /// <returns>Instance of a provider for code analysis issues reported as MsBuild warnings.</returns>
        /// <example>
        /// <para>Report code analysis issues reported as MsBuild warnings to a TFS pull request:</para>
        /// <code>
        /// <![CDATA[
        ///     ReportCodeAnalysisIssuesToPullRequest(
        ///         MsBuildCodeAnalysis(
        ///             new FilePath("C:\build\msbuild.log"),
        ///             MsBuildXmlFileLoggerFormat,
        ///             new DirectoryPath("c:\repo")),
        ///         TfsPullRequests(
        ///             new Uri("http://myserver:8080/tfs/defaultcollection/myproject/_git/myrepository"),
        ///             "refs/heads/feature/myfeature"));
        /// ]]>
        /// </code>
        /// </example>
        [CakeMethodAlias]
        [CakeAliasCategory(CakeAliasConstants.CodeAnalysisProviderCakeAliasCategory)]
        public static ICodeAnalysisProvider MsBuildCodeAnalysis(
            this ICakeContext context,
            FilePath logFilePath,
            ILogFileFormat format,
            DirectoryPath repositoryRoot)
        {
            context.NotNull(nameof(context));
            logFilePath.NotNull(nameof(logFilePath));
            format.NotNull(nameof(format));
            repositoryRoot.NotNull(nameof(repositoryRoot));

            return context.MsBuildCodeAnalysis(new MsBuildCodeAnalysisSettings(logFilePath, format, repositoryRoot));
        }

        /// <summary>
        /// Gets an instance of a provider for code analysis issues reported as MsBuild warnings using a log file from disk.
        /// </summary>
        /// <param name="context">The context.</param>
        /// <param name="logFileContent">Content of the the MsBuild log file.
        /// The log file needs to be in the format as defined by the <paramref name="format"/> parameter.</param>
        /// <param name="format">Format of the provided MsBuild log file.</param>
        /// <param name="repositoryRoot">Root path of the repository.</param>
        /// <returns>Instance of a provider for code analysis issues reported as MsBuild warnings.</returns>
        /// <example>
        /// <para>Report code analysis issues reported as MsBuild warnings to a TFS pull request:</para>
        /// <code>
        /// <![CDATA[
        ///     ReportCodeAnalysisIssuesToPullRequest(
        ///         MsBuildCodeAnalysis(
        ///             logFileContent,
        ///             MsBuildXmlFileLoggerFormat,
        ///             new DirectoryPath("c:\repo")),
        ///         TfsPullRequests(
        ///             new Uri("http://myserver:8080/tfs/defaultcollection/myproject/_git/myrepository"),
        ///             "refs/heads/feature/myfeature"));
        /// ]]>
        /// </code>
        /// </example>
        [CakeMethodAlias]
        [CakeAliasCategory(CakeAliasConstants.CodeAnalysisProviderCakeAliasCategory)]
        public static ICodeAnalysisProvider MsBuildCodeAnalysis(
            this ICakeContext context,
            string logFileContent,
            ILogFileFormat format,
            DirectoryPath repositoryRoot)
        {
            context.NotNull(nameof(context));
            logFileContent.NotNullOrWhiteSpace(nameof(logFileContent));
            format.NotNull(nameof(format));
            repositoryRoot.NotNull(nameof(repositoryRoot));

            return context.MsBuildCodeAnalysis(new MsBuildCodeAnalysisSettings(logFileContent, format, repositoryRoot));
        }

        /// <summary>
        /// Gets an instance of a provider for code analysis issues reported as MsBuild warnings using specified settings.
        /// </summary>
        /// <param name="context">The context.</param>
        /// <param name="settings">Settings for reading the MSBuild log.</param>
        /// <returns>Instance of a provider for code analysis issues reported as MsBuild warnings.</returns>
        /// <example>
        /// <para>Report code analysis issues reported as MsBuild warnings to a TFS pull request:</para>
        /// <code>
        /// <![CDATA[
        ///     var settings =
        ///         new MsBuildCodeAnalysisSettings(
        ///             new FilePath("C:\build\msbuild.log"),
        ///             MsBuildXmlFileLoggerFormat,
        ///             new DirectoryPath("c:\repo"));
        ///
        ///     ReportCodeAnalysisIssuesToPullRequest(
        ///         MsBuildCodeAnalysis(settings),
        ///         TfsPullRequests(
        ///             new Uri("http://myserver:8080/tfs/defaultcollection/myproject/_git/myrepository"),
        ///             "refs/heads/feature/myfeature"));
        /// ]]>
        /// </code>
        /// </example>
        [CakeMethodAlias]
        [CakeAliasCategory(CakeAliasConstants.CodeAnalysisProviderCakeAliasCategory)]
        public static ICodeAnalysisProvider MsBuildCodeAnalysis(
            this ICakeContext context,
            MsBuildCodeAnalysisSettings settings)
        {
            context.NotNull(nameof(context));
            settings.NotNull(nameof(settings));

            return new MsBuildCodeAnalysisProvider(context.Log, settings);
        }
    }
}
