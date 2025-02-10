using Activator.Models;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Activator.ViewModels
{
    /// <summary>
    /// <para>设置页的 ViewModel</para>
    /// <para>ViewModel for the settings page</para>
    /// </summary>
    public partial class SettingsViewModels : ViewModelBase
    {
        /// <summary>
        /// <para>Windows 产品名称（字符串）</para>
        /// <para>Windows product name (string)</para>
        /// </summary>
        [ObservableProperty]
        private string? _windowsProduct;

        /// <summary>
        /// <para>Office 版本（字符串）</para>
        /// <para>Office edition (string)</para>
        /// </summary>
        [ObservableProperty]
        private string? _officeEdition;

        /// <summary>
        /// <para>激活器版本（字符串）</para>
        /// <para>Activator version (string)</para>
        /// </summary>
        [ObservableProperty]
        private string _activatorVersion = "Dev 2025.0202";

        /// <summary>
        /// <para>是否启用更新检查</para>
        /// <para>Whether to enable update check</para>
        /// </summary>
        [ObservableProperty]
        private bool _isUpdateCheckEnabled;

        /// <summary>
        /// <para>是否为浅色主题模式</para>
        /// <para>Whether it is light theme mode</para>
        /// </summary>
        [ObservableProperty]
        private bool _isLightThemeMode;

        /// <summary>
        /// <para>是否为深色主题模式</para>
        /// <para>Whether it is dark theme mode</para>
        /// </summary>
        [ObservableProperty]
        private bool _isDarkThemeMode;

        /// <summary>
        /// <para>更新 Windows 产品名称</para>
        /// <para>Update Windows product name</para>
        /// </summary>
        /// <param name="sender">
        /// <para>Windows 产品名称，也可能是回调触发者对象</para>
        /// <para>Windows product name, may also be the callback trigger object</para>
        /// </param>
        private void UpdateWindowsProduct(WindowsProductName sender) => WindowsProduct = sender.ToString().Replace("_Point_", ".").Replace('_', ' ');

        /// <summary>
        /// <para>更新 Office 版本</para>
        /// <para>Update Office edition</para>
        /// </summary>
        /// <param name="sender">
        /// <para>Office 版本，也可能是回调触发者对象</para>
        /// <para>Office edition, may also be the callback trigger object</para>
        /// </param>
        private void UpdateOfficeEdition(OfficeEditionName sender) => OfficeEdition = sender.ToString().Replace('_', ' ');

        /// <summary>
        /// <para>更新页面中的所有信息，To Settings 指本 ViewModel，异步命令</para>
        /// <para>Update all information on the page, To Settings refers to this ViewModel, asynchronous command</para>
        /// </summary>
        /// <returns>
        /// <para><see cref="Task"/> 类型，表示异步操作</para>
        /// <para><see cref="Task"/> type, representing the task of asynchronous operation</para>
        /// </returns>
        [RelayCommand]
        private async Task UpdateInfoToSettings()
        {
            await Task.Run
            (
                () =>
                {
                    WindowsInfo.RefreshWindowsInfo();
                    OfficeInfo.RefreshOfficeInfo();
                }
            );
            UpdateWindowsProduct(WindowsInfo.WindowsProduct);
            UpdateOfficeEdition(OfficeInfo.OfficeEdition);
        }

        /// <summary>
        /// <para>加载设置，同步命令</para>
        /// <para>Load settings, synchronous command</para>
        /// </summary>
        [RelayCommand]
        private void LoadSettings()
        {
            IsUpdateCheckEnabled = AllSettingsModel.Instance.IsUpdateCheckEnabled;
            IsLightThemeMode = AllSettingsModel.Instance.IsLightThemeMode;
            IsDarkThemeMode = !IsLightThemeMode;

            // 此处操作了 View 层的主题模式，但暂时没有找到更好的解决方案
            // The theme mode of the View layer is operated here, but a better solution has not been found for the time being
            Wpf.Ui.Appearance.ApplicationThemeManager.Apply
            (
                IsLightThemeMode ? Wpf.Ui.Appearance.ApplicationTheme.Light : Wpf.Ui.Appearance.ApplicationTheme.Dark
            );
        }

        /// <summary>
        /// <para>保存设置，异步命令</para>
        /// <para>Save settings, asynchronous command</para>
        /// </summary>
        /// <returns>
        /// <para><see cref="Task"/> 类型，表示异步操作</para>
        /// <para><see cref="Task"/> type, representing the task of asynchronous operation</para>
        /// </returns>
        [RelayCommand]
        private async Task SaveSettings()
        {
            AllSettingsModel.Instance.IsUpdateCheckEnabled = IsUpdateCheckEnabled;
            AllSettingsModel.Instance.IsLightThemeMode = IsLightThemeMode;

            await ModelOperation.SaveConfigAsync(AllSettingsModel.Instance, Shared.JsonFilePath ?? 
                                                 Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + 
                                                 "\\Anawaert KMS Activator\\Activator Configuration.json");
        }

        /// <summary>
        /// <para>切换到浅色主题模式，同步命令</para>
        /// <para>Switch to light theme mode, synchronous command</para>
        /// </summary>
        [RelayCommand]
        private void SwitchToLightThemeMode()
        {
            // 同理，此处操作了 View 层的主题模式，但暂时没有找到更好的解决方案
            // Similarly, the theme mode of the View layer is operated here, but a better solution has not been found for the time being
            Wpf.Ui.Appearance.ApplicationThemeManager.Apply(Wpf.Ui.Appearance.ApplicationTheme.Light);
            IsLightThemeMode = true;
            IsDarkThemeMode = !IsLightThemeMode;
        }

        /// <summary>
        /// <para>切换到深色主题模式，同步命令</para>
        /// <para>Switch to dark theme mode, synchronous command</para>
        /// </summary>
        [RelayCommand]
        private void SwitchToDarkThemeMode()
        {
            Wpf.Ui.Appearance.ApplicationThemeManager.Apply(Wpf.Ui.Appearance.ApplicationTheme.Dark);
            IsDarkThemeMode = true;
            IsLightThemeMode = !IsDarkThemeMode;
        }

        /// <summary>
        /// <para>设置页的 ViewModel 主构造函数，暂无使用 IoC 容器进行依赖注入</para>
        /// <para>Main constructor of the ViewModel for the settings page, no use of IoC container for dependency injection</para>
        /// </summary>
        public SettingsViewModels()
        {
            // 使用事件订阅的方式更新 Windows 产品名称和 Office 版本
            // Update Windows product name and Office edition using event subscription
            WindowsInfo.WindowsProductChanged += UpdateWindowsProduct;
            OfficeInfo.OfficeEditionChanged += UpdateOfficeEdition;

            UpdateWindowsProduct(WindowsInfo.WindowsProduct);
            UpdateOfficeEdition(OfficeInfo.OfficeEdition);
        }

        /// <summary>
        /// <para>设置页的 ViewModel 析构函数，释放事件订阅</para>
        /// <para>Destructor of the ViewModel for the settings page, release event subscription</para>
        /// </summary>
        ~SettingsViewModels()
        {
            WindowsInfo.WindowsProductChanged -= UpdateWindowsProduct;
            OfficeInfo.OfficeEditionChanged -= UpdateOfficeEdition;
        }
    }
}
