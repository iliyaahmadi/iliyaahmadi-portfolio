using Domain.Entities;
using MediatR;

namespace Application.Home.Queries.GetHomeContent;

public class GetHomeContentHandler : IRequestHandler<GetHomeContentQuery, HomeContent>
{
    private readonly IHomeContentSource _homeContentSource;

    public GetHomeContentHandler(IHomeContentSource homeContentSource)
    {
        _homeContentSource = homeContentSource;
    }

    public async Task<HomeContent> Handle(GetHomeContentQuery request, CancellationToken cancellationToken)
    {
        return await _homeContentSource.GetAsync(request.Culture);
    }
}