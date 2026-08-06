using Application.Experience;
using Application.Home;
using Application.Projects;
using Infrastructure.Content;
using Microsoft.Extensions.DependencyInjection;
using Application.Contact;
using Infrastructure.Email;

namespace Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services)
    {
        services.AddScoped<IProjectSource, JsonProjectSource>();
        services.AddScoped<IHomeContentSource, JsonHomeContentSource>();
        services.AddScoped<IExperienceSource, JsonExperienceSource>();
        services.AddScoped<IEmailSender, SmtpEmailSender>();
        return services;
    }
}