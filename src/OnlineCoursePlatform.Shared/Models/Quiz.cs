namespace OnlineCoursePlatform.Shared.Models;

public class Quiz
{
    public int Id { get; set; }
    public int LessonId { get; set; }
    public string Title { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public int PassingScore { get; set; } = 70; // Percentage
    public int TimeLimit { get; set; } // In minutes, 0 = no limit
    public bool AllowRetake { get; set; } = true;
    public int MaxAttempts { get; set; } = 3; // 0 = unlimited
    public List<QuizQuestion> Questions { get; set; } = new();
}

public class QuizQuestion
{
    public int Id { get; set; }
    public int QuizId { get; set; }
    public string Question { get; set; } = string.Empty;
    public QuestionType Type { get; set; } = QuestionType.SingleChoice;
    public List<QuizOption> Options { get; set; } = new();
    public string? Explanation { get; set; }
    public int Points { get; set; } = 1;
    public int Order { get; set; }
}

public enum QuestionType
{
    SingleChoice,
    MultipleChoice,
    TrueFalse,
    ShortAnswer
}

public class QuizOption
{
    public int Id { get; set; }
    public int QuestionId { get; set; }
    public string Text { get; set; } = string.Empty;
    public bool IsCorrect { get; set; }
}

public class QuizAttempt
{
    public int Id { get; set; }
    public int UserId { get; set; }
    public int QuizId { get; set; }
    public DateTime StartedAt { get; set; } = DateTime.UtcNow;
    public DateTime? CompletedAt { get; set; }
    public int Score { get; set; }
    public int TotalPoints { get; set; }
    public double Percentage => TotalPoints > 0 ? (double)Score / TotalPoints * 100 : 0;
    public bool IsPassed { get; set; }
    public List<QuizAnswer> Answers { get; set; } = new();
}

public class QuizAnswer
{
    public int Id { get; set; }
    public int AttemptId { get; set; }
    public int QuestionId { get; set; }
    public List<int> SelectedOptionIds { get; set; } = new();
    public string? TextAnswer { get; set; }
    public bool IsCorrect { get; set; }
    public int PointsEarned { get; set; }
}
