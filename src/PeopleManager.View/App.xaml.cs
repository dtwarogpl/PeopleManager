using System;
using System.IO;
using System.Windows;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using PeopleManager.Infrastructure.Helpers;
using PeopleManager.Infrastructure.Options;
using PeopleManager.ViewModel.Helpers;

namespace PeopleManager.View
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : System.Windows.Application
    {
        public IServiceProvider? ServiceProvider { get; private set; }

        public IConfiguration? Configuration { get; private set; }

        protected override void OnStartup(StartupEventArgs e)
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);

            Configuration = builder.Build();

            var serviceCollection = new ServiceCollection();
            ConfigureServices(serviceCollection);

            ServiceProvider = serviceCollection.BuildServiceProvider();

            var mainWindow = ServiceProvider.GetRequiredService<MainWindow>();
            mainWindow.Show(); 
        }

        private void ConfigureServices(IServiceCollection services)
        {
            services.Configure<XmlDataFileOptions>(Configuration!.GetSection(nameof(XmlDataFileOptions)));
            services.AddTransient(typeof(MainWindow));
            services.AddInfrastructure(Configuration!);
            services.AddViewModel();

        }
    }
}
