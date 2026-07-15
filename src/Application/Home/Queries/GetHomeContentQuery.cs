using Domain.Entities;
using MediatR;

namespace Application.Home.Queries.GetHomeContent;

public record GetHomeContentQuery(string Culture = "en") : IRequest<HomeContent>;