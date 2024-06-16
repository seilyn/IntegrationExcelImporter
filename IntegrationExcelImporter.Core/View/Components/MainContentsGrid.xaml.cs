using System.Windows.Controls;

namespace IntegrationExcelImporter.Core.View.Components
{
    public partial class MainContentsGrid : UserControl
    {
        public MainContentsGrid()
        {
            InitializeComponent();

        }

        private void DataGrid_LoadingRow(object sender, DataGridRowEventArgs e)
        {
            e.Row.Header = (e.Row.GetIndex() + 1).ToString();

        }
    }
}
