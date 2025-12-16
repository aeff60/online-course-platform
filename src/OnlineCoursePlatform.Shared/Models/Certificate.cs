namespace OnlineCoursePlatform.Shared.Models;

public class Certificate
{
    public int Id { get; set; }
    public int UserId { get; set; }
    public int CourseId { get; set; }
    public int EnrollmentId { get; set; }
    public string CertificateNumber { get; set; } = string.Empty;
    public DateTime IssuedAt { get; set; } = DateTime.UtcNow;
    public string? VerificationUrl { get; set; }
    public User? User { get; set; }
    public Course? Course { get; set; }

    public static string GenerateCertificateNumber()
    {
        return $"CERT-{DateTime.UtcNow:yyyyMMdd}-{Guid.NewGuid().ToString()[..8].ToUpper()}";
    }
}
