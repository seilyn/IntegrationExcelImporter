using IntegrationExcelImporter.Common;
using IntegrationExcelImporter.Core.Init;
using IntegrationExcelImporter.Common.Utility;
using IntegrationExcelImporter.Core.ViewModel;
using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Threading;

namespace IntegrationExcelImporter.Core
{
    public partial class MainWindow : Window
    {
        MainViewModel mainViewModel = new MainViewModel();

        public MainWindow()
        {
            InitializeComponent();
            _ = new Initialize();
            
            DataContext = mainViewModel;
            mainViewModel.MergeCompleted += OnMergeCompleted;

            
            Log.Info("**************** PROGRAM START ****************");
        }

        private void OnMergeCompleted()
        {
            Dispatcher.Invoke(() =>
            {
                if (FindName("_gridTabControl") is TabControl tabControl)
                {
                    tabControl.SelectedIndex = 1;
                }
            });
        }

        private void Grid_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton == MouseButton.Left && e.ClickCount == 2)
            {
                // 현재 창의 상태 확인
                if (WindowState == WindowState.Maximized)
                {
                    // 이미 최대화된 경우 최대화 해제
                    WindowState = WindowState.Normal;
                }
                else
                {
                    // 최대화가 아닌 경우 최대화
                    mainViewModel.WindowsMaximizeCommand.Execute(null);
                }
            }
            else
            {
                DragMove();
            }
        }
    }
}
