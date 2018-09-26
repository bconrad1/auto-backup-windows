using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutoBackup
{
    public class FolderLocationModel : ObservableObject
    {
        public string Location { get; set; }
        private string _displayText;
        public string DisplayText {
            get {
                return _displayText;
            }
            set
            {
                _displayText = createDisplayText(value);
            }
        }

        public FolderLocationModel() { }
        public FolderLocationModel(string location, string displayText)
        {
            this.Location = location;
            this.DisplayText = createDisplayText(displayText);
        }

        private string createDisplayText(string location)
        {
            int stringLength = location.Length;
            int subStringLength = 30;
            return stringLength > subStringLength ? (location.Substring(0, subStringLength) + "...") : location;

        }
    }
}
