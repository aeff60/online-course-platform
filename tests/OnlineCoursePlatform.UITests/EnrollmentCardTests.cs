using Bunit;
using FluentAssertions;
using Microsoft.AspNetCore.Components;
using OnlineCoursePlatform.Client.Components;
using OnlineCoursePlatform.Shared.DTOs;
using Xunit;

namespace OnlineCoursePlatform.UITests;

public class EnrollmentCardTests : IDisposable
{
    private readonly BunitContext _ctx = new();

    public void Dispose() => _ctx.Dispose();

    private IRenderedComponent<T> Render<T>(Action<ComponentParameterCollectionBuilder<T>> parameterBuilder) where T : IComponent
        => _ctx.Render(parameterBuilder);
    [Fact]
    public void EnrollmentCard_ShouldRenderCourseTitle()
    {
        // Arrange
        var enrollment = new EnrollmentDto
        {
            Id = 1,
            CourseTitle = "My Enrolled Course",
            CourseImage = "https://via.placeholder.com/400x225",
            ProgressPercentage = 50,
            EnrolledAt = DateTime.UtcNow,
            Status = "Active",
            CompletedLessons = 5,
            TotalLessons = 10
        };

        var cut = Render<EnrollmentCard>(parameters => parameters
            .Add(p => p.Enrollment, enrollment));

        // Assert
        cut.Find("h5").TextContent.Should().Be("My Enrolled Course");
    }

    [Fact]
    public void EnrollmentCard_ShouldDisplayProgress()
    {
        // Arrange
        var enrollment = new EnrollmentDto
        {
            Id = 1,
            CourseTitle = "Course",
            ProgressPercentage = 75,
            Status = "Active",
            CompletedLessons = 7,
            TotalLessons = 10
        };

        var cut = Render<EnrollmentCard>(parameters => parameters
            .Add(p => p.Enrollment, enrollment));

        cut.Markup.Should().Contain("75%");
    }

    [Fact]
    public void EnrollmentCard_ShouldDisplayActiveStatus()
    {
        // Arrange
        var enrollment = new EnrollmentDto
        {
            Id = 1,
            CourseTitle = "Active Course",
            ProgressPercentage = 30,
            Status = "Active",
            CompletedLessons = 3,
            TotalLessons = 10
        };

        var cut = Render<EnrollmentCard>(parameters => parameters
            .Add(p => p.Enrollment, enrollment));

        cut.Markup.Should().Contain("เรียนต่อ");
    }

    [Fact]
    public void EnrollmentCard_ShouldDisplayCompletedStatus()
    {
        // Arrange
        var enrollment = new EnrollmentDto
        {
            Id = 1,
            CourseTitle = "Completed Course",
            ProgressPercentage = 100,
            Status = "Completed",
            CompletedLessons = 12,
            TotalLessons = 12
        };

        var cut = Render<EnrollmentCard>(parameters => parameters
            .Add(p => p.Enrollment, enrollment));

        cut.Markup.Should().Contain("เรียนจบแล้ว");
    }

    [Fact]
    public void EnrollmentCard_ProgressBar_ShouldHaveCorrectWidth()
    {
        // Arrange
        var enrollment = new EnrollmentDto
        {
            Id = 1,
            CourseTitle = "Course",
            ProgressPercentage = 60,
            Status = "Active",
            CompletedLessons = 6,
            TotalLessons = 10
        };

        var cut = Render<EnrollmentCard>(parameters => parameters
            .Add(p => p.Enrollment, enrollment));

        var progressBar = cut.Find(".progress-bar");
        progressBar.GetAttribute("style").Should().Contain("width: 60%");
    }
}
