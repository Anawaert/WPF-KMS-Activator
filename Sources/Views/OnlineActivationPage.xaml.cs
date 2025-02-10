using System;
using System.Windows.Controls;
using Microsoft.Extensions.DependencyInjection;
using Activator.ViewModels;

namespace Activator.Views
{
    /// <summary>
    /// <para>OnlineActivationPage.xaml 的交互逻辑</para>
    /// <para>OnlineActivationPage.xaml's interaction logic</para>
    /// </summary>
    public partial class OnlineActivationPage : Page
    {
        /// <summary>
        /// <para>（自动）在线激活页的构造函数，需要传入一个 <see cref="OnlineActivationViewModel"/> 实例 </para>
        /// <para>Constructor of (auto) online activation page, need to pass a <see cref="OnlineActivationViewModel"/> instance</para>
        /// </summary>
        /// <param name="onlineActivationViewModel">
        /// <para>一个 <see cref="OnlineActivationViewModel"/> 实例，用作 DataContext</para>
        /// <para>A <see cref="OnlineActivationViewModel"/> instance, used as DataContext</para>
        /// </param>
        public OnlineActivationPage(OnlineActivationViewModel onlineActivationViewModel)
        {
            // 应在初始化组件前设置 DataContext
            // DataContext should be set before initializing components
            this.DataContext = onlineActivationViewModel;
            InitializeComponent();
        }

        /// <summary>
        /// <para>主窗口的无参构造函数，但会自动获取 <see cref="OnlineActivationViewModel"/> 的实例</para>
        /// <para>Constructor of MainWindow without parameters, but will automatically get the instance of <see cref="OnlineActivationViewModel"/></para>
        /// <para>由于服务一定存在且被注册，因此直接使用 <see cref="App.ServiceProvider"/> 获取服务</para>
        /// <para>Since the service must exist and be registered, use <see cref="App.ServiceProvider"/> to get the service directly</para>
        /// </summary>
        public OnlineActivationPage() : this(App.ServiceProvider!.GetRequiredService<OnlineActivationViewModel>()) { }
    }
}
