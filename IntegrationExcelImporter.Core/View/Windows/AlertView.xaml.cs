using IntegrationExcelImporter.Core.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace IntegrationExcelImporter.Core.View.Windows
{
    /// <summary>
    /// AlertView.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class AlertView : Window
    {
       
        public AlertView(string type, string text)
        {
            InitializeComponent();
            AlertViewModel alertViewModel = new AlertViewModel(type, text);
            alertViewModel.OnRequestClose += () => Close();
            DataContext = alertViewModel;
        }


        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}
