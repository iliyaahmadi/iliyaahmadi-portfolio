using System.Text.Json;
using Application.Home;
using Domain.Entities;

namespace Infrastructure.Content;

public class JsonHomeContentSource : IHomeContentSource
{
    private static readonly JsonSerializerOptions Options = new()
    {
        PropertyNameCaseInsensitive = true
    };

    public async Task<HomeContent> GetAsync(string culture)
    {
        return new HomeContent
        {
            Hero = await ReadSectionAsync<HeroContent>("hero", culture),
            Work = await ReadSectionAsync<WorkSectionContent>("selected-work", culture),
            Experience = await ReadSectionAsync<ExperienceSectionContent>("work-history", culture),
            Skills = await ReadSectionAsync<SkillsSectionContent>("tools-and-expertise", culture),
            Contact = await ReadSectionAsync<ContactContent>("contact", culture)
        };
    }

    private static async Task<T> ReadSectionAsync<T>(string section, string culture)
    {
        var fileName = $"{section}.{culture}.json";
        var path = Path.Combine(AppContext.BaseDirectory, "Content", "Data", fileName);

        if (!File.Exists(path))
        {
            fileName = $"{section}.en.json";
            path = Path.Combine(AppContext.BaseDirectory, "Content", "Data", fileName);
        }

        await using var stream = File.OpenRead(path);
        var content = await JsonSerializer.DeserializeAsync<T>(stream, Options);

        return content ?? throw new InvalidOperationException($"Could not load {fileName}");
    }
}
