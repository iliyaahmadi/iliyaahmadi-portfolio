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
        var fileName = $"home.{culture}.json";
        var path = Path.Combine(AppContext.BaseDirectory, "Content", "Data", fileName);

        if (!File.Exists(path))
        {
            // fallback to English if a requested culture file doesn't exist yet
            path = Path.Combine(AppContext.BaseDirectory, "Content", "Data", "home.en.json");
        }

        await using var stream = File.OpenRead(path);
        var content = await JsonSerializer.DeserializeAsync<HomeContent>(stream, Options);

        return content ?? throw new InvalidOperationException($"Could not load {fileName}");
    }
}