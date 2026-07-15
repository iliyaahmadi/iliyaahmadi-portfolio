using Domain.Entities;

namespace Application.Home;

public interface IHomeContentSource
{
    Task<HomeContent> GetAsync(string culture);
}