using Domain.Entities;
using MediatR;

namespace Application.Experience.Queries.GetAllExperience;

public record GetAllExperienceQuery(string Culture = "en") : IRequest<List<ExperienceEntry>>;