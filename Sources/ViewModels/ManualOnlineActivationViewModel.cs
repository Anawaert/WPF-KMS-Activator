using System;
using System.Threading.Tasks;
using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.Extensions.DependencyInjection;
using Activator.Models;
using Activator.ServiceInterfaces;

namespace Activator.ViewModels
{
    /// <summary>
    /// <para>手动在线激活的 ViewModel</para>
    /// <para>Manual online activation ViewModel</para>
    /// </summary>
    public partial class ManualOnlineActivationViewModels : ViewModelBase
    {
        /// <summary>
        /// <para>显示 Information 图标类型的服务</para>
        /// <para>Service to show Information type message box</para>
        /// </summary>
        private readonly IShowInformationMessageBoxService _showInformationMessageBoxService;

        /// <summary>
        /// <para>显示 Error 图标类型的服务</para>
        /// <para>Service to show Error type message box</para>
        /// </summary>
        private readonly IShowErrorMessageBoxService _showErrorMessageBoxService;

        /// <summary>
        /// <para>用户是否选择为 Windows 激活模式</para>
        /// <para>Whether user selects Windows activation mode</para>
        /// </summary>
        [ObservableProperty]
        private bool _isWindowsActivationMode = true;

        /// <summary>
        /// <para>用户是否选择为 Office 激活模式</para>
        /// <para>Whether user selects Office activation mode</para>
        /// </summary>
        [ObservableProperty]
        private bool _isOfficeActivationMode = false;

        /// <summary>
        /// <para>支持的 Windows 产品名称集合</para>
        /// <para>Supported Windows product names collection</para>
        /// </summary>
        [ObservableProperty]
        private ObservableCollection<string> _selectiveWindowsProductNamesCollection = new ObservableCollection<string>();

        /// <summary>
        /// <para>支持的 Office 版本集合</para>
        /// <para>Supported Office editions collection</para>
        /// </summary>
        [ObservableProperty]
        private ObservableCollection<string> _selectiveOfficeEditionsCollection = new ObservableCollection<string>();

        /// <summary>
        /// <para>用户选择的产品名称或版本（字符串）</para>
        /// <para>Selected product name or edition (string) by user</para>
        /// </summary>
        [ObservableProperty]
        private string? _selectedProductNameOrEditionString;

        /// <summary>
        /// <para>用户选择的产品名称或版本索引</para>
        /// <para>Selected product name or edition index by user</para>
        /// </summary>
        [ObservableProperty]
        private int _selectedProductNameOrEditionIndex = 0;

        /// <summary>
        /// <para>手动激活的服务器地址</para>
        /// <para>Manual activation server address</para>
        /// </summary>
        [ObservableProperty]
        private string? _serverAddress;

        /// <summary>
        /// <para>手动激活的批量密钥</para>
        /// <para>Manual activation volume key</para>
        /// </summary>
        [ObservableProperty]
        private string? _activationKey;

