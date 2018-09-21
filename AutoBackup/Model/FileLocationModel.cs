using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutoBackup
{
    public class FileLocationModel : FolderLocationModel
    {
        public float DownloadProgress { get; set; }
        public FileLocationModel(string location)
        {
            this.Location = location;
            this.DisplayText = location;
            this.DownloadProgress = 1;
        }

      
    }
}
