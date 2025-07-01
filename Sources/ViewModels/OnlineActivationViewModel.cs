using System;
using System.Threading.Tasks;
using System.Text.RegularExpressions;
using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Activator;
using Activator.ServiceInterfaces;
using Activator.Models;
using Microsoft.Extensions.DependencyInjection;


namespace Activator.ViewModels
{
    /// <summary>
    /// <para>在线激活页的 ViewModel</para>
    /// <para>Online activation page ViewModel</para>
    /// </summary>
    public partial class OnlineActivationViewModel : ViewModelBase
    {
        /// <summary>
        /// <para>显示 Information 图标类型的 MessageBox 服务</para>
        /// <para>Service for showing MessageBox with Information button type</para>
        /// </summary>
        private readonly IShowInformationMessageBoxService _showInformationMessageBoxService;

        /// <summary>
        /// <para>显示 Error 图标类型的 MessageBox 服务</para>
        /// <para>Service for showing MessageBox with Error button type</para>
        /// </summary>
        private readonly IShowErrorMessageBoxService _showErrorMessageBoxService;

        /// <summary>
        /// <para>是否为 Windows 激活模式</para>
        /// <para>Whether it is Windows activation mode</para>
        /// </summary>
        [ObservableProperty]
        private bool _isWindowsActivationMode = true;

        /// <summary>
        /// <para>是否为 Office 激活模式</para>
        /// <para>Whether it is Office activation mode</para>
        /// </summary>
        [ObservableProperty]
        private bool _isOfficeActivationMode = false;

        /// <summary>
        /// <para>激活服务器列表，集合内容为 <see langword="string"/> 类型</para>
        /// <para>Activation server list, collection content is of type <see langword="string"/></para>
        /// </summary>
        [ObservableProperty]
        private ObservableCollection<string> _servers = new ObservableCollection<string>()
        {
            "Anawaert 服务器（推荐）",
            "服务器：kms.03k.org",
            "服务器：kms.cgtsoft.com"
        };

        /// <summary>
        /// <para>选定的激活服务器内容（字符串）</para>
        /// <para>Selected activation server content (string)</para>
        /// </summary>
        [ObservableProperty]
        private string? _selectedServerContentString;

        /// <summary>
        /// <para>选定的激活服务器索引</para>
        /// <para>Selected activation server index</para>
        /// </summary>
        [ObservableProperty]
        private int _selectedServerIndex = 0;

