using Domain.Entities;
using MediatR;

namespace Application.Experience.Queries.GetAllExperience;

public class GetAllExperienceHandler : IRequestHandler<GetAllExperienceQuery, List<ExperienceEntry>>
{
    private readonly IExperienceSource _source;

    public GetAllExperienceHandler(IExperienceSource source) => _source = source;

    public async Task<List<ExperienceEntry>> Handle(GetAllExperienceQuery request, CancellationToken ct)
        => await _source.GetAllAsync(request.Culture);
}