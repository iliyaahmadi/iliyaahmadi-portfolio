namespace Domain.Entities;

public class HomeContent
{
    public required HeroContent Hero { get; init; }
    public required WorkSectionContent Work { get; init; }
    public required ExperienceSectionContent Experience { get; init; }
    public required SkillsSectionContent Skills { get; init; }
    public required ContactContent Contact { get; init; }
}

public class HeroContent
{
    public required string PageTitle { get; init; }
    public required string MetaDescription { get; init; }
    public required string SiteName { get; init; }
    public required NavigationContent Navigation { get; init; }
    public required string Greeting { get; init; }
    public required string Name { get; init; }
    public required string Title { get; init; }
    public required string Bio { get; init; }
    public required List<HeroDetail> Details { get; init; }
    public required List<HeroAction> Actions { get; init; }
}

public class HeroDetail
{
    public required string Text { get; init; }
    public string Icon { get; init; } = "status";
}

public class HeroAction
{
    public required string Key { get; init; }
    public required string Text { get; init; }
    public required string Url { get; init; }
    public string Style { get; init; } = "link";
    public bool OpenInNewTab { get; init; }
}

public class WorkSectionContent
{
    public required string Title { get; init; }
    public required string LiveDemo { get; init; }
    public required string CompanionService { get; init; }
    public required string ApiSource { get; init; }
    public required List<Project> Projects { get; init; }
}

public class ExperienceSectionContent
{
    public required string Title { get; init; }
    public required List<InfoCard> InfoCards { get; init; }
    public required List<ExperienceEntry> Entries { get; init; }
}

public class InfoCard
{
    public required string Label { get; init; }
    public string? Heading { get; init; }
    public required List<InfoCardItem> Items { get; init; }
}

public class InfoCardItem
{
    public required string Title { get; init; }
    public required string Detail { get; init; }
}

public class SkillGroup
{
    public required string Title { get; init; }
    public required List<SkillItem> Items { get; init; }
}

public class SkillsSectionContent
{
    public required string Title { get; init; }
    public required List<SkillGroup> Groups { get; init; }
}

public class SkillItem
{
    public required string Name { get; init; }
    public required string LogoUrl { get; init; }
}

public class ContactContent
{
    public required string Availability { get; init; }
    public required string Heading { get; init; }
    public required string Description { get; init; }
    public required string EmailLabel { get; init; }
    public required string Email { get; init; }
    public required List<ContactLink> Links { get; init; }
    public required string SuccessMessage { get; init; }
    public required string FailureMessage { get; init; }
    public required string RateLimitMessage { get; init; }
    public required string NameLabel { get; init; }
    public required string EmailFieldLabel { get; init; }
    public required string MessageLabel { get; init; }
    public required string SubmitText { get; init; }
}

public class ContactLink
{
    public required string Label { get; init; }
    public required string Url { get; init; }
}

public class NavigationContent
{
    public required string Work { get; init; }
    public required string Experience { get; init; }
    public required string Skills { get; init; }
    public required string Contact { get; init; }
    public required string Lab { get; init; }
    public required string OpenLab { get; init; }
}
