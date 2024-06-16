using IntegrationExcelImporter.Common.Utility;
using System.Collections.Generic;
using System.Windows;
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

        public ICommand CloseSettingWindowCommand { get; set; }

        public SettingViewModel()
        {
            CloseSettingWindowCommand = new RelayCommand<object>(ExecuteCloseSettingWindow, CanCloseSettingWindow);
            SortOptionsDictionary = new Dictionary<Option, string>();
            foreach (var item in Option.GetValues(typeof(Option)))
            {
                SortOptionsDictionary.Add((Option)item, ((Option)item).GetDescription());
            }
        }

        private void ExecuteCloseSettingWindow(object obj)
        {
            // obj를 Window 타입으로 캐스팅하여 모달 창의 인스턴스를 얻습니다.
            var window = obj as System.Windows.Window;

            // 윈도우가 null이 아니고 닫히지 않았다면 모달 창을 닫습니다.
            if (window != null && window.DialogResult != true)
            {
                // DialogResult를 true로 설정하여 모달 창이 닫혔음을 나타냅니다.
                window.DialogResult = true;

                // 창을 닫습니다.
                window.Close();
            }
        }

        private bool CanCloseSettingWindow(object obj)
        {
            return true;
        }
    }
}
