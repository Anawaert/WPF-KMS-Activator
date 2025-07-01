using System.Windows;

namespace Activator.ServiceInterfaces
{
    /// <summary>
    /// <para>展示 Information 图标类型的 MessageBox 的服务接口</para>
    /// <para>Service interface for showing MessageBox with Information icon</para>
    /// </summary>
    public interface IShowInformationMessageBoxService
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
        MessageBoxResult ShowMessageBox(string msgBoxText, string caption);
    }

    /// <summary>
    /// <para>展示 Error 图标类型的 MessageBox 的服务接口</para>
    /// <para>Service interface for showing MessageBox with Error icon</para>
    /// </summary>
    public interface IShowErrorMessageBoxService
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
        MessageBoxResult ShowMessageBox(string msgBoxText, string caption);
    }
}
