namespace Cake.Prca.Tests
{
    using Xunit;

    public sealed class PrcaArgumentChecksTests
    {
        public sealed class TheNotNullExtension
        {
            [Fact]
            public void Should_Throw_If_Value_Is_Null()
            {
                // Given
                string value = null;

                // When
                var result = Record.Exception(() => value.NotNull("foo"));

                // Then
                result.IsArgumentNullException("foo");
            }

            [Theory]
            [InlineData("foo")]
            public void Should_Not_Throw_If_Value_Is_Set(object value)
            {
                value.NotNull("foo");
            }
        }

        public sealed class TheNotNullOrWhiteSpaceExtension
        {
            [Fact]
            public void Should_Throw_If_Value_Is_Null()
            {
                // Given
                string value = null;

                // When
                var result = Record.Exception(() => value.NotNullOrWhiteSpace("foo"));

                // Then
                result.IsArgumentNullException("foo");
            }

            [Fact]
            public void Should_Throw_If_Value_Is_Empty()
            {
                // Given
                string value = string.Empty;

                // When
                var result = Record.Exception(() => value.NotNullOrWhiteSpace("foo"));

                // Then
                result.IsArgumentOutOfRangeException("foo");
            }

            [Fact]
            public void Should_Throw_If_Value_Is_WhiteSpace()
            {
                // Given
                string value = " ";

                // When
                var result = Record.Exception(() => value.NotNullOrWhiteSpace("foo"));

                // Then
                result.IsArgumentOutOfRangeException("foo");
            }

            [Theory]
            [InlineData("foo")]
            public void Should_Not_Throw_If_Value_Is_Valid(string value)
            {
                value.NotNullOrWhiteSpace("foo");
            }
        }

        public sealed class TheNotNegativeExtension
        {
            [Theory]
            [InlineData(-1)]
            [InlineData(int.MinValue)]
            public void Should_Throw_If_Value_Is_Negative(int value)
            {
                // When
                var result = Record.Exception(() => value.NotNegative("foo"));

                // Then
                result.IsArgumentOutOfRangeException("foo");
            }

            [Theory]
            [InlineData(0)]
            [InlineData(1)]
            [InlineData(int.MaxValue)]
            public void Should_Not_Throw_If_Value_Is_Valid(int value)
            {
                value.NotNegative("foo");
            }
        }

        public sealed class TheNotNegativeOrZeroExtension
        {
            [Theory]
            [InlineData(-1)]
            [InlineData(int.MinValue)]
            public void Should_Throw_If_Value_Is_Negative(int value)
            {
                // When
                var result = Record.Exception(() => value.NotNegativeOrZero("foo"));

                // Then
                result.IsArgumentOutOfRangeException("foo");
            }

            [Fact]
            public void Should_Throw_If_Value_Is_Zero()
            {
                // Given
                var value = 0;

                // When
                var result = Record.Exception(() => value.NotNegativeOrZero("foo"));

                // Then
                result.IsArgumentOutOfRangeException("foo");
            }

            [Theory]
            [InlineData(1)]
            [InlineData(int.MaxValue)]
            public void Should_Not_Throw_If_Value_Is_Valid(int value)
            {
                value.NotNegative("foo");
            }
        }
    }
}
