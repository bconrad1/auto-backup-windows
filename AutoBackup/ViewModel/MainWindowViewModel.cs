using System.Collections.Generic;
using System.Windows;
using System.Windows.Input;

namespace AutoBackup
{
    class MainWindowViewModel : ObservableObject
    {
        public ObservableObject _currentPageViewModel;
        private List<ObservableObject> _pageViewModels;
        public MainWindowViewModel()
        {
            PageViewModels.Add(new HomeViewModel());
            CurrentPageViewModel = PageViewModels[0];

            WindowState = System.Windows.WindowState.Normal; 
        }

        public ObservableObject CurrentPageViewModel
        {
            get
            {
                return _currentPageViewModel;
            }
            set
            {
                if (_currentPageViewModel != value)
                {
                    _currentPageViewModel = value;
                    OnPropertyChanged("CurrentPageViewModel");
                }
            }
        }


        public List<ObservableObject> PageViewModels
        {
            get
            {
                if (_pageViewModels == null)
                    _pageViewModels = new List<ObservableObject>();

                return _pageViewModels;
            }
        } 

        public ICommand _minimizeApplication;
        public ICommand MinAppCommand
        {
            get
            {
                _minimizeApplication = new RelayCommand(
                    param  => MinimizeApplication(),
                    param => true);
                return _minimizeApplication;
            }    
        }
        public ICommand _closeApplication;
        public ICommand CloseAppCommand
        {
            get
            {
                _closeApplication = new RelayCommand(
                    param => CloseApplication(),
                    param => true);
                return _closeApplication;
            }
        }

        public void CloseApplication()
        {
            Application.Current.MainWindow.Close();
        }
        public void MinimizeApplication()
        {
            WindowState = System.Windows.WindowState.Minimized;
        }

        private System.Windows.WindowState _windowState;
        public System.Windows.WindowState WindowState
        {
            get { return _windowState; }
            set
            {
                _windowState = value;
                OnPropertyChanged("WindowState");
            }
        }
       
    }

}
