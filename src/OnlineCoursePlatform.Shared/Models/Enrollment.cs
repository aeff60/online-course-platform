namespace OnlineCoursePlatform.Shared.Models;

public class Enrollment
{
    public int Id { get; set; }
    public int UserId { get; set; }
    public int CourseId { get; set; }
    public DateTime EnrolledAt { get; set; } = DateTime.UtcNow;
    public DateTime? CompletedAt { get; set; }
    public EnrollmentStatus Status { get; set; } = EnrollmentStatus.Active;
    public double ProgressPercentage { get; set; }
    public int CompletedLessons { get; set; }
    public DateTime? LastAccessedAt { get; set; }
    public int? CurrentLessonId { get; set; }
    public Course? Course { get; set; }
    public User? User { get; set; }
    public List<LessonProgress> LessonProgresses { get; set; } = new();
}

public enum EnrollmentStatus
{
    Active,
    Completed,
    Expired,
    Cancelled
}
