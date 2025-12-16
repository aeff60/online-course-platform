using Microsoft.AspNetCore.Mvc;
using OnlineCoursePlatform.Server.Services;
using OnlineCoursePlatform.Shared.DTOs;

namespace OnlineCoursePlatform.Server.Controllers;

[ApiController]
[Route("api/[controller]")]
public class EnrollmentsController : ControllerBase
{
    private readonly IEnrollmentService _enrollmentService;

    public EnrollmentsController(IEnrollmentService enrollmentService)
    {
        _enrollmentService = enrollmentService;
    }

    [HttpGet]
    public async Task<ActionResult<ApiResponse<List<EnrollmentDto>>>> GetMyEnrollments()
    {
        // In a real app, get user ID from authenticated user
        var userId = 1;
        var enrollments = await _enrollmentService.GetUserEnrollmentsAsync(userId);
        return Ok(ApiResponse<List<EnrollmentDto>>.SuccessResponse(enrollments));
    }

    [HttpGet("course/{courseId}")]
    public async Task<ActionResult<ApiResponse<EnrollmentDetailDto>>> GetEnrollment(int courseId)
    {
        var userId = 1;
        var enrollment = await _enrollmentService.GetEnrollmentAsync(userId, courseId);
        if (enrollment == null)
        {
            return NotFound(ApiResponse<EnrollmentDetailDto>.ErrorResponse("Enrollment not found"));
        }
        return Ok(ApiResponse<EnrollmentDetailDto>.SuccessResponse(enrollment));
    }

    [HttpPost]
    public async Task<ActionResult<ApiResponse<EnrollmentDto>>> EnrollCourse([FromBody] CreateEnrollmentDto dto)
    {
        var userId = 1;
        var enrollment = await _enrollmentService.EnrollCourseAsync(userId, dto.CourseId);
        if (enrollment == null)
        {
            return BadRequest(ApiResponse<EnrollmentDto>.ErrorResponse("Already enrolled in this course"));
        }
        return Ok(ApiResponse<EnrollmentDto>.SuccessResponse(enrollment, "Successfully enrolled in the course"));
    }

    [HttpPost("progress")]
    public async Task<ActionResult<ApiResponse<bool>>> UpdateProgress([FromBody] UpdateProgressDto dto)
    {
        var userId = 1;
        var result = await _enrollmentService.UpdateProgressAsync(dto, userId);
        if (!result)
        {
            return BadRequest(ApiResponse<bool>.ErrorResponse("Failed to update progress"));
        }
        return Ok(ApiResponse<bool>.SuccessResponse(true, "Progress updated"));
    }

    [HttpGet("dashboard")]
    public async Task<ActionResult<ApiResponse<DashboardStatsDto>>> GetDashboardStats()
    {
        var userId = 1;
        var stats = await _enrollmentService.GetDashboardStatsAsync(userId);
        return Ok(ApiResponse<DashboardStatsDto>.SuccessResponse(stats));
    }

    [HttpGet("check/{courseId}")]
    public async Task<ActionResult<ApiResponse<bool>>> CheckEnrollment(int courseId)
    {
        var userId = 1;
        var isEnrolled = await _enrollmentService.IsEnrolledAsync(userId, courseId);
        return Ok(ApiResponse<bool>.SuccessResponse(isEnrolled));
    }
}
