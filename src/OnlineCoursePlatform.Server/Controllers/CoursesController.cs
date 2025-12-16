using Microsoft.AspNetCore.Mvc;
using OnlineCoursePlatform.Server.Services;
using OnlineCoursePlatform.Shared.DTOs;

namespace OnlineCoursePlatform.Server.Controllers;

[ApiController]
[Route("api/[controller]")]
public class CoursesController : ControllerBase
{
    private readonly ICourseService _courseService;

    public CoursesController(ICourseService courseService)
    {
        _courseService = courseService;
    }

    [HttpGet]
    public async Task<ActionResult<ApiResponse<PaginatedResponse<CourseDto>>>> GetCourses([FromQuery] CourseFilterDto filter)
    {
        var result = await _courseService.GetCoursesAsync(filter);
        return Ok(ApiResponse<PaginatedResponse<CourseDto>>.SuccessResponse(result));
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<ApiResponse<CourseDetailDto>>> GetCourse(int id)
    {
        var course = await _courseService.GetCourseByIdAsync(id);
        if (course == null)
        {
            return NotFound(ApiResponse<CourseDetailDto>.ErrorResponse("Course not found"));
        }
        return Ok(ApiResponse<CourseDetailDto>.SuccessResponse(course));
    }

    [HttpGet("featured")]
    public async Task<ActionResult<ApiResponse<List<CourseDto>>>> GetFeaturedCourses([FromQuery] int count = 6)
    {
        var courses = await _courseService.GetFeaturedCoursesAsync(count);
        return Ok(ApiResponse<List<CourseDto>>.SuccessResponse(courses));
    }

    [HttpGet("popular")]
    public async Task<ActionResult<ApiResponse<List<CourseDto>>>> GetPopularCourses([FromQuery] int count = 6)
    {
        var courses = await _courseService.GetPopularCoursesAsync(count);
        return Ok(ApiResponse<List<CourseDto>>.SuccessResponse(courses));
    }

    [HttpGet("categories")]
    public async Task<ActionResult<ApiResponse<List<string>>>> GetCategories()
    {
        var categories = await _courseService.GetCategoriesAsync();
        return Ok(ApiResponse<List<string>>.SuccessResponse(categories));
    }

    [HttpPost]
    public async Task<ActionResult<ApiResponse<CourseDto>>> CreateCourse([FromBody] CreateCourseDto dto)
    {
        // In a real app, get instructor ID from authenticated user
        var instructorId = 1;
        var course = await _courseService.CreateCourseAsync(dto, instructorId);
        return CreatedAtAction(nameof(GetCourse), new { id = course.Id }, ApiResponse<CourseDto>.SuccessResponse(course));
    }

    [HttpPut("{id}")]
    public async Task<ActionResult<ApiResponse<CourseDto>>> UpdateCourse(int id, [FromBody] UpdateCourseDto dto)
    {
        if (id != dto.Id)
        {
            return BadRequest(ApiResponse<CourseDto>.ErrorResponse("ID mismatch"));
        }

        var course = await _courseService.UpdateCourseAsync(dto);
        if (course == null)
        {
            return NotFound(ApiResponse<CourseDto>.ErrorResponse("Course not found"));
        }
        return Ok(ApiResponse<CourseDto>.SuccessResponse(course));
    }

    [HttpDelete("{id}")]
    public async Task<ActionResult<ApiResponse<bool>>> DeleteCourse(int id)
    {
        var result = await _courseService.DeleteCourseAsync(id);
        if (!result)
        {
            return NotFound(ApiResponse<bool>.ErrorResponse("Course not found"));
        }
        return Ok(ApiResponse<bool>.SuccessResponse(true, "Course deleted successfully"));
    }
}
