using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Windows.Media;

namespace AutoBackup
{
    class FileCopier : ObservableObject
    {
        public enum CopyStatusEnum { unstarted = 1, started = 2, finished = 3, error = 4 };

        private string _destination;
        private ICollection<ZipFileModel>_folders;
        private CopyStatusEnum _copyStatus;
        public FileCopier(string Destination, ICollection<ZipFileModel> Folders) 
        {
            _destination = Destination;
           _folders = Folders;
           _copyStatus = CopyStatusEnum.unstarted;
        }
        public CopyStatusEnum CopyStatus
        {
            get
            {
                return _copyStatus;
            }
            set
            {
                _copyStatus = value;
                OnPropertyChanged("CopyStatus");
            }
        }
        public void CopyFiles()
        {
            string destinationDirectory = CreateDirectory();
            MoveFiles(destinationDirectory);
        }

        private string CreateDirectory()
        {
            string destinationString = _destination;
            string currentDate = DateTime.Now.ToString("dddd-dd-MMMM-yyyy");
            string folderName = System.IO.Path.Combine(destinationString, String.Format("Backup_{0}", currentDate));
            if (!System.IO.Directory.Exists(folderName))
            {
                System.IO.Directory.CreateDirectory(folderName);
            }

            return folderName;
        }

        private void MoveFiles(string destination)
        {
            CopyStatus = CopyStatusEnum.started;
            foreach (ZipFileModel folder in _folders)
            {
                string folderLocationStripped = GetFolderOrFileName(folder.Location);
                string folderCopyLocation = Path.Combine(destination, folderLocationStripped);
                string startPath = folder.Location;
                string zipPath = String.Format("{0}\\{1}.zip",destination, folderLocationStripped);
                folder.CopyStatus = (int)CopyStatusEnum.started;
                folder.BrushColor = Brushes.Orange;

                try {
                    if (Directory.Exists(destination))
                    {
                        Directory.Delete(destination, true);
                    }
                    ZipFile.CreateFromDirectory(startPath, zipPath);
                    folder.CopyStatus = (int)CopyStatusEnum.finished;
                    folder.BrushColor = Brushes.Green;
                }
                catch (Exception ex)
                {
                    folder.CopyStatus = (int)CopyStatusEnum.error;
                    folder.BrushColor = Brushes.Red;
                    Console.WriteLine(ex);
                }    
            }
        }

        private string GetFolderOrFileName(string folderPath)
        {
            String[] pathSplit = folderPath.Replace(@"\\", @"\").Split('\\');
            return pathSplit[pathSplit.Length - 1];
        }
    }
}
