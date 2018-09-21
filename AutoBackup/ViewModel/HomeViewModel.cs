using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using System.Windows.Forms;

namespace AutoBackup
{
    public class HomeViewModel : ObservableObject, INotifyPropertyChanged
    {
  
        public ObservableCollection<FileLocationModel> FileSources { get; } = new ObservableCollection<FileLocationModel>();
        public string CurrentSource { get; set; }
        public string CurrentDestination { get; set; }

        public string notifyText;
        public string NotifyText
        {
            get
            {
                return notifyText;
            }
            set
            {
                notifyText = value;
                OnPropertyChanged("NotifyText");
            }
        }

        public string fileDestination = "PLASE ADD A DESTINATION";
        public string FileDestination
        {
            get
            {
                return fileDestination;
            }
            set
            {
                fileDestination = value;
                OnPropertyChanged("FileDestination");
            }
        }


        public HomeViewModel()
        {
            AddFile = new AddFileCommand(this);
            ClearFiles = new ClearFilesCommand(this);
            RemoveFile = new RemoveFileCommand(this);
            SetDestination = new SetDestinationCommand(this);
        }


        #region Button Commands

        public ICommand AddFile { get; private set; }
        public ICommand ClearFiles { get; private set; }
        public ICommand RemoveFile { get; private set; }
        public ICommand SetDestination { get; private set; }

        class AddFileCommand : ICommand
        {
            HomeViewModel parent;
            public AddFileCommand(HomeViewModel parent)
            {
                this.parent = parent;
                parent.PropertyChanged += delegate { CanExecuteChanged?.Invoke(this, EventArgs.Empty); };
            }

            public event EventHandler CanExecuteChanged;
            public bool CanExecute(object param) { return true; }
            public void Execute(object param)
            {
                string source = getFolderDestinationHandler();
        
                if (!parent.FileSources.Any(i => i.Location == parent.CurrentSource) && !String.IsNullOrEmpty(source))
                {
                    parent.CurrentSource = source;
                    parent.FileSources.Add(new FileLocationModel(parent.CurrentSource));
                    parent.NotifyText = null;
                }
                else
                {
                    string errorText = "That item already exists";
                    parent.NotifyText = errorText.ToUpper();
                }
                parent.CurrentSource = null;
            }        
        }

        class ClearFilesCommand : ICommand
        {
            HomeViewModel parent;
            public ClearFilesCommand(HomeViewModel parent)
            {
                this.parent = parent;
                parent.PropertyChanged += delegate { CanExecuteChanged?.Invoke(this, EventArgs.Empty); };
            }

            public event EventHandler CanExecuteChanged;
            public bool CanExecute(object param) { return true; }
            public void Execute(object param)
            {
                parent.FileSources.Clear();
            }
        }

        class RemoveFileCommand : ICommand
        {
            HomeViewModel parent;
            public RemoveFileCommand(HomeViewModel parent)
            {
                this.parent = parent;
                parent.PropertyChanged += delegate { CanExecuteChanged?.Invoke(this, EventArgs.Empty); };
            }

            public event EventHandler CanExecuteChanged;
            public bool CanExecute(object param) { return true; }
            public void Execute(object param)
            {
                ObservableCollection<FileLocationModel> FileSources = parent.FileSources;
                FileLocationModel currentFile = (FileLocationModel)param;
                FileSources.Remove(FileSources.Where(i => i.Location == currentFile.Location).First());
            }
        }

        class SetDestinationCommand : ICommand {
            HomeViewModel parent;
            public SetDestinationCommand(HomeViewModel parent)
            {
                this.parent = parent;
                parent.PropertyChanged += delegate { CanExecuteChanged?.Invoke(this, EventArgs.Empty); };
            }

            public event EventHandler CanExecuteChanged;
            public bool CanExecute(object param) { return true; }
            public void Execute(object param)
            {
                string destination = getFolderDestinationHandler();
                if (!String.IsNullOrEmpty(destination))
                {
                    parent.CurrentDestination = destination;
                    parent.FileDestination = parent.CurrentDestination;
                }
                else
                {
                    parent.CurrentDestination = "THAT IS NOT A VALID DESTINATION";
                }
                

            }
        }

        static protected string getFolderDestinationHandler()
        {
            var myDialog = new FolderBrowserDialog();
            myDialog.ShowDialog();
            if (!String.IsNullOrEmpty(myDialog.SelectedPath))
            {        
                return myDialog.SelectedPath;
            }

            return String.Empty;
        }
        #endregion
    }


}
