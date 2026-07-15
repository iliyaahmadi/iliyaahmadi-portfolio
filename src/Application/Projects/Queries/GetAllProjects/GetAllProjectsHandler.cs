using Domain.Entities;
using MediatR;

namespace Application.Projects.Queries.GetAllProjects;

public class GetAllProjectsHandler : IRequestHandler<GetAllProjectsQuery, List<Project>>
{
    private readonly IProjectSource _projectSource;

    public GetAllProjectsHandler(IProjectSource projectSource)
    {
        _projectSource = projectSource;
    }

    public async Task<List<Project>> Handle(GetAllProjectsQuery request, CancellationToken cancellationToken)
    {
        return await _projectSource.GetAllAsync();
    }
}