using Microsoft.Extensions.DependencyInjection;
using PeopleManager.Domain.Services;

namespace PeopleManager.Infrastructure.Helpers;

public static class Registration
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services)
    {
        services.AddTransient<IPeopleRepository, XmlDataRepository>();
        return services;
    }
}