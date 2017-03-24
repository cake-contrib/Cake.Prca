namespace Cake.Prca.Tests.PullRequests
{
    using System.Collections.Generic;
    using Prca.PullRequests;
    using Shouldly;
    using Xunit;

    public class PrcaDiscussionThreadTests
    {
        public sealed class ThePrcaDiscussionThreadCtor
        {
            [Theory]
            [InlineData(@"c:\src\foo.cs")]
            [InlineData(@"/foo")]
            [InlineData(@"\foo")]
            public void Should_Throw_If_File_Path_Is_Absolute(string filePath)
            {
                // Given / When
                var result = Record.Exception(() => new PrcaDiscussionThread(1, PrcaDiscussionStatus.Active, filePath, new List<IPrcaDiscussionComment>()));

                // Then
                result.IsArgumentOutOfRangeException("filePath");
            }

            [Fact]
            public void Should_Throw_If_Comments_Is_Null()
            {
                // Given / When
                var result = Record.Exception(() => new PrcaDiscussionThread(1, PrcaDiscussionStatus.Active, @"foo.cs", null));

                // Then
                result.IsArgumentNullException("comments");
            }

            [Fact]
            public void Should_Handle_File_Paths_Which_Are_Null()
            {
                // Given / When
                var thread =
                    new PrcaDiscussionThread(
                        1,
                        PrcaDiscussionStatus.Active,
                        null,
                        new List<IPrcaDiscussionComment>());

                // Then
                thread.AffectedFileRelativePath.ShouldBe(null);
            }

            [Theory]
            [InlineData(int.MinValue)]
            [InlineData(0)]
            [InlineData(int.MaxValue)]
            public void Should_Set_Id(int id)
            {
                // Given / When
                var thread =
                    new PrcaDiscussionThread(
                        id,
                        PrcaDiscussionStatus.Active,
                        "foo.cs",
                        new List<IPrcaDiscussionComment>());

                // Then
                thread.Id.ShouldBe(id);
            }

            [Theory]
            [InlineData(PrcaDiscussionStatus.Active)]
            [InlineData(PrcaDiscussionStatus.Resolved)]
            public void Should_Set_Status(PrcaDiscussionStatus status)
            {
                // Given / When
                var thread =
                    new PrcaDiscussionThread(
                        1,
                        status,
                        "foo.cs",
                        new List<IPrcaDiscussionComment>());

                // Then
                thread.Status.ShouldBe(status);
            }

            [Fact]
            public void Should_Set_Comments()
            {
                // Given
                var comments =
                    new List<IPrcaDiscussionComment>
                    {
                    new PrcaDiscussionComment
                    {
                        Content = "Foo",
                        IsDeleted = false
                    },
                    new PrcaDiscussionComment
                    {
                        Content = "Bar",
                        IsDeleted = true
                    }
                    };

                // When
                var thread =
                    new PrcaDiscussionThread(
                        1,
                        PrcaDiscussionStatus.Active,
                        "foo.cs",
                        comments);

                // Then
                thread.Comments.ShouldContain(comment => comments.Contains(comment), comments.Count);
            }

            [Theory]
            [InlineData(@"foo", @"foo")]
            [InlineData(@"foo\bar", @"foo/bar")]
            [InlineData(@"foo/bar", @"foo/bar")]
            [InlineData(@"foo\bar\", @"foo/bar")]
            [InlineData(@"foo/bar/", @"foo/bar")]
            [InlineData(@".\foo", @"foo")]
            [InlineData(@"./foo", @"foo")]
            [InlineData(@"foo\..\bar", @"foo/../bar")]
            [InlineData(@"foo/../bar", @"foo/../bar")]
            public void Should_Set_File_Path(string filePath, string expectedFilePath)
            {
                // Given / When
                // Given / When
                var thread =
                    new PrcaDiscussionThread(
                        1,
                        PrcaDiscussionStatus.Active,
                        filePath,
                        new List<IPrcaDiscussionComment>());

                // Then
                thread.AffectedFileRelativePath.ToString().ShouldBe(expectedFilePath);
                thread.AffectedFileRelativePath.IsRelative.ShouldBe(true, "File path was not set as relative.");
            }
        }
    }
}
