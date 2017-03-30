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
