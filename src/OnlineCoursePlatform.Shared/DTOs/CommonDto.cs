namespace OnlineCoursePlatform.Shared.DTOs;

public class DashboardStatsDto
{
    public int EnrolledCourses { get; set; }
    public int CompletedCourses { get; set; }
    public int InProgressCourses { get; set; }
    public int Certificates { get; set; }
    public double TotalLearningHours { get; set; }
    public int TotalLessonsCompleted { get; set; }
    public double AverageProgress { get; set; }
    public List<RecentActivityDto> RecentActivities { get; set; } = new();
    public List<EnrollmentDto> ContinueLearning { get; set; } = new();
}

public class RecentActivityDto
{
    public string Type { get; set; } = string.Empty; // enrolled, completed_lesson, completed_course, earned_certificate
    public string Title { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public DateTime Timestamp { get; set; }
    public int? CourseId { get; set; }
    public int? LessonId { get; set; }
}

public class ApiResponse<T>
{
    public bool Success { get; set; }
    public string? Message { get; set; }
    public T? Data { get; set; }
    public List<string> Errors { get; set; } = new();

    public static ApiResponse<T> SuccessResponse(T data, string? message = null)
    {
        return new ApiResponse<T>
        {
            Success = true,
            Message = message,
            Data = data
        };
    }

    public static ApiResponse<T> ErrorResponse(string message, List<string>? errors = null)
    {
        return new ApiResponse<T>
        {
            Success = false,
            Message = message,
            Errors = errors ?? new List<string>()
        };
    }
}

public class PaginatedResponse<T>
{
    public List<T> Items { get; set; } = new();
    public int TotalCount { get; set; }
    public int PageNumber { get; set; }
    public int PageSize { get; set; }
    public int TotalPages => (int)Math.Ceiling(TotalCount / (double)PageSize);
    public bool HasPreviousPage => PageNumber > 1;
    public bool HasNextPage => PageNumber < TotalPages;
}

public class CourseFilterDto
{
    public string? SearchTerm { get; set; }
    public string? Category { get; set; }
    public string? Level { get; set; }
    public decimal? MinPrice { get; set; }
    public decimal? MaxPrice { get; set; }
    public bool? IsFree { get; set; }
    public double? MinRating { get; set; }
    public string? SortBy { get; set; } = "newest"; // newest, popular, rating, price-low, price-high
    public int PageNumber { get; set; } = 1;
    public int PageSize { get; set; } = 12;
}
