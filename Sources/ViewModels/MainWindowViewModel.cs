using System;
using System.IO;
using System.Threading.Tasks;
using System.Windows;
using System.Xml.Serialization;
using Activator.Models;
using CommunityToolkit.Mvvm;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace Activator.ViewModels
{
    public partial class MainWindowViewModel : ViewModelBase
    {
        [ObservableProperty]
        private bool _isReadingSystemInfo;

        [RelayCommand]
        private async Task ReadSystemInfo()
        {
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

        [RelayCommand]
        private async Task CreateActivatorDirectory()
        {
            await Task.Run
            (
                () =>
                {
                    if (!Directory.Exists(Shared.UserDocumentsActivatorPath))
                    {
                        var retVal = Utility.CreateActivatorDirectory();
                    }
                }
            );
        }

        [RelayCommand]
        private async Task LoadAllSettings()
        {
            await Task.Run
            (
                () =>
                {
                    AllSettingsModel.Instance = ModelOperation.GetConfig<AllSettingsModel>(Shared.JsonFilePath ??
                                                                                           Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) +
                                                                                           "\\Anawaert KMS Activator\\Activator Configuration.json");
                }
            );
        }

        [RelayCommand]
        private async Task SaveAllSettings()
        {
            await Task.Run
            (
                () =>
                {
                    ModelOperation.SaveConfig(AllSettingsModel.Instance, Shared.JsonFilePath ??
                                              Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) +
                                              "\\Anawaert KMS Activator\\Activator Configuration.json");
                }
            );
        }
    }
}
