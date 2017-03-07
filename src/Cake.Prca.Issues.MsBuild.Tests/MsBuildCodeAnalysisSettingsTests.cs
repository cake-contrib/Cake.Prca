namespace Cake.Prca.Issues.MsBuild.Tests
{
    using System.IO;
    using Core.IO;
    using Prca.Tests;
    using Shouldly;
    using Testing;
    using Xunit;

    public class MsBuildCodeAnalysisSettingsTests
    {
        public sealed class TheMsBuildCodeAnalysisSettings
        {
            [Fact]
            public void Should_Throw_If_LogFilePath_Is_Null()
            {
                // Given / When
                var result = Record.Exception(() => 
                    new MsBuildCodeAnalysisSettings(
                        (FilePath)null,
                        new XmlFileLoggerFormat(new FakeLog()),
                        new DirectoryPath(@"C:\")));

                // Then
                result.IsArgumentNullException("logFilePath");
            }

            [Fact]
            public void Should_Throw_If_Format_For_LogFilePath_Is_Null()
            {
                // Given / When
                var result = Record.Exception(() =>
                    new MsBuildCodeAnalysisSettings(
                            new FilePath(@"C:\foo.log"),
                            null,
                            new DirectoryPath(@"C:\")));

                // Then
                result.IsArgumentNullException("format");
            }

            [Fact]
            public void Should_Throw_If_RepositoryRoot_For_LogFilePath_Is_Null()
            {
                // Given / When
                var result = Record.Exception(() =>
                    new MsBuildCodeAnalysisSettings(
                            new FilePath(@"C:\foo.log"),
                            new XmlFileLoggerFormat(new FakeLog()),
                            null));

                // Then
                result.IsArgumentNullException("repositoryRoot");
            }

            [Fact]
            public void Should_Throw_If_LogFileContent_Is_Null()
            {
                // Given / When
                var result = Record.Exception(() =>
                    new MsBuildCodeAnalysisSettings(
                        (string)null,
                        new XmlFileLoggerFormat(new FakeLog()),
                        new DirectoryPath(@"C:\")));

                // Then
                result.IsArgumentNullException("logFileContent");
            }

            [Fact]
            public void Should_Throw_If_LogFileContent_Is_Empty()
            {
                // Given / When
                var result = Record.Exception(() =>
                    new MsBuildCodeAnalysisSettings(
                        string.Empty,
                        new XmlFileLoggerFormat(new FakeLog()),
                        new DirectoryPath(@"C:\")));

                // Then
                result.IsArgumentOutOfRangeException("logFileContent");
            }

            [Fact]
            public void Should_Throw_If_LogFileContent_Is_WhiteSpace()
            {
                // Given / When
                var result = Record.Exception(() =>
                    new MsBuildCodeAnalysisSettings(
                        " ",
                        new XmlFileLoggerFormat(new FakeLog()),
                        new DirectoryPath(@"C:\")));

                // Then
                result.IsArgumentOutOfRangeException("logFileContent");
            }

            [Fact]
            public void Should_Throw_If_Format_For_LogFileContent_Is_Null()
            {
                // Given / When
                var result = Record.Exception(() => 
                    new MsBuildCodeAnalysisSettings(
                        "foo",
                        null,
                        new DirectoryPath(@"C:\")));

                // Then
                result.IsArgumentNullException("format");
            }

            [Fact]
            public void Should_Throw_If_RepositoryRoot_For_LogFileContent_Is_Null()
            {
                // Given / When
                var result = Record.Exception(() =>
                    new MsBuildCodeAnalysisSettings(
                        "foo",
                        new XmlFileLoggerFormat(new FakeLog()),
                        null));

                // Then
                result.IsArgumentNullException("repositoryRoot");
            }

            [Fact]
            public void Should_Set_Property_Values_Passed_To_Constructor()
            {
                // Given 
                var logFileContent = "foo";
                var format = new XmlFileLoggerFormat(new FakeLog());
                var repoRoot = new DirectoryPath(@"C:\");

                // When
                var settings = new MsBuildCodeAnalysisSettings(logFileContent, format, repoRoot);

                // Then
                settings.LogFileContent.ShouldBe(logFileContent);
                settings.Format.ShouldBe(format);
                settings.RepositoryRoot.ShouldBe(repoRoot);
            }

            [Fact]
            public void Should_Read_File_From_Disk()
            {
                var fileName = System.IO.Path.GetTempFileName();
                try
                {
                    // Given
                    using (var stream = this.GetType().Assembly.GetManifestResourceStream("Cake.Prca.Issues.MsBuild.Tests.Testfiles.IssueWithFile.xml"))
                    {
                        using (var file = new FileStream(fileName, FileMode.Create, FileAccess.Write))
                        {
                            stream.CopyTo(file);
                        }
                    }

                    var expected =
                        "<?xml version=\"1.0\" encoding=\"utf-8\"?>\r\n" +
                        "<build started=\"3/7/2017 8:31:20 AM\">\r\n" +
                        "  <warning code=\"CA2201\" file=\"c:\\Source\\Cake.Prca\\src\\Cake.Prca.CodeAnalysisProvider.MsBuild.Tests\\MsBuildCodeAnalysisProviderTests.cs\" line=\"1311\" column=\"0\"><![CDATA[Microsoft.Usage : 'ConfigurationManager.GetSortedConfigFiles(String)' creates an exception of type 'ApplicationException', an exception type that is not sufficiently specific and should never be raised by user code. If this exception instance might be thrown, use a different exception type.]]></warning>\r\n" +
                        "</build>";

                    // When
                    var settings = 
                        new MsBuildCodeAnalysisSettings(
                            new FilePath(fileName),
                            new XmlFileLoggerFormat(new FakeLog()),
                            new DirectoryPath(@"C:\"));

                    // Then
                    settings.LogFileContent.ShouldBe(expected);
                }
                finally
                {
                    if (File.Exists(fileName))
                    {
                        File.Delete(fileName);
                    }
                }
            }
        }
    }
}
