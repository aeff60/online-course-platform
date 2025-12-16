using Bunit;
using FluentAssertions;
using Microsoft.AspNetCore.Components;
using OnlineCoursePlatform.Client.Components;
using OnlineCoursePlatform.Shared.DTOs;
using Xunit;

namespace OnlineCoursePlatform.UITests;

public class LessonItemTests : IDisposable
{
    private readonly BunitContext _ctx = new();

    public void Dispose() => _ctx.Dispose();

    private IRenderedComponent<T> Render<T>(Action<ComponentParameterCollectionBuilder<T>> parameterBuilder) where T : IComponent
        => _ctx.Render(parameterBuilder);

    [Fact]
    public void LessonItem_ShouldRenderLessonTitle()
    {
        var lesson = new LessonDto
        {
            Id = 1,
            Title = "Introduction to Programming",
            Description = "Learn the basics",
            Duration = "30 นาที",
            Order = 1,
            IsCompleted = false
        };

        var cut = Render<LessonItem>(parameters => parameters
            .Add(p => p.Lesson, lesson));

        cut.Markup.Should().Contain("Introduction to Programming");
    }

    [Fact]
    public void LessonItem_ShouldDisplayDuration()
    {
        var lesson = new LessonDto
        {
            Id = 1,
            Title = "Advanced Topics",
            Duration = "45 นาที",
            Order = 2,
            IsCompleted = false
        };

        var cut = Render<LessonItem>(parameters => parameters
            .Add(p => p.Lesson, lesson));

        cut.Markup.Should().Contain("45 นาที");
    }

    [Fact]
    public void LessonItem_ShouldShowCompletedIcon_WhenCompleted()
    {
        var lesson = new LessonDto
        {
            Id = 1,
            Title = "Completed Lesson",
            Duration = "20 นาที",
            Order = 1,
            IsCompleted = true
        };

        var cut = Render<LessonItem>(parameters => parameters
            .Add(p => p.Lesson, lesson));

        cut.Markup.Should().Contain("bi-check-circle-fill");
    }

    [Fact]
    public void LessonItem_ShouldShowActiveIcon_WhenActive()
    {
        var lesson = new LessonDto
        {
            Id = 1,
            Title = "Incomplete Lesson",
            Duration = "25 นาที",
            Order = 1,
            IsCompleted = false
        };

        var cut = Render<LessonItem>(parameters => parameters
            .Add(p => p.Lesson, lesson)
            .Add(p => p.IsActive, true));

        cut.Markup.Should().Contain("bi-play-circle-fill");
    }

    [Fact]
    public void LessonItem_ShouldDisplayOrder()
    {
        var lesson = new LessonDto
        {
            Id = 1,
            Title = "Lesson Three",
            Duration = "30 นาที",
            Order = 3,
            IsCompleted = false
        };

        var cut = Render<LessonItem>(parameters => parameters
            .Add(p => p.Lesson, lesson));

        cut.Markup.Should().Contain("3.");
    }

    [Fact]
    public void LessonItem_ShouldHaveLessonItemClass()
    {
        var lesson = new LessonDto
        {
            Id = 1,
            Title = "Test Lesson",
            Duration = "15 นาที",
            Order = 1,
            IsCompleted = false
        };

        var cut = Render<LessonItem>(parameters => parameters
            .Add(p => p.Lesson, lesson));

        cut.Find(".lesson-item").Should().NotBeNull();
    }
}
