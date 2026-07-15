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
        var fileName = $"experience.{culture}.json";
        var path = Path.Combine(AppContext.BaseDirectory, "Content", "Data", fileName);

        if (!File.Exists(path))
            path = Path.Combine(AppContext.BaseDirectory, "Content", "Data", "experience.en.json");

        await using var stream = File.OpenRead(path);
        var entries = await JsonSerializer.DeserializeAsync<List<ExperienceEntry>>(stream, Options);

        return entries ?? [];
    }
}