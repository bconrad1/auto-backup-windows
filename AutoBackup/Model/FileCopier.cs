using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutoBackup
{
    class FileCopier : ObservableObject
    {
        enum CopyStatusEnum { unstarted = 1, started = 2, finished = 3, error = 4 };

        private string _destination;
        private ICollection<ZipFileModel>_folders;
        public FileCopier(string Destination, ICollection<ZipFileModel> Folders) 
        {
            _destination = Destination;
           _folders = Folders;
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
            foreach (ZipFileModel folder in _folders)
            {
                string folderLocation = folder.Location;
                string folderLocationStripped = GetFolderOrFileName(folderLocation);
                string folderCopyLocation = System.IO.Path.Combine(destination, folderLocationStripped);
                try
                {
                    string startPath = folderLocation;
                    string zipPath = folderCopyLocation + ".zip";

                    if (System.IO.File.Exists(zipPath))
                    {
                        File.Delete(zipPath);
                        ZipFile.CreateFromDirectory(startPath, zipPath);
                    }
                    else
                    {
                        ZipFile.CreateFromDirectory(startPath, zipPath);
                    }

                    folder.CopyStatus = (int) CopyStatusEnum.finished;

                }
                catch(Exception ex)
                {
                    folder.CopyStatus = (int) CopyStatusEnum.error;
                    Console.WriteLine(ex);
                }
            
            }
        }

        private string GetFolderOrFileName(string folderPath)
        {
            String[] pathSplit = folderPath.Replace(@"\\", @"\").Split('\\');
            return pathSplit[pathSplit.Length - 1];
        }

        private int getFileCount(string folderLocation)
        {
            int fileCount = 0;
            try
            {
                var files = System.IO.Directory.EnumerateFiles(folderLocation, "*", System.IO.SearchOption.AllDirectories);
                foreach (string file in files)
                {
                    fileCount += 1;
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }

            return fileCount;
        }


    }
}
