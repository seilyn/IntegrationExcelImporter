using IntegrationExcelImporter.Model;
using System;
using System.Collections.Generic;
using System.Data;
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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace IntegrationExcelImporter.View
{
    /// <summary>
    /// MainContentsGrid.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class MainContentsGrid : UserControl
    {
        public MainContentsGrid()
        {
            InitializeComponent();
            var excelManager = new ExcelManager();
            List<EduPlanGridInfo> eduPlanGridInfoList = excelManager.ReadExcelToEduPlanGridInfo();
            _dataGrid.ItemsSource = eduPlanGridInfoList;
        }
    }
}
