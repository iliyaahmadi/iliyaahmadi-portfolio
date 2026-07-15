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
        var fileName = $"projects.{culture}.json";
        var path = Path.Combine(AppContext.BaseDirectory, "Content", "Data", fileName);

        if (!File.Exists(path))
            path = Path.Combine(AppContext.BaseDirectory, "Content", "Data", "projects.en.json");

        await using var stream = File.OpenRead(path);
        var projects = await JsonSerializer.DeserializeAsync<List<Project>>(stream, Options);

        return projects ?? [];
    }
}