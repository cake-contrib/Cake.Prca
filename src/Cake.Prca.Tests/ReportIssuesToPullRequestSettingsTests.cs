namespace Cake.Prca.Tests
{
    using Core.IO;
    using Shouldly;
    using Xunit;

    public sealed class ReportIssuesToPullRequestSettingsTests
    {
        public sealed class TheCtor
        {
            [Fact]
            public void Should_Throw_If_RepositoryRoot_Is_Null()
            {
                // Given

                // When
                var result = Record.Exception(() => new ReportIssuesToPullRequestSettings(null));

                // Then
                result.IsArgumentNullException("repositoryRoot");
            }

            [Fact]
            public void Should_Set_RepositoryRoot()
            {
                // Given
                var repositoryRoot = new DirectoryPath(@"c:\repo");

                // When
                var settings = new ReportIssuesToPullRequestSettings(repositoryRoot);

                // Then
                settings.RepositoryRoot.ShouldBe(repositoryRoot);
            }
        }
    }
}
