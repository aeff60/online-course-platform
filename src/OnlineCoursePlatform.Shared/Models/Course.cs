namespace OnlineCoursePlatform.Shared.Models;

public class Course
{
    public int Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public string ShortDescription { get; set; } = string.Empty;
    public string ImageUrl { get; set; } = string.Empty;
    public string InstructorName { get; set; } = string.Empty;
    public string InstructorAvatar { get; set; } = string.Empty;
    public decimal Price { get; set; }
    public bool IsFree => Price == 0;
    public string Category { get; set; } = string.Empty;
    public string Level { get; set; } = "Beginner"; // Beginner, Intermediate, Advanced
    public double Rating { get; set; }
    public int RatingCount { get; set; }
    public int EnrollmentCount { get; set; }
    public int TotalLessons { get; set; }
    public TimeSpan TotalDuration { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
    public bool IsPublished { get; set; }
    public List<string> Tags { get; set; } = new();
    public List<string> Requirements { get; set; } = new();
    public List<string> WhatYouWillLearn { get; set; } = new();
    public List<Lesson> Lessons { get; set; } = new();
}
