using IntegrationExcelImporter.Common;
using IntegrationExcelImporter.Core.Init;
using IntegrationExcelImporter.Common.Utility;
using IntegrationExcelImporter.Core.ViewModel;
using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Threading;
using System.Windows.Forms;
using System.Drawing;
using System.IO;

namespace IntegrationExcelImporter.Core
{
    public partial class MainWindow : Window
    {
        MainViewModel mainViewModel = new MainViewModel();
        private NotifyIcon _notifyIcon;

        public MainWindow()
        {
            InitializeComponent();
            _ = new Initialize();
            CreateNotifyIcon();
            
            DataContext = mainViewModel;
            mainViewModel.MergeCompleted += OnMergeCompleted;

            
            Log.Info("**************** PROGRAM START ****************");
        }

        private void CreateNotifyIcon()
        {
            if (File.Exists("./Icon/program_icon.ico"))
            {
                _notifyIcon = new NotifyIcon();
                _notifyIcon.Icon = new Icon(System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "program_icon.ico"));
                _notifyIcon.Visible = true;
                _notifyIcon.Text = "IntegrationExcelImporter";
                _notifyIcon.DoubleClick += Show_Click;
            }

           
        }

        private void OnMergeCompleted()
        {
            Dispatcher.Invoke(() =>
            {
                if (FindName("_gridTabControl") is System.Windows.Controls.TabControl tabControl)
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

        private void Show_Click(object sender, EventArgs e)
        {
            if (this.WindowState == System.Windows.WindowState.Minimized)
            {
                this.WindowState = System.Windows.WindowState.Normal;
            }
        }
    }
}
