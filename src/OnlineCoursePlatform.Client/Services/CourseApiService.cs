using System.Net.Http.Json;
using OnlineCoursePlatform.Shared.DTOs;

namespace OnlineCoursePlatform.Client.Services;

public interface ICourseApiService
{
    Task<PaginatedResponse<CourseDto>?> GetCoursesAsync(CourseFilterDto? filter = null);
    Task<CourseDetailDto?> GetCourseByIdAsync(int id);
    Task<List<CourseDto>> GetFeaturedCoursesAsync(int count = 6);
    Task<List<CourseDto>> GetPopularCoursesAsync(int count = 6);
    Task<List<string>> GetCategoriesAsync();
}

public class CourseApiService : ICourseApiService
{
    private readonly HttpClient _httpClient;

    public CourseApiService(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<PaginatedResponse<CourseDto>?> GetCoursesAsync(CourseFilterDto? filter = null)
    {
        var queryParams = new List<string>();

        if (filter != null)
        {
            if (!string.IsNullOrEmpty(filter.SearchTerm))
                queryParams.Add($"searchTerm={Uri.EscapeDataString(filter.SearchTerm)}");
            if (!string.IsNullOrEmpty(filter.Category))
                queryParams.Add($"category={Uri.EscapeDataString(filter.Category)}");
            if (!string.IsNullOrEmpty(filter.Level))
                queryParams.Add($"level={Uri.EscapeDataString(filter.Level)}");
            if (filter.IsFree.HasValue)
                queryParams.Add($"isFree={filter.IsFree}");
            if (filter.MinRating.HasValue)
                queryParams.Add($"minRating={filter.MinRating}");
            if (!string.IsNullOrEmpty(filter.SortBy))
                queryParams.Add($"sortBy={Uri.EscapeDataString(filter.SortBy)}");
            queryParams.Add($"pageNumber={filter.PageNumber}");
            queryParams.Add($"pageSize={filter.PageSize}");
        }

        var url = "api/courses" + (queryParams.Any() ? "?" + string.Join("&", queryParams) : "");

        try
        {
            var response = await _httpClient.GetFromJsonAsync<ApiResponse<PaginatedResponse<CourseDto>>>(url);
            return response?.Data;
        }
        catch
        {
            return null;
        }
    }

    public async Task<CourseDetailDto?> GetCourseByIdAsync(int id)
    {
        try
        {
            var response = await _httpClient.GetFromJsonAsync<ApiResponse<CourseDetailDto>>($"api/courses/{id}");
            return response?.Data;
        }
        catch
        {
            return null;
        }
    }

    public async Task<List<CourseDto>> GetFeaturedCoursesAsync(int count = 6)
    {
        try
        {
            var response = await _httpClient.GetFromJsonAsync<ApiResponse<List<CourseDto>>>($"api/courses/featured?count={count}");
            return response?.Data ?? new List<CourseDto>();
        }
        catch
        {
            return new List<CourseDto>();
        }
    }

    public async Task<List<CourseDto>> GetPopularCoursesAsync(int count = 6)
    {
        try
        {
            var response = await _httpClient.GetFromJsonAsync<ApiResponse<List<CourseDto>>>($"api/courses/popular?count={count}");
            return response?.Data ?? new List<CourseDto>();
        }
        catch
        {
            return new List<CourseDto>();
        }
    }

    public async Task<List<string>> GetCategoriesAsync()
    {
        try
        {
            var response = await _httpClient.GetFromJsonAsync<ApiResponse<List<string>>>("api/courses/categories");
            return response?.Data ?? new List<string>();
        }
        catch
        {
            return new List<string>();
        }
    }
}
