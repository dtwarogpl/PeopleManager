using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using PeopleManager.Domain.Services;
using PeopleManager.Infrastructure.Options;
using PeopleManager.Infrastructure.Services.Serialization;
using PeopleManager.Infrastructure.Services.TextHandling;
using static PeopleManager.Infrastructure.XmlDataRepository;

namespace PeopleManager.Infrastructure.Helpers;

public static class Registration
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration Configuration)
    {
        services.Configure<XmlDataFileOptions>(Configuration.GetSection(nameof(XmlDataFileOptions)));
        services.AddTransient<IPeopleRepository, XmlDataRepository>();
        services.AddTransient<IAsyncDataFileHandler, FileStreamHandler>();
        services.AddTransient<IDataSerializer<DataStorageModel>, XmlDataSerializer<DataStorageModel>>();
     
        return services;
    }
}