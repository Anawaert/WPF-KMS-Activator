using Activator.ServiceInterfaces;
using Activator.ViewModels;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Wpf.Ui;

namespace Activator.Views
{
    /// <summary>
    /// OnlineActivationPage.xaml 的交互逻辑
    /// </summary>
    public partial class OnlineActivationPage : Page
    {
        public OnlineActivationPage(OnlineActivationViewModel onlineActivationViewModel)
        {
            InitializeComponent();
            this.DataContext = onlineActivationViewModel;
        }

        public OnlineActivationPage() : this(App.ServiceProvider.GetRequiredService<OnlineActivationViewModel>()) { }
    }
}
