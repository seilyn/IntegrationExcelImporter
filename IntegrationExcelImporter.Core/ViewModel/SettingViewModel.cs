using IntegrationExcelImporter.Common.Utility;
using IntegrationExcelImporter.Core.DataAccess;
using IntegrationExcelImporter.Core.Model;
using System.Collections.Generic;
using System.Data;
using System.Windows.Forms;
using System.Windows.Input;

namespace IntegrationExcelImporter.Core.ViewModel
{
    public class SettingViewModel : ObservableObjectBase<SettingViewModel>
    {
        private Dictionary<Option, string> _sortOptionsDictionary;
        public Dictionary<Option, string> SortOptionsDictionary
        {
            get { return _sortOptionsDictionary; }
            set
            {
                _sortOptionsDictionary = value;
                OnPropertyChanged(p => p.SortOptionsDictionary);
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

        public ICommand OpenFolderDialogCommand { get; set; }
        public ICommand SaveOptionsCommand { get; set; }


        public SettingViewModel()
        {
            OpenFolderDialogCommand = new RelayCommand<object>(ExecuteOpenFolder, CanOpenFolder);
            SaveOptionsCommand = new RelayCommand<object>(ExecuteSaveOptions, CanSaveOptions);
            SortOptionsDictionary = new Dictionary<Option, string>();
            SetValues();
            foreach (var item in System.Enum.GetValues(typeof(Option)))
            {
                SortOptionsDictionary.Add((Option)item, ((Option)item).GetDescription());
            }
        }

        private void ExecuteSaveOptions(object obj)
        {
            bool saveFlag = SQLiteManager.Instance.SaveOptions(Setting.Instance.SortOptions, Setting.Instance.SearchKeywords, Setting.Instance.SaveFilePath);

            if (saveFlag)
            {

                MessageBox.Show("해당 설정값을 저장했습니다.");
            }
            else
            {
                MessageBox.Show("설정값 저장에 실패했습니다.");
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
            }
        }

        private bool CanOpenFolder(object obj)
        {
            return true;
        }

        private void SetValues()
        {
            SearchKeywords = Setting.Instance.SearchKeywords;
            SaveFilePath = Setting.Instance.SaveFilePath;
            SortOptions = Setting.Instance.SortOptions;
            IsAutoLogin = Setting.Instance.IsAutoLogin;
        }
      

     
    }
}
