using OnlineCoursePlatform.Shared.DTOs;
using OnlineCoursePlatform.Shared.Models;

namespace OnlineCoursePlatform.Server.Services;

public interface IEnrollmentService
{
    Task<List<EnrollmentDto>> GetUserEnrollmentsAsync(int userId);
    Task<EnrollmentDetailDto?> GetEnrollmentAsync(int userId, int courseId);
    Task<EnrollmentDto?> EnrollCourseAsync(int userId, int courseId);
    Task<bool> UpdateProgressAsync(UpdateProgressDto dto, int userId);
    Task<DashboardStatsDto> GetDashboardStatsAsync(int userId);
    Task<bool> IsEnrolledAsync(int userId, int courseId);
}

public class EnrollmentService : IEnrollmentService
{
    private static readonly List<Enrollment> _enrollments = new();
    private static int _nextId = 1;

    public async Task<List<EnrollmentDto>> GetUserEnrollmentsAsync(int userId)
    {
        await Task.Delay(50);

        return _enrollments
            .Where(e => e.UserId == userId)
            .Select(MapToDto)
            .ToList();
    }

    public async Task<EnrollmentDetailDto?> GetEnrollmentAsync(int userId, int courseId)
    {
        await Task.Delay(50);

        var enrollment = _enrollments.FirstOrDefault(e => e.UserId == userId && e.CourseId == courseId);
        return enrollment == null ? null : MapToDetailDto(enrollment);
    }

    public async Task<EnrollmentDto?> EnrollCourseAsync(int userId, int courseId)
    {
        await Task.Delay(100);

        // Check if already enrolled
        if (_enrollments.Any(e => e.UserId == userId && e.CourseId == courseId))
        {
            return null;
        }

        var enrollment = new Enrollment
        {
            Id = _nextId++,
            UserId = userId,
            CourseId = courseId,
            EnrolledAt = DateTime.UtcNow,
            Status = EnrollmentStatus.Active,
            ProgressPercentage = 0,
            CompletedLessons = 0,
            LastAccessedAt = DateTime.UtcNow
        };

        // Generate sample course data for the enrollment
        enrollment.Course = new Course
        {
            Id = courseId,
            Title = GetSampleCourseTitle(courseId),
            ImageUrl = "https://images.unsplash.com/photo-1498050108023-c5249f4df085?w=800",
            InstructorName = "Sample Instructor",
            TotalLessons = 10
        };

        _enrollments.Add(enrollment);
        return MapToDto(enrollment);
    }

    public async Task<bool> UpdateProgressAsync(UpdateProgressDto dto, int userId)
    {
        await Task.Delay(50);

        var enrollment = _enrollments.FirstOrDefault(e => e.Id == dto.EnrollmentId && e.UserId == userId);
        if (enrollment == null) return false;

        var progress = enrollment.LessonProgresses.FirstOrDefault(p => p.LessonId == dto.LessonId);
        if (progress == null)
        {
            progress = new LessonProgress
            {
                Id = enrollment.LessonProgresses.Count + 1,
                EnrollmentId = enrollment.Id,
                LessonId = dto.LessonId
            };
            enrollment.LessonProgresses.Add(progress);
        }

        progress.WatchedPercentage = dto.WatchedPercentage;
        progress.LastWatchedAt = DateTime.UtcNow;

        if (dto.MarkAsCompleted && !progress.IsCompleted)
        {
            progress.IsCompleted = true;
            progress.CompletedAt = DateTime.UtcNow;
            enrollment.CompletedLessons++;
        }

        // Update overall progress
        if (enrollment.Course != null && enrollment.Course.TotalLessons > 0)
        {
            enrollment.ProgressPercentage = (double)enrollment.CompletedLessons / enrollment.Course.TotalLessons * 100;

            if (enrollment.ProgressPercentage >= 100)
            {
                enrollment.Status = EnrollmentStatus.Completed;
                enrollment.CompletedAt = DateTime.UtcNow;
            }
        }

        enrollment.LastAccessedAt = DateTime.UtcNow;
        enrollment.CurrentLessonId = dto.LessonId;

        return true;
    }

