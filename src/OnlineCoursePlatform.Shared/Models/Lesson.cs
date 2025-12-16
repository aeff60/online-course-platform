namespace OnlineCoursePlatform.Shared.Models;

public class Lesson
{
    public int Id { get; set; }
    public int CourseId { get; set; }
    public string Title { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public string VideoUrl { get; set; } = string.Empty;
    public TimeSpan Duration { get; set; }
    public int Order { get; set; }
    public LessonType Type { get; set; } = LessonType.Video;
    public string Content { get; set; } = string.Empty; // For text/article lessons
    public bool IsFreePreview { get; set; }
    public List<Resource> Resources { get; set; } = new();
    public Quiz? Quiz { get; set; }
}

public enum LessonType
{
    Video,
    Article,
    Quiz,
    Assignment
}

public class Resource
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Url { get; set; } = string.Empty;
    public string Type { get; set; } = string.Empty; // pdf, zip, link
    public long FileSize { get; set; }
}
