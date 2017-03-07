namespace Cake.Prca.Issues.MsBuild.Tests
{
    using Prca.Tests;
    using Xunit;

    public class MsBuildCodeAnalysisProviderTests
    {
        public sealed class TheMsBuildCodeAnalysisProvider
        {
            [Fact]
            public void Should_Throw_If_Log_Is_Null()
            {
                // Given
                var fixture = new MsBuildCodeAnalysisProviderFixture("IssueWithFile.xml");
                fixture.Log = null;

                // When
                var result = Record.Exception(() => fixture.Create());

                // Then
                result.IsArgumentNullException("log");
            }

            [Fact]
            public void Should_Throw_If_Settings_Are_Null()
            {
                // Given
                var fixture = new MsBuildCodeAnalysisProviderFixture("IssueWithFile.xml");
                fixture.Settings = null;

                // When
                var result = Record.Exception(() => fixture.Create());

                // Then
                result.IsArgumentNullException("settings");
            }
        }
    }
}
