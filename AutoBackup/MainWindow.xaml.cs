using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace AutoBackup
{
     public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
          
        }

        protected override void OnMouseLeftButtonDown(MouseButtonEventArgs e)
        {
            base.OnMouseLeftButtonDown(e);
            this.DragMove();
        }


    }
}
