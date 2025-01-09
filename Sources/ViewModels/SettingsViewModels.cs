using Activator.Models;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Activator.ViewModels
{
    public partial class SettingsViewModels : ViewModelBase
    {
        [ObservableProperty]
        private string? _windowsProduct;

        [ObservableProperty]
        private string? _officeEdition;

        [ObservableProperty]
        private string _activatorVersion = "Dev 2025.0110";

        [ObservableProperty]
        private bool _isUpdateCheckEnabled;

        private void UpdateWindowsProduct(WindowsProductName sender) => this.WindowsProduct = sender.ToString().Replace("_Point_", ".").Replace('_', ' ');

        private void UpdateOfficeEdition(OfficeEditionName sender) => this.OfficeEdition = sender.ToString().Replace('_', ' ');

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

        [RelayCommand]
        private void LoadSettings() => this.IsUpdateCheckEnabled = AllSettingsModel.Instance.IsUpdateCheckEnabled;

        [RelayCommand]
        private async Task SaveSettings()
        {
            AllSettingsModel.Instance.IsUpdateCheckEnabled = this.IsUpdateCheckEnabled;
            await ModelOperation.SaveConfigAsync(AllSettingsModel.Instance, Shared.JsonFilePath ?? 
                                                 Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + 
                                                 "\\Anawaert KMS Activator\\Activator Configuration.json");
        }

        public SettingsViewModels()
        {
            WindowsInfo.WindowsProductChanged += UpdateWindowsProduct;
            OfficeInfo.OfficeEditionChanged += UpdateOfficeEdition;

            UpdateWindowsProduct(WindowsInfo.WindowsProduct);
            UpdateOfficeEdition(OfficeInfo.OfficeEdition);
        }

        ~SettingsViewModels()
        {
            WindowsInfo.WindowsProductChanged -= UpdateWindowsProduct;
            OfficeInfo.OfficeEditionChanged -= UpdateOfficeEdition;
        }

    }
}
