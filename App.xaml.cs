using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using Activator.Views;
using Activator.ViewModels;
using Activator.ServiceInterfaces;
using Activator.Services;
using Microsoft.Extensions.DependencyInjection;

namespace Activator
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        public static IServiceProvider? ServiceProvider { get; private set; }

        private void ConfigureServices(IServiceCollection services)
        {
            // 注册服务和 ViewModel
            services.AddSingleton<IShowInformationMessageBoxService, ShowInformationMessageBoxService>();
            services.AddSingleton<IShowErrorMessageBoxService, ShowErrorMessageBoxService>();
            //services.AddTransient<MainWindowViewModel>();
            services.AddTransient<OnlineActivationViewModel>();

            // 注册 View
            //services.AddTransient<MainWindow>();
            services.AddTransient<OnlineActivationPage>();
        }

        protected override void OnStartup(StartupEventArgs e)
        {
            var services = new ServiceCollection();
            ConfigureServices(services);
            ServiceProvider = services.BuildServiceProvider();

            //var mainWindow = ServiceProvider.GetRequiredService<MainWindow>();
            //mainWindow.DataContext = ServiceProvider.GetRequiredService<MainWindowViewModel>();
            //mainWindow.Show();

            base.OnStartup(e);
        }
    }
}
