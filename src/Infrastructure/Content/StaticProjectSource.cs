using Application.Projects;
using Domain.Entities;

namespace Infrastructure.Content;

public class StaticProjectSource : IProjectSource
{
    public Task<List<Project>> GetAllAsync()
    {
        var projects = new List<Project>
        {
            new()
            {
                Title = "Mohajer",
                Description = "AI-powered web app guiding Iranians through emigration decisions.",
                TechStack = ["ASP.NET Core", "Clean Architecture", "CQRS", "Claude API", "React"],
                GitHubUrl = "https://github.com/iliyaahmadi/Mohajer"
            },
            // other real projects here
        };

        return Task.FromResult(projects);
    }
}