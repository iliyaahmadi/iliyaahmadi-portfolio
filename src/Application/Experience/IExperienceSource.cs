using Domain.Entities;

namespace Application.Experience;

public interface IExperienceSource
{
    Task<List<ExperienceEntry>> GetAllAsync(string culture);
}