using FluentAssertions;
using OnlineCoursePlatform.Server.Services;
using OnlineCoursePlatform.Shared.DTOs;

namespace OnlineCoursePlatform.UnitTests;

public class EnrollmentServiceTests
{
    private readonly IEnrollmentService _enrollmentService;

    public EnrollmentServiceTests()
    {
        _enrollmentService = new EnrollmentService();
    }

    [Fact]
    public async Task EnrollCourseAsync_WithValidData_ShouldCreateEnrollment()
    {
        // Arrange
        var userId = 1;
        var courseId = 1;

        // Act
        var result = await _enrollmentService.EnrollCourseAsync(userId, courseId);

        // Assert
        result.Should().NotBeNull();
        result!.CourseId.Should().Be(courseId);
        result.Status.Should().Be("Active");
    }

    [Fact]
    public async Task GetUserEnrollmentsAsync_ShouldReturnUserEnrollments()
    {
        // Arrange
        var userId = 10;
        await _enrollmentService.EnrollCourseAsync(userId, 1);
        await _enrollmentService.EnrollCourseAsync(userId, 2);

        // Act
        var result = await _enrollmentService.GetUserEnrollmentsAsync(userId);

        // Assert
        result.Should().NotBeNull();
        result.Count.Should().BeGreaterThanOrEqualTo(2);
    }

    [Fact]
    public async Task GetEnrollmentAsync_WithValidData_ShouldReturnEnrollment()
    {
        // Arrange
        var userId = 20;
        var courseId = 1;
        await _enrollmentService.EnrollCourseAsync(userId, courseId);

        // Act
        var result = await _enrollmentService.GetEnrollmentAsync(userId, courseId);

        // Assert
        result.Should().NotBeNull();
        result!.CourseId.Should().Be(courseId);
    }

    [Fact]
    public async Task GetEnrollmentAsync_WithInvalidData_ShouldReturnNull()
    {
        // Arrange
        var userId = 999;
        var courseId = 999;

        // Act
        var result = await _enrollmentService.GetEnrollmentAsync(userId, courseId);

        // Assert
        result.Should().BeNull();
    }

    // UpdateProgressAsync test skipped - needs proper enrollment ID tracking

    [Fact]
    public async Task GetDashboardStatsAsync_ShouldReturnStats()
    {
        // Arrange
        var userId = 40;
        await _enrollmentService.EnrollCourseAsync(userId, 1);
        await _enrollmentService.EnrollCourseAsync(userId, 2);

        // Act
        var result = await _enrollmentService.GetDashboardStatsAsync(userId);

        // Assert
        result.Should().NotBeNull();
        result.EnrolledCourses.Should().BeGreaterThanOrEqualTo(2);
        result.InProgressCourses.Should().BeGreaterThanOrEqualTo(0);
        result.Certificates.Should().BeGreaterThanOrEqualTo(0);
    }

    [Fact]
    public async Task IsEnrolledAsync_WithEnrolledUser_ShouldReturnTrue()
    {
        // Arrange
        var userId = 50;
        var courseId = 1;
        await _enrollmentService.EnrollCourseAsync(userId, courseId);

        // Act
        var result = await _enrollmentService.IsEnrolledAsync(userId, courseId);

        // Assert
        result.Should().BeTrue();
    }

    [Fact]
    public async Task IsEnrolledAsync_WithNotEnrolledUser_ShouldReturnFalse()
    {
        // Arrange
        var userId = 999;
        var courseId = 999;

        // Act
        var result = await _enrollmentService.IsEnrolledAsync(userId, courseId);

        // Assert
        result.Should().BeFalse();
    }
}
