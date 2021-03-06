﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace AutoBackup
{
    public class ZipFileModel : FolderLocationModel
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

        private int _copyStatus;
        public int CopyStatus
        {
            get
            {
                return (int) _copyStatus;
            }
            set
            {
                _copyStatus = value;
                OnPropertyChanged("CopyStatus");
            }
        }
        public ZipFileModel(string location)
        {
            Location = location;
            DisplayText = location;
            DownloadProgress = 0;
            BrushColor = Brushes.White;
        }
        private SolidColorBrush _brushColor;
        public SolidColorBrush BrushColor
        {
            get
            {
                return _brushColor;
            }
            set
            {
                _brushColor = value;
                OnPropertyChanged("BrushColor");
            }
        }

      
    }
}
