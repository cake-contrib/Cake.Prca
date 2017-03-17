namespace Cake.Prca.Tests.PullRequests
{
    using Shouldly;
    using Testing;
    using Xunit;

    public sealed class PullRequestSystemTests
    {
        public sealed class ThePullRequestSystemCtor
        {
            [Fact]
            public void Should_Throw_If_File_Log_Is_Null()
            {
                // Given / When
                var result = Record.Exception(() => new FakePullRequestSystem(null));

                // Then
                result.IsArgumentNullException("log");
            }

            [Fact]
            public void Should_Set_Log()
            {
                // Given
                var log = new FakeLog();

                // When
                var prSystem = new FakePullRequestSystem(log);

                // Then
                prSystem.Log.ShouldBe(log);
            }
        }
    }
}
