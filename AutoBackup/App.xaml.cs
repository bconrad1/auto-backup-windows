using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

namespace AutoBackup
{
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            MainWindow landingView = new MainWindow();
            MainWindowViewModel context = new MainWindowViewModel();
            landingView.DataContext = context;
            landingView.Show();
        }
    }
}
