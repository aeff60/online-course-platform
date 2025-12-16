using System.Net.Http.Json;
using OnlineCoursePlatform.Shared.DTOs;

namespace OnlineCoursePlatform.Client.Services;

public interface IEnrollmentApiService
{
    Task<List<EnrollmentDto>> GetMyEnrollmentsAsync();
    Task<EnrollmentDetailDto?> GetEnrollmentAsync(int courseId);
    Task<EnrollmentDto?> EnrollCourseAsync(int courseId);
    Task<bool> UpdateProgressAsync(UpdateProgressDto dto);
    Task<DashboardStatsDto?> GetDashboardStatsAsync();
    Task<bool> CheckEnrollmentAsync(int courseId);
}

public class EnrollmentApiService : IEnrollmentApiService
{
    private readonly HttpClient _httpClient;

    public EnrollmentApiService(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<List<EnrollmentDto>> GetMyEnrollmentsAsync()
    {
        try
        {
            var response = await _httpClient.GetFromJsonAsync<ApiResponse<List<EnrollmentDto>>>("api/enrollments");
            return response?.Data ?? new List<EnrollmentDto>();
        }
        catch
        {
            return new List<EnrollmentDto>();
        }
    }

    public async Task<EnrollmentDetailDto?> GetEnrollmentAsync(int courseId)
    {
        try
        {
            var response = await _httpClient.GetFromJsonAsync<ApiResponse<EnrollmentDetailDto>>($"api/enrollments/course/{courseId}");
            return response?.Data;
        }
        catch
        {
            return null;
        }
    }

    public async Task<EnrollmentDto?> EnrollCourseAsync(int courseId)
    {
        try
        {
            var dto = new CreateEnrollmentDto { CourseId = courseId };
            var response = await _httpClient.PostAsJsonAsync("api/enrollments", dto);
            if (response.IsSuccessStatusCode)
            {
                var result = await response.Content.ReadFromJsonAsync<ApiResponse<EnrollmentDto>>();
                return result?.Data;
            }
            return null;
        }
        catch
        {
            return null;
        }
    }

    public async Task<bool> UpdateProgressAsync(UpdateProgressDto dto)
    {
        try
        {
            var response = await _httpClient.PostAsJsonAsync("api/enrollments/progress", dto);
            return response.IsSuccessStatusCode;
        }
        catch
        {
            return false;
        }
    }

    public async Task<DashboardStatsDto?> GetDashboardStatsAsync()
    {
        try
        {
            var response = await _httpClient.GetFromJsonAsync<ApiResponse<DashboardStatsDto>>("api/enrollments/dashboard");
            return response?.Data;
        }
        catch
        {
            return null;
        }
    }

    public async Task<bool> CheckEnrollmentAsync(int courseId)
    {
        try
        {
            var response = await _httpClient.GetFromJsonAsync<ApiResponse<bool>>($"api/enrollments/check/{courseId}");
            return response?.Data ?? false;
        }
        catch
        {
            return false;
        }
    }
}
