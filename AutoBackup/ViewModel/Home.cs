using System;
using System.Collections;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Windows.Forms;

namespace AutoBackup
{
    public class Home : INotifyPropertyChanged
    {
        public string notifyText;

        public string CurrentSource { get; set; }
        public string NotifyText
        {
            get
            {
                return notifyText;
            }
            set
            {
                notifyText = value;
                OnPropertyChanged();
            }
        }
        public ObservableCollection<FileLocationModel> FileSources { get; } = new ObservableCollection<FileLocationModel>();

        public Home()
        {
            AddFile = new AddFileCommand(this);
            ClearFiles = new ClearFilesCommand(this);
            RemoveFile = new RemoveFileCommand(this);
        }

        public ICommand AddFile { get; private set; }
        public ICommand ClearFiles { get; private set; }
        public ICommand RemoveFile { get; private set; }

        class AddFileCommand : ICommand
        {
            Home parent;
            public AddFileCommand(Home parent)
            {
                this.parent = parent;
                parent.PropertyChanged += delegate { CanExecuteChanged?.Invoke(this, EventArgs.Empty); };
            }

            public event EventHandler CanExecuteChanged;
            public bool CanExecute(object param) { return true; }
            public void Execute(object param)
            {
                getFolderDestinationHandler();
                if (!parent.FileSources.Any(i => i.Location == parent.CurrentSource))
                {
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

            void getFolderDestinationHandler()
            {
                Console.WriteLine("OpeningFile");
                var myDialog = new FolderBrowserDialog();

                if (string.IsNullOrEmpty(parent.CurrentSource))
                {
                    myDialog.ShowDialog();
                    parent.CurrentSource = myDialog.SelectedPath;
                }

                return;
            }
        }

        class ClearFilesCommand : ICommand
        {
            Home parent;
            public ClearFilesCommand(Home parent)
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
            Home parent;
            public RemoveFileCommand(Home parent)
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

        public event PropertyChangedEventHandler PropertyChanged;

        void OnPropertyChanged([CallerMemberName]string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }


    }


}
