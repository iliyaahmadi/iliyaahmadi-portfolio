namespace Domain.Entities;

public class Project
{
    public required string Title { get; init; }
    public required string Description { get; init; }
    public required List<string> TechStack { get; init; }
    public string? GitHubUrl { get; init; }
    public string? LiveDemoUrl { get; init; }
    public string? ImageUrl { get; init; }
    public string? Status { get; init; }  
}