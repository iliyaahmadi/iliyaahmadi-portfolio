using System.Text.Json;
using Application.Projects;
using Domain.Entities;

namespace Infrastructure.Content;

public class JsonProjectSource : IProjectSource
{
    private static readonly JsonSerializerOptions Options = new()
    {
        PropertyNameCaseInsensitive = true
    };

    public async Task<List<Project>> GetAllAsync(string culture)
    {
        var fileName = $"selected-work.{culture}.json";
        var path = Path.Combine(AppContext.BaseDirectory, "Content", "Data", fileName);

        if (!File.Exists(path))
            path = Path.Combine(AppContext.BaseDirectory, "Content", "Data", "selected-work.en.json");

        await using var stream = File.OpenRead(path);
        var section = await JsonSerializer.DeserializeAsync<WorkSectionContent>(stream, Options);

        return section?.Projects ?? [];
    }
}
