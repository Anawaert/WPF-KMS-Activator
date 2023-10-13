 using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using static KMS_Activator.Shared;
using Fms = System.Windows.Forms;
using static KMS_Activator.Office_Configurator;
using static KMS_Activator.Animations_Related;
using System.Windows.Media.Animation;
using System.Threading;

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
            Fms::Application.EnableVisualStyles();
        }

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

        private void deleteServerName_Button_Click(object sender, RoutedEventArgs e)
        {
            selectServer_ComboBox.Items.Remove
            (
                (string)((ComboBoxItem)selectServer_ComboBox.SelectedItem).Content == "Anawaert KMS 服务器" ? 
                null                                                                                        :
                selectServer_ComboBox.SelectedItem
            );
        }

        private void activate_Button_MouseLeftButtonUp(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            MainW_Slide(mainGrid);
            if (actWin_RadioButton.IsChecked == true)
            {
                Thread subThread = new Thread
                (
                    () =>
                    {
                        Thread.Sleep(1000);
                        string selectedContent = "anawaert.tech";
                        this.Dispatcher.Invoke
                        (
                            () =>
                            {
                                selectedContent = (string)((ComboBoxItem)selectServer_ComboBox.SelectedItem).Content == "Anawaert KMS 服务器" ?
                                                  "anawaert.tech"                                                                             :
                                                  (string)((ComboBoxItem)selectServer_ComboBox.SelectedItem).Content                          ;
                            }
                        );
                        Win_Activator activator = new Win_Activator();
                        activator.ActWin(selectedContent);
                        IsWinActivated();
                    }
                );
                subThread.Start();
            }
            else if (actOffice_RadioButton.IsChecked == true)
            {
                //Thread subThread = new Thread
                //(
                //    () =>
                //    {
                        Thread.Sleep(1000);
                        string selectedContent = "anawaert.tech";
                        this.Dispatcher.Invoke
                        (
                            () =>
                            {
                                selectedContent = (string)((ComboBoxItem)selectServer_ComboBox.SelectedItem).Content == "Anawaert KMS 服务器" ?
                                                  "anawaert.tech" :
                                                  (string)((ComboBoxItem)selectServer_ComboBox.SelectedItem).Content;
                                Office_Activator activator = new Office_Activator();
                                activator.ActOffice(selectedContent);
                            }
                        );

                    //}
                //);
            }
        }

        private void autoRenew_CheckBox_Checked(object sender, RoutedEventArgs e)
        {
            //AutoRenewSign(autoRenew_CheckBox.IsChecked == true);
        }
    }
}
