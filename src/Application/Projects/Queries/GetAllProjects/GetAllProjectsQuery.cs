using Domain.Entities;
using MediatR;

namespace Application.Projects.Queries.GetAllProjects;

public record GetAllProjectsQuery : IRequest<List<Project>>;