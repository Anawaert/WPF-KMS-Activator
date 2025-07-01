using System;
using System.Windows;
using Wpf.Ui.Controls;
using Microsoft.Extensions.DependencyInjection;
using Activator.ViewModels;


namespace Activator.Views
{
    /// <summary>
    /// <para>MainWindow.xaml 的交互逻辑</para>
    /// <para>Interaction logic for MainWindow.xaml</para>
    /// </summary>
    public partial class MainWindow : FluentWindow
    {

        /// <summary>
        /// <para>主窗口类构造函数，需要传入 <see cref="MainWindow"/> 的 ViewModel</para>
        /// <para>Constructor of MainWindow class, need to pass the ViewModel of <see cref="MainWindow"/></para>
        /// </summary>
        public MainWindow(MainWindowViewModel mainWindowViewModel)
        {
            // 应当在在初始化组件前指定 DataContext
            // DataContext should be assigned before initializing components
            this.DataContext = mainWindowViewModel;
            InitializeComponent();
        }

        /// <summary>
        /// <para>主窗口的无参构造函数，但会自动获取 <see cref="MainWindowViewModel"/> 的实例</para>
        /// <para>Constructor of MainWindow without parameters, but will automatically get the instance of <see cref="MainWindowViewModel"/></para>
        /// <para>由于服务一定存在且被注册，因此直接使用 <see cref="App.ServiceProvider"/> 获取服务</para>
        /// <para>Since the service must exist and be registered, use <see cref="App.ServiceProvider"/> to get the service directly</para>
        /// </summary>
        public MainWindow() : this(App.ServiceProvider!.GetRequiredService<MainWindowViewModel>()) { }

        /// <summary>
        /// <para>主窗口载入后的事件处理</para>
        /// <para>Event handling after the main window is loaded</para>
        /// </summary>
        /// <param name="sender">
        /// <para>事件触发者</para>
        /// <para>Event trigger</para>
        /// </param>
        /// <param name="e">
        /// <para>路由事件参数</para>
        /// <para>Routed event arguments</para>
        /// </param>
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            // 令导航视图导航到主页
            // Navigate the navigation view to the home page
            mainWindowNavigationView.Navigate(typeof(HomePage));
        }

        /// <summary>
        /// <para>当导航页发生改变时的事件处理</para>
        /// <para>Event handling when the navigation page changes</para>
        /// </summary>
        /// <param name="sender">
        /// <para>事件触发者</para>
        /// <para>Event trigger</para>
        /// </param>
        /// <param name="args">
        /// <para>路由事件参数</para>
        /// <para>Routed event arguments</para>
        /// </param>
        private void OnNavigationViewSelectionChanged(NavigationView sender, RoutedEventArgs args)
        {
            // 若事件触发者不是 NavigationView，则直接返回
            // If the event trigger is not NavigationView, return directly
            if (sender is not NavigationView navigationView)
            {
                return;
            }

            // 当导航选中项的目标页面类型不是 HomePage 时，显示导航视图的标题
            // When the target page type of the selected navigation item is not HomePage, display the title of the navigation view
            sender.SetCurrentValue(NavigationView.HeaderVisibilityProperty,
                                   navigationView.SelectedItem?.TargetPageType != typeof(HomePage) ?
                                   Visibility.Visible :
                                   Visibility.Collapsed
            );
        }
    }
}
