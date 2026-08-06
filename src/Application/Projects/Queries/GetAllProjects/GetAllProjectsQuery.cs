using Domain.Entities;
using MediatR;

namespace Application.Projects.Queries.GetAllProjects;

public record GetAllProjectsQuery(string Culture = "en") : IRequest<List<Project>>;