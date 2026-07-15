namespace Domain.Entities;

public class HomeContent
{
    public required string Greeting { get; init; }
    public required string Name { get; init; }
    public required string Title { get; init; }
    public required string Bio { get; init; }
    public required string CtaText { get; init; }
}