        /// <summary>
        /// <para>当用户选择为 Windows 激活时执行的函数</para>
        /// <para>Function to execute when user selects Windows activation</para>
        /// </summary>
        /// <returns>
        /// <para><see cref="Task"/> 类型，表示异步操作</para>
        /// <para><see cref="Task"/> type, represents asynchronous operation</para>
        /// </returns>
        private async Task ActivateOnWindowsMode()
        {
            // 以异步方式执行
            // Execute asynchronously
            await Task.Run
            (
                () =>
                {
                    WindowsOnlineActivator windowsOnlineActivator = new WindowsOnlineActivator();

                    // 先检查 Windows 是否已激活，若已激活则提示用户无需再次激活
                    // Check if Windows is activated first, if activated, prompt user that no need to activate again
                    if (WindowsInfo.IsWindowsActivated)
                    {
                        _showInformationMessageBoxService.ShowMessageBox("您的 Windows 已处于激活状态，无需再次激活", "提示");
                        return;
                    }

                    // 检查是否填入了非空的服务器地址
                    // Check if non-empty server address is filled
                    if (!string.IsNullOrEmpty(ServerAddress))
                    {
                        string server = ServerAddress.Trim().Replace(" ", string.Empty);

                        // 将 Windows 转换为非评估版本
                        // Convert Windows to non-evaluation version
                        windowsOnlineActivator.ConvertToReleaseVersion(WindowsInfo.WindowsProduct, WindowsInfo.IsReleaseVersion);

                        string? windowsKey = ActivationKey?.Trim().Replace(" ", string.Empty);

                        // 根据 KMS 激活步骤逐步执行，若某一步骤失败，则提示用户
                        // Execute step by step according to KMS activation steps, if any step fails, prompt user
                        if (string.IsNullOrEmpty(windowsKey))
                        {
                            _showErrorMessageBoxService.ShowMessageBox("您未填入任何批量激活密钥，请填写批量激活密钥", "错误");
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
                        _showErrorMessageBoxService.ShowMessageBox("您未填入任何服务器地址，请填写服务器地址", "错误");
                    }

                    // 刷新 Windows 信息
                    // Refresh Windows information
                    WindowsInfo.RefreshWindowsInfo();
                }
            );
        }

        /// <summary>
        /// <para>当用户选择为 Windows 移除激活时执行的函数</para>
        /// <para>Function to execute when user selects to deactivate Windows</para>
        /// </summary>
        /// <returns>
        /// <para><see cref="Task"/> 类型，表示异步操作</para>
        /// <para><see cref="Task"/> type, represents asynchronous operation</para>
        /// </returns>
        private async Task DeActivateOnWindowsMode()
        {
            await Task.Run
            (
                () =>
                {
                    WindowsOnlineActivator windowsOnlineActivator = new WindowsOnlineActivator();

                    // 若 Windows 已激活，则执行移除激活操作
                    // If Windows is activated, execute deactivation operation
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


        // 以下两个函数与上面两个函数的逻辑类似，不再赘述
        // The logic of the following two functions is similar to the above two functions, so it is not repeated here
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

                    if (!string.IsNullOrEmpty(ServerAddress))
                    {
                        string server = ServerAddress.Trim().Replace(" ", string.Empty);

                        officeOnlineActivator.ConvertToVolumeLicense(OfficeInfo.LicenseDirectory, OfficeInfo.OsppDirectory, OfficeInfo.OfficeEdition, OfficeInfo.VisioEdition);

                        string? officeKey = ActivationKey?.Trim().Replace(" ", string.Empty);

                        if (string.IsNullOrEmpty(officeKey))
                        {
                            _showErrorMessageBoxService.ShowMessageBox("您未填入任何批量激活密钥，请填写批量激活密钥", "错误");
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
                        _showErrorMessageBoxService.ShowMessageBox("您未填入任何服务器地址，请填写服务器地址", "错误");
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
        /// <para><see cref="Task"/> 类型，表示异步操作</para>
        /// <para><see cref="Task"/> type, represents asynchronous operation</para>
        /// </returns>
        [RelayCommand]
        private async Task ActivateAsync()
        {
            // 逻辑可优化
            // Logic can be optimized
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
        /// <para>执行移除激活，异步命令</para>
        /// <para>Execute deactivation, asynchronous command</para>
        /// </summary>
        /// <returns>
        /// <para><see cref="Task"/> 类型，表示异步操作</para>
        /// <para><see cref="Task"/> type, represents asynchronous operation</para>
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
            SelectedProductNameOrEditionIndex = AllSettingsModel.Instance.SelectedProductNameOrEditionIndex;
            ServerAddress = AllSettingsModel.Instance.ManualActivationServerAddress;
            ActivationKey = AllSettingsModel.Instance.ManualActivationVolumeKey;
        }

        /// <summary>
        /// <para>保存设置，异步命令</para>
        /// <para>Save settings, asynchronous command</para>
        /// </summary>
        /// <returns>
        /// <para><see cref="Task"/> 类型，表示异步操作</para>
        /// <para><see cref="Task"/> type, represents asynchronous operation</para>
        /// </returns>
        [RelayCommand]
        private async Task SaveSettings()
        {
            AllSettingsModel.Instance.IsWindowsActivationMode = IsWindowsActivationMode;
            AllSettingsModel.Instance.IsOfficeActivationMode = IsOfficeActivationMode;
            AllSettingsModel.Instance.SelectedProductNameOrEditionIndex = SelectedProductNameOrEditionIndex;
            AllSettingsModel.Instance.ManualActivationServerAddress = ServerAddress;
            AllSettingsModel.Instance.ManualActivationVolumeKey = ActivationKey;
            await ModelOperation.SaveConfigAsync(AllSettingsModel.Instance, Shared.JsonFilePath ??
                                                 Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) +
                                                 "\\Anawaert KMS Activator\\Activator Configuration.json");
        }

        /// <summary>
        /// <para>手动在线激活页的 ViewModel 主构造函数，通过 IoC 容器注入两个 MessageBox 服务</para>
        /// <para>Main constructor of Manual online activation page ViewModel, inject two MessageBox services through IoC container</para>
        /// </summary>
        /// <param name="showInformationMessageBoxService">
        /// <para>显示 Information 图标类型的服务</para>
        /// <para>Service to show Information type message box</para>
        /// </param>
        /// <param name="showErrorMessageBoxService">
        /// <para>显示 Error 图标类型的服务</para>
        /// <para>Service to show Error type message box</para>
        /// </param>
        public ManualOnlineActivationViewModels(IShowInformationMessageBoxService showInformationMessageBoxService, IShowErrorMessageBoxService showErrorMessageBoxService)
        {
            _showInformationMessageBoxService = showInformationMessageBoxService;
            _showErrorMessageBoxService = showErrorMessageBoxService;

            // 从枚举中获取 Windows 产品名称和 Office 版本名称，添加到集合中
            // Get Windows product names and Office edition names from enumeration, add to collections
            foreach (var windowsProductName in Enum.GetNames(typeof(WindowsProductName)))
            {
                if (windowsProductName.Replace("_Point_", ".").Replace("_", " ") == WindowsProductName.Unsupported.ToString())
                {
                    continue;
                }
                SelectiveWindowsProductNamesCollection.Add(windowsProductName.Replace("_Point_", ".").Replace("_", " "));
            }
            foreach (var officeEditionName in Enum.GetNames(typeof(OfficeEditionName)))
            {
                if (officeEditionName.Replace("_", " ") == OfficeEditionName.Unsupported.ToString())
                {
                    continue;
                }
                SelectiveOfficeEditionsCollection.Add(officeEditionName.Replace("_", " "));
            }
        }

        /// <summary>
        /// <para>手动在线激活页的 ViewModel 无参构造函数，用于提供设计时实例</para>
        /// <para>Manual online activation page ViewModel parameterless constructor, used to provide design-time instance</para>
        /// </summary>
        public ManualOnlineActivationViewModels()
        {
            _showInformationMessageBoxService = App.ServiceProvider!.GetRequiredService<IShowInformationMessageBoxService>();
            _showErrorMessageBoxService = App.ServiceProvider!.GetRequiredService<IShowErrorMessageBoxService>();
        }
    }
}