        /// <summary>
        /// <para>当选择为 Windows 激活时执行的函数</para>
        /// <para>Function executed when Windows activation is selected</para>
        /// </summary>
        /// <returns>
        /// <para><see cref="Task"/> 类型，表示异步任务</para>
        /// <para><see cref="Task"/> type, representing an asynchronous task</para>
        /// </returns>
        private async Task ActivateOnWindowsMode()
        {
            await Task.Run
            (
                () =>
                {
                    WindowsOnlineActivator windowsOnlineActivator = new WindowsOnlineActivator();

                    // 首先检查 Windows 是否已激活，如果已激活则无需再次激活
                    // First check if Windows is activated, if it is activated, there is no need to activate it again
                    if (WindowsInfo.IsWindowsActivated)
                    {
                        _showInformationMessageBoxService.ShowMessageBox("您的 Windows 已处于激活状态，无需再次激活", "提示");
                        return;
                    }

                    // 检查 Windows 版本是否为不支持的版本，如果是则提示错误
                    // Check if the Windows version is an unsupported version, if it is, an error message is displayed
                    if (WindowsInfo.WindowsProduct == WindowsProductName.Unsupported)
                    {
                        _showErrorMessageBoxService.ShowMessageBox("未找到对应的 Windows 版本，请联系 Anawaert", "错误");
                        return;
                    }

                    if (!string.IsNullOrEmpty(SelectedServerContentString))
                    {
                        // 当选定的服务器内容包含 "Anawaert" 时，使用 Anawaert 服务器地址，否则使用正则表达式匹配的服务器地址
                        // When the selected server content contains "Anawaert", use the Anawaert server address, otherwise use the server address matched by the regular expression
                        string server = SelectedServerContentString.Contains("Anawaert") ?
                                        Shared.AnawaertServerAddress :
                                        Regex.Match(SelectedServerContentString, @"([a-zA-Z0-9]+\.[a-zA-Z0-9]+\.[a-zA-Z0-9]+)").Value;

                        // 转换 Windows 为非评估版本
                        // Convert Windows to non-evaluation version
                        windowsOnlineActivator.ConvertToReleaseVersion(WindowsInfo.WindowsProduct, WindowsInfo.IsReleaseVersion);
                        string? windowsKey = windowsOnlineActivator.GetActivationKey(WindowsInfo.WindowsProduct);

                        // 按照 KMS 激活的流程进行激活
                        // Activate according to the KMS activation process
                        if (string.IsNullOrEmpty(windowsKey))
                        {
                            _showErrorMessageBoxService.ShowMessageBox("未找到对应的 Windows 批量激活密钥，请联系 Anawaert", "错误");
                            return;
                        }
                        if (!windowsOnlineActivator.SetActivationKey(windowsKey))
                        {
                            _showErrorMessageBoxService.ShowMessageBox("未能正确设置批量激活密钥，请重试或联系 Anawaert", "错误");
                            return;
                        }
                        if (!windowsOnlineActivator.SetKMSServer(server))
                        {
                            _showErrorMessageBoxService.ShowMessageBox("未能正确连接至选定的服务器，请重试或联系 Anawaert", "错误");
                            return;
                        }
                        if (!windowsOnlineActivator.ApplyActivation())
                        {
                            _showErrorMessageBoxService.ShowMessageBox("未能正确执行激活，请重试或联系 Anawaert", "错误");
                            return;
                        }
                        
                        _showInformationMessageBoxService.ShowMessageBox("您的 Windows 已成功激活", "提示");
                    }
                    else
                    {
                        _showErrorMessageBoxService.ShowMessageBox("您未选择任何激活服务器，请重试", "错误");
                    }

                    // 刷新 Windows 信息
                    // Refresh Windows information
                    WindowsInfo.RefreshWindowsInfo();
                }
            );
        }

        /// <summary>
        /// <para>当选择为 Windows 激活时执行的卸载激活函数</para>
        /// <para>Uninstall activation function executed when Windows activation is selected</para>
        /// </summary>
        /// <returns>
        /// <para><see cref="Task"/> 类型，表示异步任务</para>
        /// <para><see cref="Task"/> type, representing an asynchronous task</para>
        /// </returns>
        private async Task DeActivateOnWindowsMode()
        {
            await Task.Run
            (
                () =>
                {
                    WindowsOnlineActivator windowsOnlineActivator = new WindowsOnlineActivator();

                    // 如果 Windows 已激活，则移除激活
                    // If Windows is activated, remove the activation
                    if (WindowsInfo.IsWindowsActivated)
                    {
                        if (windowsOnlineActivator.RemoveActivation())
                        {
                            _showInformationMessageBoxService.ShowMessageBox("您的 Windows 已成功移除激活", "提示");
                        }
                        else
                        {
                            _showErrorMessageBoxService.ShowMessageBox("未能正确移除激活，请重试或联系 Anawaert", "错误");
                        }
                    }
                    else
                    {
                        _showInformationMessageBoxService.ShowMessageBox("您的 Windows 未激活，无需移除激活", "提示");
                    }

                    // 刷新 Windows 信息
                    // Refresh Windows information
                    WindowsInfo.RefreshWindowsInfo();
                }
            );
        }

