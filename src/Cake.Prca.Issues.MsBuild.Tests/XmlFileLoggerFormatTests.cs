namespace Cake.Prca.Issues.MsBuild.Tests
{
    using System.Linq;
    using Core.IO;
    using Prca.Tests;
    using Shouldly;
    using Xunit;

    public class XmlFileLoggerFormatTests
    {
        public sealed class TheXmlFileLoggerFormat
        {
            [Fact]
            public void Should_Throw_If_Log_Is_Null()
            {
                // Given / When
                var result = Record.Exception(() => new XmlFileLoggerFormat(null));

                // Then
                result.IsArgumentNullException("log");
            }

            [Fact]
            public void Should_Read_Issue_With_File_Correct()
            {
                // Given
                var fixture = new MsBuildCodeAnalysisProviderFixture("IssueWithFile.xml");

                // When
                var issues = fixture.ReadIssues();

                // Then
                issues.Count().ShouldBe(1);
                issues.ShouldContain(x =>
                    x.AffectedFileRelativePath.ToString() == new FilePath(@"\src\Cake.Prca.CodeAnalysisProvider.MsBuild.Tests\MsBuildCodeAnalysisProviderTests.cs").ToString() &&
                    x.Line == 1311 &&
                    x.Rule == "CA2201" &&
                    x.Priority == 0 &&
                    x.Message == @"Microsoft.Usage : 'ConfigurationManager.GetSortedConfigFiles(String)' creates an exception of type 'ApplicationException', an exception type that is not sufficiently specific and should never be raised by user code. If this exception instance might be thrown, use a different exception type.");
            }

            [Fact]
            public void Should_Read_Issue_With_File_Without_Path_Correct()
            {
                // Given
                var fixture = new MsBuildCodeAnalysisProviderFixture("IssueWithOnlyFileName.xml");

                // When
                var issues = fixture.ReadIssues();

                // Then
                issues.Count().ShouldBe(1);
                issues.ShouldContain(x =>
                    x.AffectedFileRelativePath.ToString() == new FilePath(@"\src\Cake.Prca.CodeAnalysisProvider.MsBuild.Tests\MsBuildCodeAnalysisProviderTests.cs").ToString() &&
                    x.Line == 13 &&
                    x.Rule == "CS0219" &&
                    x.Priority == 0 &&
                    x.Message == @"The variable 'foo' is assigned but its value is never used");
            }

            [Fact]
            public void Should_Read_Issue_Without_File_Correct()
            {
                // Given
                var fixture = new MsBuildCodeAnalysisProviderFixture("IssueWithoutFile.xml");

                // When
                var issues = fixture.ReadIssues();

                // Then
                // TODO Is this correct? Or should we return them here and have the core logic or pull request system implementation taking care how to handle them?
                issues.Count().ShouldBe(0);
                //issues.Count().ShouldBe(1);
                //issues.ShouldContain(x =>
                //    x.AffectedFileRelativePath == string.Empty &&
                //    x.Line == -1 &&
                //    x.Rule == "CA1711" &&
                //    x.Priority == 0 &&
                //    x.Message == @"Microsoft.Naming : Rename type name 'UniqueQueue(Of T)' so that it does not end in 'Queue'.");
            }
        }
    }
}
