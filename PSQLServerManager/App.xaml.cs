using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using PSQLServerManager.Service;
using PSQLServerManager.Service.Interfaces;
using PSQLServerManager.Windows;
using System.Windows;
using BackgroundService = PSQLServerManager.Service.BackgroundService;

namespace PSQLServerManager
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : System.Windows.Application
    {
        public static IHost AppHost { get; private set; }

        public App()
        {
            AppHost = Host.CreateDefaultBuilder()
                .ConfigureServices((hostContext, services) =>
                {
                    //windows
                    services.AddSingleton<ServerHub>();
                    services.AddSingleton<ClosePrompt>();
                    services.AddSingleton<OpenPrompt>();

                    //services
                    services.AddTransient<IBackgroundService, BackgroundService>();
                    services.AddTransient<ICommandRunnerService, CommandRunnerService>();
                })
                .Build();
        }

        private async void Application_Startup(object sender, StartupEventArgs startupEvent)
        {
            await AppHost.StartAsync();
            var startupForm = AppHost.Services.GetRequiredService<ServerHub>();
            startupForm.Show();
        }

        private async void Application_Exit(object sender, ExitEventArgs e)
        {
            await AppHost.StopAsync();
        }
    }
}
