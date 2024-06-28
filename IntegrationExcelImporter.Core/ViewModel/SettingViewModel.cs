using IntegrationExcelImporter.Common.Utility;
using IntegrationExcelImporter.Core.DataAccess;
using IntegrationExcelImporter.Core.Model;
using IntegrationExcelImporter.Core.View.Windows;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Windows.Forms;
using System.Windows.Input;

namespace IntegrationExcelImporter.Core.ViewModel
{
    public class SettingViewModel : ObservableObjectBase<SettingViewModel>
    {

        private string _sortOptions;
        public string SortOptions
        {
            get { return _sortOptions; }
            set
            {
                _sortOptions = value;
                OnPropertyChanged(p => p.SortOptions);
                Setting.Instance.SortOptions = _sortOptions;
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
                    Setting.Instance.SearchKeywords = _searchKeywords;
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
                    Setting.Instance.SaveFilePath = _saveFilePath;
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
                    Setting.Instance.IsAutoLogin = _isAutoLogin;
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
                    Setting.Instance.SaveFileName = _saveFileName;
                }
            }
        }

        private void SetValues()
        {
            SearchKeywords = Setting.Instance.SearchKeywords;
            SaveFilePath = Setting.Instance.SaveFilePath;
            SortOptions = Setting.Instance.SortOptions;
            IsAutoLogin = Setting.Instance.IsAutoLogin;
            SaveFileName = Setting.Instance.SaveFileName;
        }

        public ICommand OpenFolderDialogCommand { get; set; }
        public ICommand SaveOptionsCommand { get; set; }


        public SettingViewModel()
        {
            OpenFolderDialogCommand = new RelayCommand<object>(ExecuteOpenFolder, CanOpenFolder);
            SaveOptionsCommand = new RelayCommand<object>(ExecuteSaveOptions, CanSaveOptions);
            SetValues();
           
        }

        private void ExecuteSaveOptions(object obj)
        {
            bool saveFlag = SQLiteManager.Instance.SaveOptions(SortOptions, SearchKeywords, SaveFilePath, SaveFileName);

            if (saveFlag)
            {
                AlertView alert = new AlertView("info", "설정값 저장에 성공했습니다.");
                alert.ShowDialog();
            }
            else
            {
                AlertView alert = new AlertView("error", "설정값 저장에 실패했습니다.");
                alert.ShowDialog();
            }
        }

        private bool CanSaveOptions(object obj)
        {
            return true;
        }

        private void ExecuteOpenFolder(object obj)
        {
            FolderBrowserDialog dialog = new FolderBrowserDialog
            {
                SelectedPath = Setting.Instance.SaveFilePath
            };

            if (dialog.ShowDialog() == DialogResult.OK)
            {
                Setting.Instance.SaveFilePath = dialog.SelectedPath;
                SaveFilePath = dialog.SelectedPath;
            }
        }

        private bool CanOpenFolder(object obj)
        {
            return true;
        }

      
    }
}
