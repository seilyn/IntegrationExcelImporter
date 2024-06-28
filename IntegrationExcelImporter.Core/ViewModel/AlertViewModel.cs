using IntegrationExcelImporter.Common.Utility;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Windows.Threading;

namespace IntegrationExcelImporter.Core.ViewModel
{
    public class AlertViewModel : ObservableObjectBase<AlertViewModel>
    {
        private DispatcherTimer _timer;

        private string _text;
        private string _type;

        public string Text
        {
            get => _text;
            set
            {
                _text = value;
                OnPropertyChanged(p => p.Text);
            }
        }

        public string Type
        {
            get => _type;
            set
            {
                _type = value;
                OnPropertyChanged(p => p.Type);
            }
        }

        public AlertViewModel(string type, string text)
        {
            Type = type;
            Text = text;


            // 타이머 초기화 및 설정
            _timer = new DispatcherTimer();
            _timer.Interval = TimeSpan.FromSeconds(3); 
            _timer.Tick += Timer_Tick;
            _timer.Start();
        }

        private void Timer_Tick(object sender, EventArgs e)
        {
            // 타이머 이벤트에서 실행될 메서드
            Close();
        }

        private void Close()
        {
            // 타이머 중지
            _timer.Stop();


            // 창 닫기
            OnRequestClose?.Invoke();
        }

        // 이벤트를 통해 창 닫기 요청 전달
        public event Action OnRequestClose;


    }
}
