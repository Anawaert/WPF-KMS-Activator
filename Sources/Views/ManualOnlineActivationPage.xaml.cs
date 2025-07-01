using Activator.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using Microsoft.Extensions.DependencyInjection;

namespace Activator.Views
{
    /// <summary>
    /// <para>ManualOnlineActivationPage.xaml 的交互逻辑</para>
    /// <para>ManualOnlineActivationPage.xaml's interaction logic</para>
    /// </summary>
    public partial class ManualOnlineActivationPage : Page
    {
        /// <summary>
        /// <para>手动在线激活页面的构造函数，需要传入 <see cref="ManualOnlineActivationViewModels"/> 实例</para>
        /// <para>Constructor of ManualOnlineActivationPage, need to pass the instance of <see cref="ManualOnlineActivationViewModels"/></para>
        /// </summary>
        /// <param name="manualOnlineActivationViewModels">
        /// <para>一个 <see cref="ManualOnlineActivationViewModels"/> 实例，<see cref="ManualOnlineActivationPage"/> 的 DataContext</para>
        /// <para>A <see cref="ManualOnlineActivationViewModels"/> instance, DataContext of <see cref="ManualOnlineActivationPage"/></para>
        /// </param>
        public ManualOnlineActivationPage(ManualOnlineActivationViewModels manualOnlineActivationViewModels)
        {
            // 应在初始化组件之前设置 DataContext
            // DataContext should be set before initializing components
            DataContext = manualOnlineActivationViewModels;
            InitializeComponent();
        }

        /// <summary>
        /// <para>手动在线激活页的无参构造函数，但会自动获取 <see cref="ManualOnlineActivationViewModels"/> 的实例</para>
        /// <para>Constructor of ManualOnlineActivationPage without parameters, but will automatically get the instance of <see cref="ManualOnlineActivationViewModels"/></para>
        /// <para>由于服务一定存在且被注册，因此直接使用 <see cref="App.ServiceProvider"/> 获取服务</para>
        /// <para>Since the service must exist and be registered, use <see cref="App.ServiceProvider"/> to get the service directly</para>
        /// </summary>
        public ManualOnlineActivationPage() : this(App.ServiceProvider!.GetRequiredService<ManualOnlineActivationViewModels>()) { }
    }
}
