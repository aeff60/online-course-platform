namespace OnlineCoursePlatform.Shared.DTOs;

public class QuizDto
{
    public int Id { get; set; }
    public int LessonId { get; set; }
    public string Title { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public int PassingScore { get; set; }
    public int TimeLimit { get; set; }
    public bool AllowRetake { get; set; }
    public int MaxAttempts { get; set; }
    public int QuestionCount { get; set; }
    public int TotalPoints { get; set; }
}

public class QuizDetailDto : QuizDto
{
    public List<QuizQuestionDto> Questions { get; set; } = new();
}

public class QuizQuestionDto
{
    public int Id { get; set; }
    public string Question { get; set; } = string.Empty;
    public string Type { get; set; } = string.Empty;
    public List<QuizOptionDto> Options { get; set; } = new();
    public int Points { get; set; }
    public int Order { get; set; }
}

public class QuizOptionDto
{
    public int Id { get; set; }
    public string Text { get; set; } = string.Empty;
}

public class SubmitQuizDto
{
    public int QuizId { get; set; }
    public List<QuizAnswerSubmitDto> Answers { get; set; } = new();
}

public class QuizAnswerSubmitDto
{
    public int QuestionId { get; set; }
    public List<int> SelectedOptionIds { get; set; } = new();
    public string? TextAnswer { get; set; }
}

public class QuizResultDto
{
    public int AttemptId { get; set; }
    public int Score { get; set; }
    public int TotalPoints { get; set; }
    public double Percentage { get; set; }
    public bool IsPassed { get; set; }
    public int PassingScore { get; set; }
    public DateTime CompletedAt { get; set; }
    public List<QuizAnswerResultDto> AnswerResults { get; set; } = new();
}

public class QuizAnswerResultDto
{
    public int QuestionId { get; set; }
    public string Question { get; set; } = string.Empty;
    public bool IsCorrect { get; set; }
    public int PointsEarned { get; set; }
    public int MaxPoints { get; set; }
    public string? Explanation { get; set; }
    public List<int> SelectedOptionIds { get; set; } = new();
    public List<int> CorrectOptionIds { get; set; } = new();
}
