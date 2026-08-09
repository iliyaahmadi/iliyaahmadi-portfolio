using System.Text.Json;
using Application.Experience;
using Domain.Entities;

namespace Infrastructure.Content;

public class JsonExperienceSource : IExperienceSource
{
    private static readonly JsonSerializerOptions Options = new()
    {
        PropertyNameCaseInsensitive = true
    };

    public async Task<List<ExperienceEntry>> GetAllAsync(string culture)
    {
        var fileName = $"work-history.{culture}.json";
        var path = Path.Combine(AppContext.BaseDirectory, "Content", "Data", fileName);

        if (!File.Exists(path))
            path = Path.Combine(AppContext.BaseDirectory, "Content", "Data", "work-history.en.json");

        await using var stream = File.OpenRead(path);
        var section = await JsonSerializer.DeserializeAsync<ExperienceSectionContent>(stream, Options);

        return section?.Entries ?? [];
    }
}
