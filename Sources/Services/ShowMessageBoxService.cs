using System.Windows;
using Activator.ServiceInterfaces;

namespace Activator.Services
{
    public class ShowInformationMessageBoxService : IShowInformationMessageBoxService
    {
        public MessageBoxResult ShowMessageBox(string msgBoxText, string caption)
        {
            return MessageBox.Show(msgBoxText, caption, MessageBoxButton.OK, MessageBoxImage.Information);
        }
    }

    public class ShowErrorMessageBoxService : IShowErrorMessageBoxService
    {
        public MessageBoxResult ShowMessageBox(string msgBoxText, string caption)
        {
            return MessageBox.Show(msgBoxText, caption, MessageBoxButton.OK, MessageBoxImage.Error);
        }
    }
}
