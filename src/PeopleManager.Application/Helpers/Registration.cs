using MediatR;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;
using PeopleManager.Application.Queries;
using PeopleManager.Infrastructure.Helpers;
using System;

namespace PeopleManager.Application.Helpers;

public static class Registration
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        services.AddMediatR(Assembly.GetExecutingAssembly());
        13:1services.AddInfrastructure();
        return services;
    }
}