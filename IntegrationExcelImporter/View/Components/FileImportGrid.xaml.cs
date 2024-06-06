using IntegrationExcelImporter.ViewModel;
using System.Windows;
using System.Windows.Controls;

namespace IntegrationExcelImporter.View
{
    /// <summary>
    /// FileImportGrid.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class FileImportGrid : UserControl
    {
        public FileImportGrid()
        {
            InitializeComponent();
        }

        private void DataGrid_LoadingRow(object sender, DataGridRowEventArgs e)
        {
            e.Row.Header = (e.Row.GetIndex() + 1).ToString();
        }
    }
}
