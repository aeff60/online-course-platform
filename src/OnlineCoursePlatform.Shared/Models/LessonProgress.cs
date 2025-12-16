namespace OnlineCoursePlatform.Shared.Models;

public class LessonProgress
{
    public int Id { get; set; }
    public int EnrollmentId { get; set; }
    public int LessonId { get; set; }
    public bool IsCompleted { get; set; }
    public DateTime? CompletedAt { get; set; }
    public TimeSpan WatchedDuration { get; set; }
    public double WatchedPercentage { get; set; }
    public DateTime? LastWatchedAt { get; set; }
    public string? Notes { get; set; }
    public Lesson? Lesson { get; set; }
}
