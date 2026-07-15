namespace Domain.Entities;

public class ExperienceEntry
{
    public required string Company { get; init; }
    public required string Role { get; init; }
    public string? Team { get; init; }              // null if no sub-team split
    public required string DateRange { get; init; }
    public required List<string> Highlights { get; init; }
    public string? LogoUrl { get; init; }
    public required string GroupKey { get; init; }   // same value for entries under the same company, for the connecting line
}