using System.Windows;

namespace Activator.ServiceInterfaces
{
    public interface IShowInformationMessageBoxService
    {
        MessageBoxResult ShowMessageBox(string msgBoxText, string caption);
    }

    public interface IShowErrorMessageBoxService
    {
        MessageBoxResult ShowMessageBox(string msgBoxText, string caption);
    }
}