        // 以下两个函数的实现与上述两个函数类似，不再赘述
        // The implementation of the following two functions is similar to the above two functions, so it is not repeated here
        private async Task ActivateOnOfficeMode()
        {
            await Task.Run
            (
                () =>
                {
                    OfficeOnlineActivator officeOnlineActivator = new OfficeOnlineActivator();

                    if (OfficeInfo.VisioEdition == VisioEditionName.Unsupported || OfficeInfo.OfficeEdition == OfficeEditionName.Unsupported ||
                        string.IsNullOrEmpty(OfficeInfo.OsppDirectory) || string.IsNullOrEmpty(OfficeInfo.LicenseDirectory))
                    {
                        _showErrorMessageBoxService.ShowMessageBox("未找到对应的 Office 或 Visio 版本，请联系 Anawaert", "错误");
                        return;
                    }

                    if (OfficeInfo.IsOfficeActivated)
                    {
                        _showInformationMessageBoxService.ShowMessageBox("您的 Office 已处于激活状态，无需再次激活", "提示");
                        return;
                    }

                    if (!string.IsNullOrEmpty(SelectedServerContentString))
                    {
                        string server = SelectedServerContentString.Contains("Anawaert") ?
                                        Shared.AnawaertServerAddress :
                                        Regex.Match(SelectedServerContentString, @"([a-zA-Z0-9]+\.[a-zA-Z0-9]+\.[a-zA-Z0-9]+)").Value;

                        officeOnlineActivator.ConvertToVolumeLicense(OfficeInfo.LicenseDirectory, OfficeInfo.OsppDirectory, OfficeInfo.OfficeEdition, OfficeInfo.VisioEdition);
                        string? officeKey = officeOnlineActivator.GetOfficeActivationKey(OfficeInfo.OfficeEdition);
                        if (string.IsNullOrEmpty(officeKey))
                        {
                            _showErrorMessageBoxService.ShowMessageBox("未找到对应的 Office 批量激活密钥，请联系 Anawaert", "错误");
                            return;
                        }
                        if (!officeOnlineActivator.SetOfficeActivationKey(officeKey, OfficeInfo.OsppDirectory))
                        {
                            _showErrorMessageBoxService.ShowMessageBox("未能正确设置批量激活密钥，请重试或联系 Anawaert", "错误");
                            return;
                        }
                        if (!officeOnlineActivator.SetKMSServer(server, OfficeInfo.OsppDirectory))
                        {
                            _showErrorMessageBoxService.ShowMessageBox("未能正确连接至选定的服务器，请重试或联系 Anawaert", "错误");
                            return;
                        }
                        if (!officeOnlineActivator.ApplyActivation(OfficeInfo.OsppDirectory))
                        {
                            _showErrorMessageBoxService.ShowMessageBox("未能正确执行激活，请重试或联系 Anawaert", "错误");
                            return;
                        }

                        _showInformationMessageBoxService.ShowMessageBox("您的 Office 已成功激活", "提示");
                    }
                    else
                    {
                        _showErrorMessageBoxService.ShowMessageBox("您未选择任何激活服务器，请重试", "错误");
                    }
                    OfficeInfo.RefreshOfficeInfo();
                }
            );
        }

        private async Task DeActivateOnOfficeMode()
        {
            await Task.Run
            (
                () =>
                {
                    OfficeOnlineActivator officeOnlineActivator = new OfficeOnlineActivator();

                    if (OfficeInfo.IsOfficeActivated)
                    {
                        if (officeOnlineActivator.RemoveActivation(OfficeInfo.OsppDirectory))
                        {
                            _showInformationMessageBoxService.ShowMessageBox("您的 Office 已成功移除激活", "提示");
                        }
                        else
                        {
                            _showErrorMessageBoxService.ShowMessageBox("未能正确移除激活，请重试或联系 Anawaert", "错误");
                        }
                    }
                    else
                    {
                        _showInformationMessageBoxService.ShowMessageBox("您的 Office 未激活，无需移除激活", "提示");
                    }
                    OfficeInfo.RefreshOfficeInfo();
                }
            );
        }

        /// <summary>
        /// <para>执行激活，异步命令</para>
        /// <para>Execute activation, asynchronous command</para>
        /// </summary>
        /// <returns>
        /// <para><see cref="Task"/> 类型，表示异步任务</para>
        /// <para><see cref="Task"/> type, representing an asynchronous task</para>
        /// </returns>
        [RelayCommand]
        private async Task ActivateAsync()
        {
            // 逻辑有待优化
            // Logic needs to be optimized
            if (IsWindowsActivationMode && !IsOfficeActivationMode)
            {
                await ActivateOnWindowsMode();
            }
            else if (!IsWindowsActivationMode && IsOfficeActivationMode)
            {
                await ActivateOnOfficeMode();
            }
        }

