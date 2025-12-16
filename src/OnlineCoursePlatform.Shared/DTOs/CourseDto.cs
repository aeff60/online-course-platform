namespace OnlineCoursePlatform.Shared.DTOs;

public class CourseDto
{
    public int Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public string ShortDescription { get; set; } = string.Empty;
    public string ImageUrl { get; set; } = string.Empty;
    public string InstructorName { get; set; } = string.Empty;
    public decimal Price { get; set; }
    public bool IsFree => Price == 0;
    public string Category { get; set; } = string.Empty;
    public string Level { get; set; } = string.Empty;
    public double Rating { get; set; }
    public int RatingCount { get; set; }
    public int EnrollmentCount { get; set; }
    public int TotalLessons { get; set; }
    public string TotalDuration { get; set; } = string.Empty;
    public List<string> Tags { get; set; } = new();
}

public class CourseDetailDto : CourseDto
{
    public string Description { get; set; } = string.Empty;
    public string InstructorAvatar { get; set; } = string.Empty;
    public List<string> Requirements { get; set; } = new();
    public List<string> WhatYouWillLearn { get; set; } = new();
    public List<LessonDto> Lessons { get; set; } = new();
    public DateTime UpdatedAt { get; set; }
}

public class CreateCourseDto
{
    public string Title { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public string ShortDescription { get; set; } = string.Empty;
    public string ImageUrl { get; set; } = string.Empty;
    public decimal Price { get; set; }
    public string Category { get; set; } = string.Empty;
    public string Level { get; set; } = "Beginner";
    public List<string> Tags { get; set; } = new();
    public List<string> Requirements { get; set; } = new();
    public List<string> WhatYouWillLearn { get; set; } = new();
}

public class UpdateCourseDto : CreateCourseDto
{
    public int Id { get; set; }
    public bool IsPublished { get; set; }
}
