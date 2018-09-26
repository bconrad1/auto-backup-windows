using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutoBackup
{
    public class FileLocationModel : FolderLocationModel
    {
        private int _downloadProgress;
        public int DownloadProgress {
            get
            {
                return _downloadProgress;
            }
            set
            {
                _downloadProgress = value;
                OnPropertyChanged("DownloadProgress");
            }
        }
        public FileLocationModel(string location)
        {
            Location = location;
            DisplayText = location;
            DownloadProgress = 0;
        }

      
    }
}
