namespace Cake.Prca.Issues.MsBuild.Tests
{
    using System.Collections.Generic;
    using System.IO;
    using Core.Diagnostics;
    using Testing;

    public class MsBuildCodeAnalysisProviderFixture
    {

        public MsBuildCodeAnalysisProviderFixture(string fileResourceName)
        {
            this.Log = new FakeLog();
            Log.Verbosity = Verbosity.Normal;

            using (var stream = this.GetType().Assembly.GetManifestResourceStream("Cake.Prca.Issues.MsBuild.Tests.Testfiles." + fileResourceName))
            {
                using (var sr = new StreamReader(stream))
                {
                    this.Settings = 
                        new MsBuildCodeAnalysisSettings(
                            sr.ReadToEnd(),
                            new XmlFileLoggerFormat(this.Log),
                            new Core.IO.DirectoryPath(@"c:\Source\Cake.Prca"));
                }
            }
        }

        public FakeLog Log { get; set; }

        public MsBuildCodeAnalysisSettings Settings { get; set; }

        public MsBuildCodeAnalysisProvider Create()
        {
            return new MsBuildCodeAnalysisProvider(this.Log, this.Settings);
        }

        public IEnumerable<ICodeAnalysisIssue> ReadIssues()
        {
            var codeAnalysisProvider = this.Create();
            return codeAnalysisProvider.ReadIssues();
        }
    }
}
