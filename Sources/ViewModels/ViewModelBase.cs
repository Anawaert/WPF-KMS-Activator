using System;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace Activator.ViewModels
{
    public partial class ViewModelBase : ObservableObject
    {
        [RelayCommand]
        private void RefreshGlobalConfig()
        {

        }
    }
}
