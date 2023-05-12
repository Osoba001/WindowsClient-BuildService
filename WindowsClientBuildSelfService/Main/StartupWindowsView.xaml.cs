using Microsoft.Extensions.DependencyInjection;
using System.Windows;

namespace WindowsClientBuildSelfService.Main
{
    /// <summary>
    /// Interaction logic for StartupWindowsView.xaml
    /// </summary>
    public partial class StartupWindowsView : Window
    {
        public StartupWindowsView()
        {
            InitializeComponent();
            DataContext = new StartupWindowsViewModel();
        }
    }
}
