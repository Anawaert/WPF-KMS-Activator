using System;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;
using System.Text.RegularExpressions;
using System.Collections.ObjectModel;
using System.Windows.Controls;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Activator;
using Activator.ServiceInterfaces;


namespace Activator.ViewModels
{
    public partial class OnlineActivationViewModel : ViewModelBase
    {
        private readonly IShowInformationMessageBoxService _showInformationMessageBoxService;

        private readonly IShowErrorMessageBoxService _showErrorMessageBoxService;

        [ObservableProperty]
        private bool _isWindowsActivationMode = true;

        [ObservableProperty]
        private bool _isOfficeActivationMode = false;

        [ObservableProperty]
        private ObservableCollection<string> _servers = new ObservableCollection<string>()
        {
            "Anawaert 服务器（推荐）",
            "服务器：kms.03k.org",
            "服务器：kms.cgtsoft.com"
        };

        [ObservableProperty]
        private string? _selectedServerContentString;

        [ObservableProperty]
        private bool _isAutoKMSRenewal = true;

        [ObservableProperty]
        private bool _isButtonEnabled = true;

        private async Task ActivateOnWindowsMode()
        {
            await Task.Run
            (
                () =>
                {
                    WindowsOnlineActivator windowsOnlineActivator = new WindowsOnlineActivator();

                    if (!string.IsNullOrEmpty(SelectedServerContentString))
                    {
                        string server = SelectedServerContentString.Contains("Anawaert") ?
                                        Shared.AnawaertServerAddress :
                                        Regex.Match(SelectedServerContentString, @"([a-zA-Z0-9]+\.[a-zA-Z0-9]+\.[a-zA-Z0-9]+)").Value;

                        if (WindowsInfo.IsWindowsActivated)
                        {
                            _showInformationMessageBoxService.ShowMessageBox("您的 Windows 已处于激活状态，无需再次激活", "提示");
                            return;
                        }

                        windowsOnlineActivator.ConvertToReleaseVersion(WindowsInfo.WindowsProduct, WindowsInfo.IsReleaseVersion);
                        string? windowsKey = windowsOnlineActivator.GetActivationKey(WindowsInfo.WindowsProduct);
                        if (windowsKey is null)
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
                            _showErrorMessageBoxService.ShowMessageBox("未能正确连接至选定的服务器，，请重试或联系 Anawaert", "错误");
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
                        return;
                    }

                    WindowsInfo.RefreshWindowsInfo();
                }
            );
        }

        private async Task DeActivateOnWindowsMode()
        {
            await Task.Run
            (
                () =>
                {
                    WindowsOnlineActivator windowsOnlineActivator = new WindowsOnlineActivator();

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

                    WindowsInfo.RefreshWindowsInfo();
                }
            );
        }

        [RelayCommand]
        private async Task ActivateAsync()
        {
            if (IsWindowsActivationMode && !IsOfficeActivationMode)
            {
                await ActivateOnWindowsMode();
            }
            else if (!IsWindowsActivationMode && IsOfficeActivationMode)
            {
                Debug.WriteLine("Office activation mode is not supported yet");
            }
        }

        [RelayCommand]
        private async Task DeActivateAsync()
        {
            if (IsWindowsActivationMode && !IsOfficeActivationMode)
            {
                await DeActivateOnWindowsMode();
            }
            else if (!IsWindowsActivationMode && IsOfficeActivationMode)
            {
                Debug.WriteLine("Office activation mode is not supported yet");
            }
        }

        public OnlineActivationViewModel(IShowInformationMessageBoxService showInformationMessageBoxService, IShowErrorMessageBoxService showErrorMessageBoxService)
        {
            _showInformationMessageBoxService = showInformationMessageBoxService;
            _showErrorMessageBoxService = showErrorMessageBoxService;
        }

        public OnlineActivationViewModel() { }
    }
}
