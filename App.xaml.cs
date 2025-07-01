using System;
using System.Windows;
using Serilog;
using Microsoft.Extensions.DependencyInjection;
using Activator.Models;
using Activator.Views;
using Activator.ViewModels;
using Activator.ServiceInterfaces;
using Activator.Services;

namespace Activator
{
    /// <summary>
    /// App.xaml 的交互逻辑
    /// <br/>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        /// <summary>
        /// 获取或设置服务提供程序
        /// <br/>
        /// Get or set the service provider
        /// </summary>
        public static IServiceProvider? ServiceProvider { get; private set; }

        /// <summary>
        /// 配置服务
        /// <br/>
        /// Configure services
        /// </summary>
        /// <param name="services">
        /// 服务集合
        /// <br/>
        /// Service collection
        /// </param>
        private void ConfigureServices(IServiceCollection services)
        {
            // 注册服务，主要有 MessageBox 服务，采用单例模式
            // Register services, mainly MessageBox services, using singleton mode
            services.AddSingleton<IShowInformationMessageBoxService, ShowInformationMessageBoxService>();
            services.AddSingleton<IShowErrorMessageBoxService, ShowErrorMessageBoxService>();

            // 注册所有需要的 ViewModel，采用单例模式
            // Register all required ViewModels, using singleton mode
            services.AddSingleton<MainWindowViewModel>();
            services.AddSingleton<OnlineActivationViewModel>();
            services.AddSingleton<ManualOnlineActivationViewModels>();

            // 注册所有需要的 View，采用瞬态模式
            // Register all required Views, using transient mode
            services.AddTransient<MainWindow>();
            services.AddTransient<OnlineActivationPage>();
            services.AddTransient<ManualOnlineActivationPage>();
        }

        /// <summary>
        /// 尝试创建 Activator 的程序保留目录
        /// <br/>
        /// Try to create the program reserved directory of Activator
        /// </summary>
        /// <returns>
        /// 一个 <see cref="bool"/> 值，表示是否创建成功
        /// <br/>
        /// A <see cref="bool"/> value, indicating whether the creation is successful
        /// </returns>
        private bool TryCreateActivatorDirectory()
        {
            try
            {
                Utility.CreateActivatorDirectory();
                return true;
            }
            catch (Exception exception)
            {
                // 若出现错误，即出现如权限不足或无法访问的情况，记录错误日志并返回 false
                // If an error occurs, such as insufficient permissions or inaccessible, record the error log and return false
                Log.Logger.Error(exception, "Failed to create the Activator directory.");
                return false;
            }
        }

        /// <summary>
        /// 设置应用程序主题
        /// <br/>
        /// Set the application theme
        /// </summary>
        private void SetAppTheme()
        {
            // 为了能在程序启动时就设置好相应的主题，与主题相关的操作写在了 App.xaml.cs 中
            // In order to set the corresponding theme when the program starts, the operations related to the theme are written in App.xaml.cs
            Wpf.Ui.Appearance.ApplicationThemeManager.Apply
            (
                AllSettingsModel.Instance.IsLightThemeMode ?
                Wpf.Ui.Appearance.ApplicationTheme.Light : 
                Wpf.Ui.Appearance.ApplicationTheme.Dark
            );
        }

        /// <summary>
        /// 重写启动事件，用于配置服务与程序预处理
        /// <br/>
        /// Override the startup event for configuring services and pre-processing of the program
        /// </summary>
        /// <param name="e">
        /// 启动事件参数，不得随意更改
        /// <br/>
        /// Startup event arguments, should not be changed arbitrarily
        /// </param>
        protected override void OnStartup(StartupEventArgs e)
        {
            // 初始化日志对象
            // Initialize the log object
            var services = new ServiceCollection();
            ConfigureServices(services);
            ServiceProvider = services.BuildServiceProvider();

            // 尝试创建 Activator 的程序保留目录与设置应用程序主题
            // Try to create the program reserved directory of Activator and set the application theme
            TryCreateActivatorDirectory();
            SetAppTheme();

            base.OnStartup(e);
        }
    }
}
