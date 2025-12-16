namespace OnlineCoursePlatform.Shared.DTOs;

public class EnrollmentDto
{
    public int Id { get; set; }
    public int CourseId { get; set; }
    public string CourseTitle { get; set; } = string.Empty;
    public string CourseImage { get; set; } = string.Empty;
    public string InstructorName { get; set; } = string.Empty;
    public DateTime EnrolledAt { get; set; }
    public double ProgressPercentage { get; set; }
    public int CompletedLessons { get; set; }
    public int TotalLessons { get; set; }
    public string Status { get; set; } = string.Empty;
    public DateTime? LastAccessedAt { get; set; }
    public int? CurrentLessonId { get; set; }
}

public class EnrollmentDetailDto : EnrollmentDto
{
    public List<LessonProgressDto> LessonProgresses { get; set; } = new();
    public DateTime? CompletedAt { get; set; }
    public bool HasCertificate { get; set; }
    public string? CertificateId { get; set; }
}

public class CreateEnrollmentDto
{
    public int CourseId { get; set; }
}

public class LessonProgressDto
{
    public int LessonId { get; set; }
    public string LessonTitle { get; set; } = string.Empty;
    public bool IsCompleted { get; set; }
    public DateTime? CompletedAt { get; set; }
    public double WatchedPercentage { get; set; }
}

public class UpdateProgressDto
{
    public int EnrollmentId { get; set; }
    public int LessonId { get; set; }
    public double WatchedPercentage { get; set; }
    public bool MarkAsCompleted { get; set; }
}
