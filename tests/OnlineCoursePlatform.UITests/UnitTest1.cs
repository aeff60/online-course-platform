using Bunit;
using FluentAssertions;
using Microsoft.AspNetCore.Components;
using OnlineCoursePlatform.Client.Components;
using OnlineCoursePlatform.Shared.DTOs;
using Xunit;

namespace OnlineCoursePlatform.UITests;

public class CourseCardTests : IDisposable
{
    private readonly BunitContext _ctx = new();

    public void Dispose() => _ctx.Dispose();

    private IRenderedComponent<T> Render<T>(Action<ComponentParameterCollectionBuilder<T>> parameterBuilder) where T : IComponent
        => _ctx.Render(parameterBuilder);

    [Fact]
    public void CourseCard_ShouldRenderCourseTitle()
    {
        var course = new CourseDto
        {
            Id = 1,
            Title = "Test Course",
            ShortDescription = "Test Description",
            Price = 999,
            Rating = 4.5,
            RatingCount = 100,
            ImageUrl = "https://via.placeholder.com/400x225",
            Category = "Dev",
            Level = "Beginner",
            InstructorName = "Jane"
        };

        var cut = Render<CourseCard>(parameters => parameters
            .Add(p => p.Course, course));

        cut.Find("h5").TextContent.Should().Be("Test Course");
    }

    [Fact]
    public void CourseCard_ShouldDisplayFreeTag_WhenCourseIsFree()
    {
        var course = new CourseDto
        {
            Id = 1,
            Title = "Free Course",
            ShortDescription = "Description",
            Price = 0,
            Category = "Dev",
            Level = "Beginner",
            InstructorName = "Jane"
        };

        var cut = Render<CourseCard>(parameters => parameters
            .Add(p => p.Course, course));

        cut.Markup.Should().Contain("ฟรี");
    }

    [Fact]
    public void CourseCard_ShouldDisplayPrice_WhenCourseIsNotFree()
    {
        var course = new CourseDto
        {
            Id = 1,
            Title = "Paid Course",
            ShortDescription = "Description",
            Price = 1999,
            Category = "Dev",
            Level = "Beginner",
            InstructorName = "Jane"
        };

        var cut = Render<CourseCard>(parameters => parameters
            .Add(p => p.Course, course));

        cut.Markup.Should().Contain("฿1,999");
    }

    [Fact]
    public void CourseCard_ShouldRenderRating()
    {
        var course = new CourseDto
        {
            Id = 1,
            Title = "Course",
            ShortDescription = "Description",
            Rating = 4.8,
            RatingCount = 20,
            Price = 0,
            Category = "Dev",
            Level = "Beginner",
            InstructorName = "Jane"
        };

        var cut = Render<CourseCard>(parameters => parameters
            .Add(p => p.Course, course));

        cut.Markup.Should().Contain("4.8");
    }

    [Fact]
    public void CourseCard_ShouldRenderRatingCount()
    {
        var course = new CourseDto
        {
            Id = 1,
            Title = "Popular Course",
            ShortDescription = "Description",
            Rating = 4.2,
            RatingCount = 5000,
            Price = 0,
            Category = "Dev",
            Level = "Beginner",
            InstructorName = "Jane"
        };

        var cut = Render<CourseCard>(parameters => parameters
            .Add(p => p.Course, course));

        cut.Markup.Should().Contain("5,000");
    }
}
