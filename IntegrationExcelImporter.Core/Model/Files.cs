using IntegrationExcelImporter.Common;
using IntegrationExcelImporter.Common.Utility;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IntegrationExcelImporter.Core.Model
{
    public class Files : ObservableObjectBase<Files>
    {
        private static readonly Files _instance = new Files();
        public static Files Instance
        {
            get { return _instance; }
        }
        /// <summary>
        /// 파일명
        /// </summary>
        private string _fileName = string.Empty;
        public string FileName
        {
            get { return _fileName; }
            set
            {
                _fileName = value;
                OnPropertyChanged(p => p.FileName);
            }
        }

        private string _filePath = string.Empty;
        public string FilePath
        {
            get { return _filePath; }
            set
            {
                _filePath = value;
                OnPropertyChanged(p => p.FilePath);
            }
        }

        private bool _isSelected = false;
        public bool IsSelected
        {
            get { return _isSelected; }
            set
            {
                _isSelected = value;
                OnPropertyChanged(p => p.IsSelected);
            }
        }
       

    }
}
