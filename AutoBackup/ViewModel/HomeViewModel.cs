using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using System.Windows.Forms;
using System.Threading;
using System.Windows.Media;

namespace AutoBackup
{
    public class HomeViewModel : ObservableObject
    {
  
        public ObservableCollection<ZipFileModel> FileSources { get; } = new ObservableCollection<ZipFileModel>();
        public string CurrentSource { get; set; }
        public string CurrentDestination { get; set; }

        private string _notifyText;
        public string NotifyText
        {
            get
            {
                return _notifyText;
            }
            set
            {
                 _notifyText = value;
                OnPropertyChanged("NotifyText");
            }
        }

        private string _fileDestination = "PLASE ADD A DESTINATION";
        public string FileDestination
        {
            get
            {
                return _fileDestination;
            }
            set
            {
                _fileDestination = value;
                OnPropertyChanged("FileDestination");
            }
        }

        private SolidColorBrush _canCopyBackground;
        public SolidColorBrush CanCopyBackground
        {
            get
            {
                return _canCopyBackground;
            }
            set
            {
                _canCopyBackground = value;
                OnPropertyChanged("CanCopyBackground");
            }
        }

        HomeViewModel parent;
        public HomeViewModel()
        {
            parent = this;
            CanCopyBackground = Brushes.Gray;
        }


        #region Button Commands
        public ICommand _addFile;
        public ICommand AddFile
        {
            get
            {
                _addFile = new RelayCommand(
                    param => AddFileCommand(),
                    param => true);
                return _addFile;
            }
        }
        public void AddFileCommand()
        {
            string source = GetFolderDestinationHandler();
            parent.CurrentSource = source;

            if (!parent.FileSources.Any(i => i.Location == parent.CurrentSource) && !String.IsNullOrEmpty(source))
            {                
                ZipFileModel newFolder = (new ZipFileModel(parent.CurrentSource));
                parent.FileSources.Add(newFolder);
                parent.NotifyText = null;
            }
            else
            {
                string errorText;
                if (String.IsNullOrEmpty(source))
                {
                    errorText = "Please select a folder";
                }
                else
                {
                    errorText = "That item already exists";
                }
                
                parent.NotifyText = errorText.ToUpper();
                parent.CurrentSource = null;
            }
            setCopyBtnBg(parent);
        }

        public ICommand _clearFiles;
        public ICommand ClearFiles
        {
            get
            {
                _clearFiles = new RelayCommand(
                    param => ClearFilesCommand(),
                    param => true
                    );
                return _clearFiles;
            }
        }
        public void ClearFilesCommand()
        {
            parent.FileSources.Clear();
            parent.NotifyText = null;
            setCopyBtnBg(parent);
        }

        public ICommand _removeFile;
        public ICommand RemoveFile
        {
            get
            {
                _removeFile = new RelayCommand(
                     param => RemoveFileCommand(param),
                     param => true);
                return _removeFile;
            }
        }
        public void RemoveFileCommand(object param)
        {
           
            ObservableCollection<ZipFileModel> FileSources = parent.FileSources;
            ZipFileModel currentFile = (ZipFileModel)param;
            FileSources.Remove(FileSources.Where(i => i.Location == currentFile.Location).First());
            setCopyBtnBg(parent);
        }

        public ICommand _setDestination;
        public ICommand SetDestination {
        get
            {
                _setDestination = new RelayCommand(
                    param => SetDestinationCommand(),
                    param => true
                    );
                return _setDestination;
            }
        }
        public void SetDestinationCommand()
        {
            string destination = GetFolderDestinationHandler();
            if (!String.IsNullOrEmpty(destination))
            {
                parent.CurrentDestination = destination;
                parent.FileDestination = parent.CurrentDestination;
                setCopyBtnBg(parent);
            }
            else
            {
                parent.CurrentDestination = "THAT IS NOT A VALID DESTINATION";
            }
        }

        public ICommand _copyFiles;
        public ICommand CopyFiles
        {
            get
            {
                _copyFiles = new RelayCommand(
                    param => CopyFilesCommand(),
                    param => (parent.FileSources.Count > 0 && !String.IsNullOrEmpty(parent.CurrentDestination))
                    );
                return _copyFiles;
            }
        }
        public void CopyFilesCommand()
        {
            FileCopier copier = new FileCopier(parent.CurrentDestination, parent.FileSources);
            Thread thread = new Thread(new ThreadStart(copier.CopyFiles));
            thread.Start();
       
        }

        protected void setCopyBtnBg(HomeViewModel parent)
        {
            SolidColorBrush brushColor = (FileSources.Count > 0 && !String.IsNullOrEmpty(parent.CurrentDestination)) ? Brushes.Green : Brushes.Gray;
            parent.CanCopyBackground = brushColor;
        }

        static protected string GetFolderDestinationHandler()
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
