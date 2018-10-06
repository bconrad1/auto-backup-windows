using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using System.Windows.Forms;
using System.Threading;

namespace AutoBackup
{
    public class HomeViewModel : ObservableObject
    {
  
        public ObservableCollection<ZipFileModel> FileSources { get; } = new ObservableCollection<ZipFileModel>();
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

        public HomeViewModel parent;
        public HomeViewModel()
        {
            parent = this;
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
                string errorText = "That item already exists";       
                parent.NotifyText = errorText.ToUpper();
                parent.CurrentSource = null;
            }
    
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
                    param => parent.FileSources.Count > 0
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