        /// <summary>
        /// <para>执行卸载激活，异步命令</para>
        /// <para>Execute uninstall activation, asynchronous command</para>
        /// </summary>
        /// <returns>
        /// <para><see cref="Task"/> 类型，表示异步任务</para>
        /// <para><see cref="Task"/> type, representing an asynchronous task</para>
        /// </returns>
        [RelayCommand]
        private async Task DeActivateAsync()
        {
            if (IsWindowsActivationMode && !IsOfficeActivationMode)
            {
                await DeActivateOnWindowsMode();
            }
            else if (!IsWindowsActivationMode && IsOfficeActivationMode)
            {
                await DeActivateOnOfficeMode();
            }
        }

        /// <summary>
        /// <para>加载设置，同步命令</para>
        /// <para>Load settings, synchronous command</para>
        /// </summary>
        [RelayCommand]
        private void LoadSettings()
        {
            IsWindowsActivationMode = AllSettingsModel.Instance.IsWindowsActivationMode;
            IsOfficeActivationMode = AllSettingsModel.Instance.IsOfficeActivationMode;
            SelectedServerIndex = AllSettingsModel.Instance.SelectedServerIndex;
        }

        /// <summary>
        /// <para>保存设置，异步命令</para>
        /// <para>Save settings, asynchronous command</para>
        /// </summary>
        /// <returns>
        /// <para><see cref="Task"/> 类型，表示异步任务</para>
        /// <para><see cref="Task"/> type, representing an asynchronous task</para>
        /// </returns>
        [RelayCommand]
        private async Task SaveSettings()
        {
            AllSettingsModel.Instance.IsWindowsActivationMode = IsWindowsActivationMode;
            AllSettingsModel.Instance.IsOfficeActivationMode = IsOfficeActivationMode;
            AllSettingsModel.Instance.SelectedServerIndex = SelectedServerIndex;
            await ModelOperation.SaveConfigAsync(AllSettingsModel.Instance, Shared.JsonFilePath ??
                                                 Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) +
                                                 "\\Anawaert KMS Activator\\Activator Configuration.json");
        }

        /* 暂时删去的手动自动续签功能，因为 KMS 激活实际上会自动连接服务器进行续签，无需手动执行一遍 KMS 命令激活 */
        /* The manual automatic renewal function that was temporarily removed, because KMS activation will actually automatically connect to the server for renewal, there is no need to manually execute the KMS command activation */
        //[RelayCommand]
        //private void EnabledManualRenewalSwitch()
        //{

        //}

        //[RelayCommand]
        //private void DisabledManualRenewalSwitch()
        //{

        //}

        /// <summary>
        /// <para>在线激活页的 ViewModel 的主构造函数，使用 IoC 容器注入两个 MessageBox 服务</para>
        /// <para>Main constructor of the ViewModel of the online activation page, using IoC container to inject two MessageBox services</para>
        /// </summary>
        /// <param name="showInformationMessageBoxService">
        /// <para>显示 Information 图标类型的 MessageBox 服务</para>
        /// <para>Service for showing MessageBox with Information button type</para>
        /// </param>
        /// <param name="showErrorMessageBoxService">
        /// <para>显示 Error 图标类型的 MessageBox 服务</para>
        /// <para>Service for showing MessageBox with Error button type</para>
        /// </param>
        public OnlineActivationViewModel(IShowInformationMessageBoxService showInformationMessageBoxService, IShowErrorMessageBoxService showErrorMessageBoxService)
        {
            _showInformationMessageBoxService = showInformationMessageBoxService;
            _showErrorMessageBoxService = showErrorMessageBoxService;
        }

        /// <summary>
        /// <para>在线激活页的 ViewModel 的无参构造函数，用于产生设计时实例</para>
        /// <para>Parameterless constructor of the ViewModel of the online activation page, used to generate design-time instances</para>
        /// </summary>
        public OnlineActivationViewModel()
        {
            _showInformationMessageBoxService = App.ServiceProvider!.GetRequiredService<IShowInformationMessageBoxService>();
            _showErrorMessageBoxService = App.ServiceProvider!.GetRequiredService<IShowErrorMessageBoxService>();
        }
    }
}
