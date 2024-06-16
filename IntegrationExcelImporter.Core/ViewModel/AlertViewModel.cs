using IntegrationExcelImporter.Common.Utility;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace IntegrationExcelImporter.Core.ViewModel
{
    public class AlertViewModel : ObservableObjectBase<AlertViewModel> 
    {
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

    }
}
