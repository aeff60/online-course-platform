namespace OnlineCoursePlatform.Shared.DTOs;

public class LessonDto
{
    public int Id { get; set; }
    public int CourseId { get; set; }
    public string Title { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public string Duration { get; set; } = string.Empty;
    public int Order { get; set; }
    public string Type { get; set; } = string.Empty;
    public bool IsFreePreview { get; set; }
    public bool IsCompleted { get; set; }
    public List<ResourceDto> Resources { get; set; } = new();
    public bool HasQuiz { get; set; }
}

public class LessonDetailDto : LessonDto
{
    public string VideoUrl { get; set; } = string.Empty;
    public string Content { get; set; } = string.Empty;
    public QuizDto? Quiz { get; set; }
}

public class CreateLessonDto
{
    public int CourseId { get; set; }
    public string Title { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public string VideoUrl { get; set; } = string.Empty;
    public int DurationMinutes { get; set; }
    public int Order { get; set; }
    public string Type { get; set; } = "Video";
    public string Content { get; set; } = string.Empty;
    public bool IsFreePreview { get; set; }
}

public class ResourceDto
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Url { get; set; } = string.Empty;
    public string Type { get; set; } = string.Empty;
    public string FileSize { get; set; } = string.Empty;
}