    public async Task<DashboardStatsDto> GetDashboardStatsAsync(int userId)
    {
        await Task.Delay(50);

        var userEnrollments = _enrollments.Where(e => e.UserId == userId).ToList();

        return new DashboardStatsDto
        {
            EnrolledCourses = userEnrollments.Count,
            CompletedCourses = userEnrollments.Count(e => e.Status == EnrollmentStatus.Completed),
            InProgressCourses = userEnrollments.Count(e => e.Status == EnrollmentStatus.Active),
            Certificates = userEnrollments.Count(e => e.Status == EnrollmentStatus.Completed),
            TotalLearningHours = userEnrollments.Sum(e => e.LessonProgresses.Sum(p => p.WatchedDuration.TotalHours)),
            TotalLessonsCompleted = userEnrollments.Sum(e => e.CompletedLessons),
            AverageProgress = userEnrollments.Any() ? userEnrollments.Average(e => e.ProgressPercentage) : 0,
            ContinueLearning = userEnrollments
                .Where(e => e.Status == EnrollmentStatus.Active)
                .OrderByDescending(e => e.LastAccessedAt)
                .Take(3)
                .Select(MapToDto)
                .ToList(),
            RecentActivities = GenerateRecentActivities(userEnrollments)
        };
    }

    public async Task<bool> IsEnrolledAsync(int userId, int courseId)
    {
        await Task.Delay(10);
        return _enrollments.Any(e => e.UserId == userId && e.CourseId == courseId);
    }

    private static EnrollmentDto MapToDto(Enrollment enrollment)
    {
        return new EnrollmentDto
        {
            Id = enrollment.Id,
            CourseId = enrollment.CourseId,
            CourseTitle = enrollment.Course?.Title ?? "Unknown Course",
            CourseImage = enrollment.Course?.ImageUrl ?? "",
            InstructorName = enrollment.Course?.InstructorName ?? "",
            EnrolledAt = enrollment.EnrolledAt,
            ProgressPercentage = enrollment.ProgressPercentage,
            CompletedLessons = enrollment.CompletedLessons,
            TotalLessons = enrollment.Course?.TotalLessons ?? 0,
            Status = enrollment.Status.ToString(),
            LastAccessedAt = enrollment.LastAccessedAt,
            CurrentLessonId = enrollment.CurrentLessonId
        };
    }

    private static EnrollmentDetailDto MapToDetailDto(Enrollment enrollment)
    {
        return new EnrollmentDetailDto
        {
            Id = enrollment.Id,
            CourseId = enrollment.CourseId,
            CourseTitle = enrollment.Course?.Title ?? "Unknown Course",
            CourseImage = enrollment.Course?.ImageUrl ?? "",
            InstructorName = enrollment.Course?.InstructorName ?? "",
            EnrolledAt = enrollment.EnrolledAt,
            ProgressPercentage = enrollment.ProgressPercentage,
            CompletedLessons = enrollment.CompletedLessons,
            TotalLessons = enrollment.Course?.TotalLessons ?? 0,
            Status = enrollment.Status.ToString(),
            LastAccessedAt = enrollment.LastAccessedAt,
            CurrentLessonId = enrollment.CurrentLessonId,
            CompletedAt = enrollment.CompletedAt,
            HasCertificate = enrollment.Status == EnrollmentStatus.Completed,
            LessonProgresses = enrollment.LessonProgresses.Select(p => new LessonProgressDto
            {
                LessonId = p.LessonId,
                LessonTitle = p.Lesson?.Title ?? $"Lesson {p.LessonId}",
                IsCompleted = p.IsCompleted,
                CompletedAt = p.CompletedAt,
                WatchedPercentage = p.WatchedPercentage
            }).ToList()
        };
    }

    private static List<RecentActivityDto> GenerateRecentActivities(List<Enrollment> enrollments)
    {
        var activities = new List<RecentActivityDto>();

        foreach (var enrollment in enrollments.OrderByDescending(e => e.EnrolledAt).Take(5))
        {
            activities.Add(new RecentActivityDto
            {
                Type = "enrolled",
                Title = "ลงทะเบียนคอร์ส",
                Description = enrollment.Course?.Title ?? "Unknown Course",
                Timestamp = enrollment.EnrolledAt,
                CourseId = enrollment.CourseId
            });
        }

        return activities.OrderByDescending(a => a.Timestamp).Take(10).ToList();
    }

    private static string GetSampleCourseTitle(int courseId)
    {
        return courseId switch
        {
            1 => "Complete Web Development Bootcamp 2024",
            2 => "Machine Learning with Python",
            3 => "Flutter & Dart - Build iOS and Android Apps",
            4 => "AWS Cloud Practitioner",
            5 => "เริ่มต้น Git และ GitHub",
            6 => "UI/UX Design Masterclass",
            _ => $"Course {courseId}"
        };
    }
}
