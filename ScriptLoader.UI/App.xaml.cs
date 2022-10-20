using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using ScriptLoader.Core.FileService;
using ScriptLoader.Core.ScriptLoaders;
using ScriptLoader.UI.Factories;
using ScriptLoader.UI.ViewModels;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Windows;

namespace ScriptLoader.UI
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        private ServiceProvider serviceProvider;
        IConfigurationRoot configuration;
        public App()
        {
            var builder = new ConfigurationBuilder()
      .SetBasePath(Directory.GetCurrentDirectory())
      .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);

            configuration = builder.Build();
            ServiceCollection services = new ServiceCollection();
            ConfigureServices(services);
            serviceProvider = services.BuildServiceProvider();
        }

        private void ConfigureServices(ServiceCollection services)
        {
            var path = string.IsNullOrEmpty(configuration["ScriptsDirectoryPath"]) ? Directory.GetCurrentDirectory() :
                configuration["ScriptsDirectoryPath"];
            services.AddTransient<IFileService,DiskFileService>(_=> new DiskFileService(path));
            var httpBuilder = services.AddHttpClient(Microsoft.Extensions.Options.Options.DefaultName, client =>
            {
                client.DefaultRequestHeaders.Add("User-Agent", configuration.GetSection("HttpClientSettings")["UserAgent"]);
            });
            var proxySection = configuration.GetSection("ProxySettings");
            if (!string.IsNullOrEmpty(proxySection["Address"]) && int.TryParse(proxySection["Port"], out var port))
            {
                httpBuilder.ConfigureHttpMessageHandlerBuilder(h => new SocketsHttpHandler
                {
                    Proxy = new WebProxy(proxySection["Address"], port)
                    {
                        Credentials = new NetworkCredential(proxySection["login"], proxySection["password"])
                    }
                });
            }
            services.AddTransient<ScriptLoaderViewModel>();
            services.AddTransient<IScriptLoader, HttpScriptLoader>();
            services.AddSingleton<MainWindow>();
            services.AddTransient<IScriptViewModelFactory, ScriptViewModelFactory>();
        }
        private void OnStartup(object sender, StartupEventArgs e)
        {
            var mainWindow = serviceProvider.GetService<MainWindow>();
            mainWindow!.Show();
        }
    }
}
