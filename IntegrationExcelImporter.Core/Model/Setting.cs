using IntegrationExcelImporter.Common.Utility;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IntegrationExcelImporter.Core.Model
{
    public class Setting : ObservableObjectBase<Setting>
    {
        private static readonly Setting _instance = new Setting();
        public static Setting Instance
        {
            get { return _instance; }
        }

        /// <summary>
        /// DB 경로
        /// </summary>
        private string _dbPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "manager_option.db");
        public string DbPath
        {
            get { return _dbPath; }
            set
            {
                _dbPath = value;
            }
        }

        private string _sortOptions;
        public string SortOptions
        {
            get { return _sortOptions; }
            set
            {
                _sortOptions = value;
                OnPropertyChanged(p => p.SortOptions);
            }
        }

        private string _searchKeywords;
        public string SearchKeywords
        {
            get => _searchKeywords;
            set
            {
                if (_searchKeywords != value)
                {
                    _searchKeywords = value;
                    OnPropertyChanged(p => p.SearchKeywords);
                }
            }
        }
        private string _saveFilePath;

        public string SaveFilePath
        {
            get => _saveFilePath;
            set
            {
                if (_saveFilePath != value)
                {
                    _saveFilePath = value;
                    OnPropertyChanged(p => p.SaveFilePath);
                }
            }
        }

        private string _isAutoLogin;
        public string IsAutoLogin
        {
            get => _isAutoLogin;
            set
            {
                if (_isAutoLogin != value)
                {
                    _isAutoLogin = value;
                    OnPropertyChanged(p => p.IsAutoLogin);
                }
            }
        }

        private string _saveFileName;

        public string SaveFileName
        {
            get => _saveFileName;
            set
            {
                if (_saveFileName != value)
                {
                    _saveFileName = value;
                    OnPropertyChanged(p => p.SaveFileName);
                }
            }
        }
    }
}
