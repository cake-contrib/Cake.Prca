namespace Cake.Prca.Tests.Issues
{
    using Shouldly;
    using Testing;
    using Xunit;

    public sealed class CodeAnalysisProviderTests
    {
        public sealed class TheCodeAnalysisProviderCtor
        {
            [Fact]
            public void Should_Throw_If_File_Log_Is_Null()
            {
                // Given / When
                var result = Record.Exception(() => new FakeCodeAnalysisProvider(null));

                // Then
                result.IsArgumentNullException("log");
            }

            [Fact]
            public void Should_Set_Log()
            {
                // Given
                var log = new FakeLog();

                // When
                var provider = new FakeCodeAnalysisProvider(log);

                // Then
                provider.Log.ShouldBe(log);
            }
        }

        public sealed class TheInitializeMethod
        {
            [Fact]
            public void Should_Throw_If_Settings_Are_Null()
            {
                // Given
                var provider = new FakeCodeAnalysisProvider(new FakeLog());

                // When
                var result = Record.Exception(() => provider.Initialize(null));

                // Then
                result.IsArgumentNullException("settings");
            }

            [Fact]
            public void Should_Set_Settings()
            {
                // Given
                var provider = new FakeCodeAnalysisProvider(new FakeLog());
                var settings = new ReportIssuesToPullRequestSettings(@"c:\foo");

                // When
                provider.Initialize(settings);

                // Then
                provider.PrcaSettings.ShouldBe(settings);
            }

            [Fact]
            public void Should_Return_True()
            {
                // Given
                var provider = new FakeCodeAnalysisProvider(new FakeLog());
                var settings = new ReportIssuesToPullRequestSettings(@"c:\foo");

                // When
                var result = provider.Initialize(settings);

                // Then
                result.ShouldBe(true);
            }
        }

        public sealed class TheReadIssuesMethod
        {
            [Fact]
            public void Should_Throw_If_PrcaSettings_Is_Null()
            {
                // Given
                var provider = new FakeCodeAnalysisProvider(new FakeLog());

                // When
                var result = Record.Exception(() => provider.ReadIssues(PrcaCommentFormat.PlainText));

                // Then
                result.IsInvalidOperationException("Initialize needs to be called first.");
            }
        }
    }
}
