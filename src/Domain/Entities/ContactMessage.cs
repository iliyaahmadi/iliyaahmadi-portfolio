namespace Domain.Entities;

public class ContactMessage
{
    public required string Name { get; init; }
    public required string Email { get; init; }
    public required string Message { get; init; }
    public DateTime SentAtUtc { get; init; } = DateTime.UtcNow;
}