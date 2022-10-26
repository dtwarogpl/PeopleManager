using Microsoft.Extensions.DependencyInjection;
using PeopleManager.ViewModel.Abstractions;
using System;

namespace PeopleManager.ViewModel.Helpers;

public static class Registration
{
    public static IServiceCollection AddViewModel(this IServiceCollection services)
    {
        services.AddSingleton<IViewModel,ApplicationViewModel>();
        services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
        return services;
    }
}