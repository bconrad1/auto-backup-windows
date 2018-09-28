using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutoBackup
{
    class FileCopier : ObservableObject
    {
        private string _destination;
        private ICollection<FileLocationModel>_folders;
        public FileCopier(string Destination, ICollection<FileLocationModel> Folders) 
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
            foreach (FileLocationModel folder in _folders)
            {
                string folderLocation = folder.Location;
                string folderLocationStripped = GetFolderName(folderLocation);
                string folderCopyLocation = System.IO.Path.Combine(destination, folderLocationStripped);
                try
                {
                    System.IO.Directory.CreateDirectory(folderCopyLocation);
                    int fileCount = getFileCount(folderLocation);
                    var files = System.IO.Directory.EnumerateFiles(folderLocation, "*", System.IO.SearchOption.AllDirectories);
                    foreach (string file in files)
                    {
                    //    File.Copy(file, folderCopyLocation);
                    //    float copyPercentage = 1 / fileCount * 100;
                    //    folder.DownloadProgress += (int) Math.Floor(copyPercentage);
                    }
       

                }
                catch(Exception ex)
                {
                    Console.WriteLine(ex);
                }
            
            }
        }

        private string GetFolderName(string folderPath)
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
