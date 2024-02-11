using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Animation;
using System.Threading;
using System.Collections.Generic;
using static KMS_Activator.Shared;
using static KMS_Activator.Office_Configurator;
using static KMS_Activator.Animations_Related;

namespace KMS_Activator
{
    /// <summary>
    /// <para>Interaction logic for MainWindow.xaml</para>
    /// <para>将不会为该类的所有函数编写XML文档</para>
    /// <para>XML documents will not be written for all functions of this class</para>
    /// </summary>
    public partial class MainWindow : Window
    {
        /// <summary>
        /// 主窗口类构造函数
        /// </summary>
        public MainWindow()
        {
            InitializeComponent();
        }

        private async void Window_Loaded(object sender, RoutedEventArgs e)
        {
            // 设置几个Label
            // Set the content of Labels in MainWindow
            mainWindow = (MainWindow)Application.Current.MainWindow;
            winVersion_Label.Content = WIN_VERSION;
            officeVersion_Label.Content = officeProduct;
            EnableConfigToUI();
            // 新增：加载完主窗口后选择是否检查更新
            if (Current_Config.isAutoUpdate)
            {
                await AutoCheckUpdate();
            }
        }

        // 当“+”号Label按钮按下的时候
        // When the "+" Label button is pressed
        private void addServerName_Button_Click(object sender, RoutedEventArgs e)
        {
            if (addServerName_TextBox.Visibility == Visibility.Hidden)
            {
                addServerName_TextBox.Visibility = Visibility.Visible;
                addServerName_Button.Content = "√";
                deleteServerName_Button.Visibility = Visibility.Hidden;
            }
            else
            {
                if (addServerName_TextBox.Text.Trim(' ') != string.Empty && !addServerName_TextBox.Text.Contains("Anawaert KMS 服务器"))
                {
                    ComboBoxItem newItem = new ComboBoxItem
                    {
                        Content = addServerName_TextBox.Text,
                        IsSelected = true,
                    };
                    selectServer_ComboBox.Items.Add(newItem);
                }
                addServerName_TextBox.Text = string.Empty;
                addServerName_TextBox.Visibility = Visibility.Hidden;
                deleteServerName_Button.Visibility = Visibility.Visible;
                addServerName_Button.Content = "+";
            }
        }

        // 当“-”号Label按钮按下的时候
        // When the "-" Label button is pressed
        private void deleteServerName_Button_Click(object sender, RoutedEventArgs e)
        {
            selectServer_ComboBox.Items.Remove
            (
                (string)((ComboBoxItem)selectServer_ComboBox.SelectedItem).Content == "Anawaert KMS 服务器" ? 
                null                                                                                        :
                selectServer_ComboBox.SelectedItem
            );
        }

        // 当“激活”Label按钮按下的时候
        // When the “激活” Label button is pressed
        private void activate_Button_MouseLeftButtonUp(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            // 执行动画
            // Begin the animation
            MainW_Slide(new List<Grid> { menuGrid, page1_Grid });
            awaiting_ProgressBar.IsIndeterminate = true;
            // 当awaiting_ProgressBar被选中时
            // When awaiting_ProgressBar was selected
            if (actWin_RadioButton.IsChecked == true)
            {
                // 使用另一个线程
                // Use another thread to run the functions for activating
                Thread subThread = new Thread
                (
                    () =>
                    {
                        Thread.Sleep(650);
                        string selectedContent = "anawaert.tech";
                        bool isAutoRenewChecked = true;
                        // 通过UI线程拿到KMS服务器的选择
                        // Get the KMS server selection through the UI thread
                        this.Dispatcher.Invoke
                        (
                            () =>
                            {
                                selectedContent = (string)((ComboBoxItem)selectServer_ComboBox.SelectedItem).Content == "Anawaert KMS 服务器" ?
                                                  "anawaert.tech" :
                                                  (string)((ComboBoxItem)selectServer_ComboBox.SelectedItem).Content;
                                isAutoRenewChecked = autoRenew_CheckBox.IsChecked == true;
                            }
                        );
                        Win_Activator activator = new Win_Activator();
                        activator.ActWin(selectedContent);
                        IsWinActivated();

                        // 若“自动续签”被选中，那么就设定一下计划任务；否则，就取消自动续签
                        // If Auto-renew is selected, set the scheduled tasks; Otherwise, the auto-renewal is canceled
                        if (Current_Config.isAutoRenew)
                        {
                            CancelAutoRenew("WINDOWS");
                            AutoRenewSign("WINDOWS");
                        }
                        else
                        {
                            CancelAutoRenew("WINDOWS");
                        }

                        // 执行完后再返回UI线程执行剩下的动画
                        //When you're done, return to the UI thread to execute the rest of the animation
                        this.Dispatcher.Invoke
                        (
                            () =>
                            {
                                MainW_SlideBack(new List<Grid> { menuGrid, page1_Grid });
                                awaiting_ProgressBar.IsIndeterminate = false;
                            }
                        );
                    }
                );
                subThread.Start();
            }
            // 同理上述
            // Same thing as above
            else if (actOffice_RadioButton.IsChecked == true)
            {
                Thread subThread = new Thread
                (
                    () =>
                    {
                        Thread.Sleep(650);
                        string selectedContent = "anawaert.tech";
                        bool isAutoRenewChecked = true;
                        this.Dispatcher.Invoke
                        (
                            () =>
                            {
                                selectedContent = (string)((ComboBoxItem)selectServer_ComboBox.SelectedItem).Content == "Anawaert KMS 服务器" ?
                                                  "anawaert.tech" :
                                                  (string)((ComboBoxItem)selectServer_ComboBox.SelectedItem).Content;
                                isAutoRenewChecked = autoRenew_CheckBox.IsChecked == true;
                            }
                        );
                        Office_Activator activator = new Office_Activator();
                        activator.ActOffice(selectedContent);

                        if (Current_Config.isAutoRenew)
                        {
                            CancelAutoRenew("OFFICE");
                            AutoRenewSign("OFFICE");
                        }
                        else
                        {
                            CancelAutoRenew("OFFICE");
                        }

                        this.Dispatcher.Invoke
                        (
                            () =>
                            {
                                MainW_SlideBack(new List<Grid> { menuGrid, page1_Grid });
                                awaiting_ProgressBar.IsIndeterminate = false;
                                officeVersion_Label.Content = officeProduct;
                            }
                        );
                    }
                );
                subThread.Start();
            }
        }

        private void autoRenew_CheckBox_Click(object sender, RoutedEventArgs e)
        {
            RefreshConfigInit();
            RefreshConfigFile(JSON_CFG_PATH);
        }

        private void autoUpdate_CheckBox_Click(object sender, RoutedEventArgs e)
        {
            RefreshConfigInit();
            RefreshConfigFile(JSON_CFG_PATH);
        }
    }
}
