using IntegrationExcelImporter.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IntegrationExcelImporter.Model
{
    public class FileProperty : ObservableObjectBase<FileProperty>
    {
        private static readonly FileProperty _instance = new FileProperty();
        public static FileProperty Instance
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

        /// <summary>
        /// 파일 확장자
        /// </summary>
        private string _fileExtension = string.Empty;
        public string FileExtension
        {
            get { return _fileExtension; }
            set
            {
                _fileExtension = value;
                OnPropertyChanged(p => p.FileExtension);
            }
        }

    }
}
