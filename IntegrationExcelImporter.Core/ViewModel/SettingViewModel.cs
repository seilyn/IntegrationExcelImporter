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

        public ICommand OpenFolderDialogCommand { get; set; }
        public ICommand SaveOptionsCommand { get; set; }


        public SettingViewModel()
        {
            OpenFolderDialogCommand = new RelayCommand<object>(ExecuteOpenFolder, CanOpenFolder);
            SaveOptionsCommand = new RelayCommand<object>(ExecuteSaveOptions, CanSaveOptions);
            SortOptionsDictionary = new Dictionary<Option, string>();
            foreach (var item in System.Enum.GetValues(typeof(Option)))
            {
                SortOptionsDictionary.Add((Option)item, ((Option)item).GetDescription());
            }
            InitOptions();
            
            
        }

        private void ExecuteSaveOptions(object obj)
        {
            bool saveFlag = SQLiteManager.Instance.SaveOptions(SortOptions, SearchKeywords, SaveFilePath);

            if (saveFlag)
            {
                /// TODO :
                /// 메시지박스는 추후 커스텀, 바인딩된 메시지박스를 사용함.
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
                SelectedPath = SaveFilePath
            };

            if (dialog.ShowDialog() == DialogResult.OK)
            {
                SaveFilePath = dialog.SelectedPath;
            }
        }

        private bool CanOpenFolder(object obj)
        {
            return true;
        }

        private void InitOptions()
        {
            DataTable dataTable = SQLiteManager.Instance.SelectOptions();

            if (dataTable.Rows.Count > 0)
            {
                DataRow row = dataTable.Rows[0];

                //SortOptions = row["SORT_OPTION"].ToString();
                SearchKeywords = row["SEARCH_EXCELSHEETS_KEYWORD"].ToString();
                SaveFilePath = row["SAVE_FILE_PATH"].ToString();
            }
        }

     
    }
}
