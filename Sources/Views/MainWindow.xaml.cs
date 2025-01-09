using System;
using System.Windows;
using System.Windows.Controls;
using System.Threading;
using System.Collections.Generic;
using Wpf.Ui;
using static Activator.Shared;
using Wpf.Ui.Controls;


namespace Activator.Views
{
    /// <summary>
    /// <para>Interaction logic for MainWindow.xaml</para>
    /// <para>将不会为该类的所有函数编写XML文档</para>
    /// <para>XML documents will not be written for all functions of this class</para>
    /// </summary>
    public partial class MainWindow : FluentWindow
    {

        /// <summary>
        /// 主窗口类构造函数
        /// </summary>
        public MainWindow()
        {
            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            mainWindowNavigationView.Navigate(typeof(HomePage));
        }

        private void OnNavigationViewSelectionChanged(NavigationView sender, RoutedEventArgs args)
        {
            if (sender is not NavigationView navigationView)
            {
                return;
            }

            sender.SetCurrentValue(NavigationView.HeaderVisibilityProperty,
                                   navigationView.SelectedItem?.TargetPageType != typeof(HomePage) ?
                                   Visibility.Visible :
                                   Visibility.Collapsed
            );
        }
    }
}
