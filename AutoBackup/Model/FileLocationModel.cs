using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutoBackup
{
    public class FileLocationModel
    {
        public string Location { get; private set; }
        public string DisplayText { get; private set; }
        public float DownloadProgress { get; set; }
        public FileLocationModel(string location)
        {
            this.Location = location;
            this.DisplayText = createDisplayText(location);
            this.DownloadProgress = 1;
        }

        private string createDisplayText(string location)
        {
            int stringLength = location.Length;
            int subStringLength = 30;
            return stringLength > subStringLength ? (location.Substring(0, subStringLength) + "...") : location;

        }
    }
}
