 using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using static KMS_Activator.Shared;
using static KMS_Activator.Office_Configurator;
using static KMS_Activator.Animations_Related;
using System.Windows.Media.Animation;

namespace KMS_Activator
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            winVersion_Label.Content = WIN_VERSION;
        }

        private void addServerName_Button_Click(object sender, RoutedEventArgs e)
        {
            if (addServerName_TextBox.Visibility == Visibility.Hidden)
            {
                addServerName_TextBox.Visibility = Visibility.Visible;
                addServerName_Button.Content = "√";
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
                addServerName_Button.Content = "+";
            }
        }

        private void activate_Button_Click(object sender, RoutedEventArgs e)
        {

            TranslateTransform translateTransform = new TranslateTransform();
            mainGrid.RenderTransform = translateTransform;

            // 创建平移动画
            DoubleAnimation animation = new DoubleAnimation();
            animation.From = 0; // 起始位置（左侧屏幕外）
            animation.To = -this.Width;     // 终止位置（屏幕中央）
            animation.Duration = TimeSpan.FromSeconds(1); // 动画持续时间

            // 添加慢入慢出的缓动函数
            animation.EasingFunction = new QuadraticEase { EasingMode = EasingMode.EaseInOut };

            // 启动动画
            translateTransform.BeginAnimation(TranslateTransform.XProperty, animation);

            if (actWin_RadioButton.IsChecked == true)
            {
                this.Dispatcher.Invoke
                (
                    () =>
                    {
                        Win_Activator activator = new Win_Activator();
                        activator.ActWin
                        (
                            (string)((ComboBoxItem)selectServer_ComboBox.SelectedItem).Content == "Anawaert KMS 服务器" ? 
                            "anawaert.tech"                                                                            : 
                            (string)((ComboBoxItem)selectServer_ComboBox.SelectedItem).Content
                        );
                        IsWinActivated();
                    }
                );
            }
            else if (actOffice_RadioButton.IsChecked == true)
            {
                this.Dispatcher.Invoke
                (
                    () =>
                    {
                        Office_Activator activator = new Office_Activator();
                        activator.ActOffice
                        (
                            (string)((ComboBoxItem)selectServer_ComboBox.SelectedItem).Content == "Anawaert KMS 服务器" ?
                            "anawaert.tech"                                                                            :
                            (string)((ComboBoxItem)selectServer_ComboBox.SelectedItem).Content
                        );
                    }
                );
            }
        }

        private void deleteServerName_Button_Click(object sender, RoutedEventArgs e)
        {
            selectServer_ComboBox.Items.Remove(selectServer_ComboBox.SelectedItem);
        }
    }
}
