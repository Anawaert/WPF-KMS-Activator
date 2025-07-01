using System.Windows;
using Activator.ServiceInterfaces;

namespace Activator.Services
{
    /// <summary>
    /// <para>展示 Information 图标类型的 MessageBox 的服务实现</para>
    /// <para>Service implementation for showing MessageBox with Information icon</para>
    /// </summary>
    public class ShowInformationMessageBoxService : IShowInformationMessageBoxService
    {
        /// <summary>
        /// <para>展示 Information 图标类型的 MessageBox</para>
        /// <para>Show MessageBox with Information icon</para>
        /// </summary>
        /// <param name="msgBoxText">
        /// <para>要显示的消息文本</para>
        /// <para>Message text to display</para>
        /// </param>
        /// <param name="caption">
        /// <para>MessageBox 标题</para>
        /// <para>MessageBox title</para>
        /// </param>
        /// <returns>
        /// <para>用户点击后接收的 <see cref="MessageBoxResult"/> 类型的选择</para>
        /// <para><see cref="MessageBoxResult"/> choice received after user click</para>
        /// </returns>
        public MessageBoxResult ShowMessageBox(string msgBoxText, string caption)
        {
            return MessageBox.Show(msgBoxText, caption, MessageBoxButton.OK, MessageBoxImage.Information);
        }
    }

    /// <summary>
    /// <para>展示 Error 图标类型的 MessageBox 的服务实现</para>
    /// <para>Service implementation for showing MessageBox with Error icon</para>
    /// </summary>
    public class ShowErrorMessageBoxService : IShowErrorMessageBoxService
    {
        /// <summary>
        /// <para>展示 Error 图标类型的 MessageBox</para>
        /// <para>Show MessageBox with Error icon</para>
        /// </summary>
        /// <param name="msgBoxText">
        /// <para>要显示的消息文本</para>
        /// <para>Message text to display</para>
        /// </param>
        /// <param name="caption">
        /// <para>MessageBox 标题</para>
        /// <para>MessageBox title</para>
        /// </param>
        /// <returns>
        /// <para>用户点击后接收的 <see cref="MessageBoxResult"/> 类型的选择</para>
        /// <para><see cref="MessageBoxResult"/> choice received after user click</para>
        /// </returns>
        public MessageBoxResult ShowMessageBox(string msgBoxText, string caption)
        {
            return MessageBox.Show(msgBoxText, caption, MessageBoxButton.OK, MessageBoxImage.Error);
        }
    }
}
