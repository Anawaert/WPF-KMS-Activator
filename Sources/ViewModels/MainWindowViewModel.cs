using System;
using System.IO;
using System.Threading.Tasks;
using System.Windows;
using System.Xml.Serialization;
using Activator.Views;
using Activator.Models;
using Activator.ServiceInterfaces;
using CommunityToolkit.Mvvm;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.Extensions.DependencyInjection;

namespace Activator.ViewModels
{
    /// <summary>
    /// <para><see cref="MainWindow"/> 的 ViewModel</para>
    /// <para>ViewModel for <see cref="MainWindow"/></para>
    /// </summary>
    public partial class MainWindowViewModel : ViewModelBase
    {
        // 用于弹出 Information 类型的 MessageBox 服务
        // Service for showing Information type MessageBox
        private readonly IShowInformationMessageBoxService _showInformationMessageBoxService;

        // 用于弹出 Error 类型的 MessageBox 服务
        // Service for showing Error type MessageBox
        private readonly IShowErrorMessageBoxService _showErrorMessageBoxService;

        /// <summary>
        /// <para>读取所有必要的系统信息</para>
        /// <para>Read all required system information</para>
        /// </summary>
        /// <returns>
        /// <para><see cref="Task"/> 类型，表示异步操作</para>
        /// <para><see cref="Task"/> type, represents an asynchronous operation</para>
        /// </returns>
        [RelayCommand]
        private async Task ReadAllRequiredInfo()
        {
            // 使用 Task.Run() 来异步执行初始化操作
            // Use Task.Run() to execute initialization operation asynchronously
            await Task.Run
            (
                () =>
                {
                    Shared.InitializeSharedInfo();
                    WindowsInfo.InitializeWindowsInfo();
                    OfficeInfo.InitializeOfficeInfo();
                }
            );
        }

        /// <summary>
        /// <para>用于检查版本更新</para>
        /// <para>Used to check for updates</para>
        /// </summary>
        /// <returns>
        /// <para><see cref="Task"/> 类型，表示异步操作</para>
        /// <para><see cref="Task"/> type, represents an asynchronous operation</para>
        /// </returns>
        [RelayCommand]
        private async Task UpdateCheck()
        {
            // 由于已经读取了配置存于 Model 中，因此先从 Model 中读取配置来决定是否检查更新
            // Since the configuration has been read and stored in the Model, read the configuration from the Model first to determine whether to check for updates
            if (AllSettingsModel.Instance.IsUpdateCheckEnabled)
            {
                bool retVal = await Utility.CheckUpdateAvailable();
                if (retVal)
                {
                    /* 暂时仅提示更新，而不实现先前版本的“跳转至发行页”功能 */
                    /* Currently only prompt for updates, and do not implement the "jump to release page" function of the previous version */
                    _showInformationMessageBoxService.ShowMessageBox
                    (
                        "已有新版本的 Anawaert KMS Activator 发布，可从程序首页访问程序仓库的发布页以获取更新",
                        "提示"
                    );
                }
            }
        }

        /// <summary>
        /// <para>用于保存所有设置</para>
        /// <para>Used to save all settings</para>
        /// </summary>
        /// <returns>
        /// <para><see cref="Task"/> 类型，表示异步操作</para>
        /// <para><see cref="Task"/> type, represents an asynchronous operation</para>
        /// </returns>
        [RelayCommand]
        private async Task SaveAllSettings()
        {
            await ModelOperation.SaveConfigAsync(AllSettingsModel.Instance, Shared.JsonFilePath ??
                                                 Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) +
                                                 "\\Anawaert KMS Activator\\Activator Configuration.json");
        }

        /// <summary>
        /// <para>主窗口 ViewModel 主构造函数，通过 IoC 传递两个用于展示 MessageBox 的函数参数</para>
        /// <para>Main constructor of MainWindow ViewModel, passing two functions for showing MessageBox through IoC</para>
        /// </summary>
        /// <param name="showInformationMessageBoxService">
        /// <para>用于展示 Information 类型 MessageBox 的函数</para>
        /// <para>Function for showing Information type MessageBox</para>
        /// </param>
        /// <param name="showErrorMessageBoxService">
        /// <para>用于展示 Error 类型 MessageBox 的函数</para>
        /// <para>Function for showing Error type MessageBox</para>
        /// </param>
        public MainWindowViewModel(IShowInformationMessageBoxService showInformationMessageBoxService, IShowErrorMessageBoxService showErrorMessageBoxService)
        {
            _showErrorMessageBoxService = showErrorMessageBoxService;
            _showInformationMessageBoxService = showInformationMessageBoxService;
        }

        /// <summary>
        /// <para>主窗口 ViewModel 的无参构造函数，用于构造设计时实例</para>
        /// <para>Parameterless constructor of MainWindow ViewModel, used to construct design-time instance</para>
        /// </summary>
        public MainWindowViewModel()
        {
            // 由于服务一定存在且被注册，因此直接使用 App.ServiceProvider 获取服务
            // Since the service must exist and be registered, use App.ServiceProvider to get the service directly
            _showInformationMessageBoxService = App.ServiceProvider!.GetRequiredService<IShowInformationMessageBoxService>();
            _showErrorMessageBoxService = App.ServiceProvider!.GetRequiredService<IShowErrorMessageBoxService>();
        }
    }
}
