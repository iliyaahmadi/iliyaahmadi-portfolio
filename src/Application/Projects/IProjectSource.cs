using Domain.Entities;

namespace Application.Projects;

public interface IProjectSource
{
    Task<List<Project>> GetAllAsync();
}
