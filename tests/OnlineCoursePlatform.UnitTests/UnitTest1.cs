using FluentAssertions;
using OnlineCoursePlatform.Server.Services;
using OnlineCoursePlatform.Shared.DTOs;

namespace OnlineCoursePlatform.UnitTests;

public class CourseServiceTests
{
    private readonly ICourseService _courseService;

    public CourseServiceTests()
    {
        _courseService = new CourseService();
    }

    [Fact]
    public async Task GetCoursesAsync_ShouldReturnPaginatedResponse()
    {
        // Arrange
        var filter = new CourseFilterDto { PageNumber = 1, PageSize = 10 };

        // Act
        var result = await _courseService.GetCoursesAsync(filter);

        // Assert
        result.Should().NotBeNull();
        result.Items.Should().NotBeNull();
        result.PageNumber.Should().Be(1);
        result.PageSize.Should().Be(10);
    }

    [Fact]
    public async Task GetCoursesAsync_WithSearchTerm_ShouldFilterResults()
    {
        // Arrange
        var filter = new CourseFilterDto 
        { 
            SearchTerm = "Web", 
            PageNumber = 1, 
            PageSize = 10 
        };

        // Act
        var result = await _courseService.GetCoursesAsync(filter);

        // Assert
        result.Should().NotBeNull();
        result.Items.Should().OnlyContain(c => 
            c.Title.Contains("Web", StringComparison.OrdinalIgnoreCase) ||
            c.ShortDescription.Contains("Web", StringComparison.OrdinalIgnoreCase) ||
            c.Tags.Any(t => t.Contains("Web", StringComparison.OrdinalIgnoreCase)));
    }

    [Fact]
    public async Task GetCoursesAsync_WithCategoryFilter_ShouldReturnOnlyMatchingCategory()
    {
        // Arrange
        var filter = new CourseFilterDto 
        { 
            Category = "Web Development",
            PageNumber = 1, 
            PageSize = 10 
        };

        // Act
        var result = await _courseService.GetCoursesAsync(filter);

        // Assert
        result.Should().NotBeNull();
        result.Items.Should().OnlyContain(c => 
            c.Category.Equals("Web Development", StringComparison.OrdinalIgnoreCase));
    }

    [Fact]
    public async Task GetCoursesAsync_WithFreeFilter_ShouldReturnOnlyFreeCourses()
    {
        // Arrange
        var filter = new CourseFilterDto 
        { 
            IsFree = true,
            PageNumber = 1, 
            PageSize = 10 
        };

        // Act
        var result = await _courseService.GetCoursesAsync(filter);

        // Assert
        result.Should().NotBeNull();
        result.Items.Should().OnlyContain(c => c.IsFree);
    }

    [Fact]
    public async Task GetCourseByIdAsync_WithValidId_ShouldReturnCourse()
    {
        // Arrange
        var courseId = 1;

        // Act
        var result = await _courseService.GetCourseByIdAsync(courseId);

        // Assert
        result.Should().NotBeNull();
        result!.Id.Should().Be(courseId);
        result.Title.Should().NotBeNullOrEmpty();
        result.Lessons.Should().NotBeNull();
    }

    [Fact]
    public async Task GetCourseByIdAsync_WithInvalidId_ShouldReturnNull()
    {
        // Arrange
        var courseId = 99999;

        // Act
        var result = await _courseService.GetCourseByIdAsync(courseId);

        // Assert
        result.Should().BeNull();
    }

    [Fact]
    public async Task GetFeaturedCoursesAsync_ShouldReturnRequestedCount()
    {
        // Arrange
        var count = 3;

        // Act
        var result = await _courseService.GetFeaturedCoursesAsync(count);

        // Assert
        result.Should().NotBeNull();
        result.Count.Should().BeLessThanOrEqualTo(count);
    }

    [Fact]
    public async Task GetPopularCoursesAsync_ShouldReturnSortedByEnrollment()
    {
        // Arrange
        var count = 6;

        // Act
        var result = await _courseService.GetPopularCoursesAsync(count);

        // Assert
        result.Should().NotBeNull();
        result.Should().BeInDescendingOrder(c => c.EnrollmentCount);
    }

    [Fact]
    public async Task GetCategoriesAsync_ShouldReturnDistinctCategories()
    {
        // Act
        var result = await _courseService.GetCategoriesAsync();

        // Assert
        result.Should().NotBeNull();
        result.Should().OnlyHaveUniqueItems();
        result.Should().BeInAscendingOrder();
    }

    [Fact]
    public async Task CreateCourseAsync_ShouldCreateNewCourse()
    {
        // Arrange
        var dto = new CreateCourseDto
        {
            Title = "Test Course",
            Description = "Test Description",
            ShortDescription = "Short Description",
            Price = 999,
            Category = "Test Category",
            Level = "Beginner"
        };

        // Act
        var result = await _courseService.CreateCourseAsync(dto, 1);

        // Assert
        result.Should().NotBeNull();
        result.Title.Should().Be(dto.Title);
        result.Price.Should().Be(dto.Price);
        result.Id.Should().BeGreaterThan(0);
    }
}
