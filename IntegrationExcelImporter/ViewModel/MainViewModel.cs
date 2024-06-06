using IntegrationExcelImporter.Common;
using IntegrationExcelImporter.Model;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace IntegrationExcelImporter.ViewModel
{
    public class MainViewModel : ObservableObjectBase<MainViewModel>
    {
        public event PropertyChangedEventHandler PropertyChanged;
        ExcelManager excelManager = new ExcelManager();

        private ObservableCollection<Model.Files> _files;
        public ObservableCollection<Model.Files> Files
        {
            get
            {
                if (_files == null)
                {
                    _files = new ObservableCollection<Model.Files>();
                }
                return _files;
            }
            set
            {
                _files = value;
                OnPropertyChanged(p => p.Files);
            }
        }

        private ObservableCollection<Plan> _eduPlanGridInfoList;
        public ObservableCollection<Plan> EduPlanGridInfoList
        {
            get => _eduPlanGridInfoList;
            set
            {
                _eduPlanGridInfoList = value;
                OnPropertyChanged(p => p.EduPlanGridInfoList);
            }
        }

        public ICommand OpenFileCommand { get; set; }
        public ICommand SelectFileCommand { get; set; }
        public ICommand FileImportGridLoadingRowCommand { get; set; }
        public ICommand ApplicationCloseCommand { get; set; }
        public ICommand WindowsMaximizeCommand { get; set; }
        public ICommand WindowsMinimizeCommand { get; set; }

        private Model.Files _selectedFile;
        public Model.Files SelectedFile
        {
            get => _selectedFile;
            set
            {
                if (_selectedFile != value)
                {
                    _selectedFile = value;
                    OnPropertyChanged(p => p.SelectedFile);
                }
            }
        }

        public MainViewModel()
        {
            OpenFileCommand = new RelayCommand<object>(ExecuteOpenFileButton, CanOpenFileButton);
            SelectFileCommand = new RelayCommand<object>(ExecuteSelectFile, CanSelectFile);
            ApplicationCloseCommand = new RelayCommand<object>(ExecuteApplicationClose, CanApplicationClose);
            WindowsMaximizeCommand = new RelayCommand<object>(ExecuteWindowsMaximize, CanWindowsMaximize);
            WindowsMinimizeCommand = new RelayCommand<object>(ExecuteWindowsMinimize, CanWindowsMinimize);
            EduPlanGridInfoList = new ObservableCollection<Plan>();
            Application.Current.MainWindow.DragMove();
        }

        private void ExecuteWindowsMinimize(object obj)
        {
            Application.Current.MainWindow.WindowState = WindowState.Minimized;
        }

        private bool CanWindowsMinimize(object obj)
        {
            return true;
        }

        private void ExecuteWindowsMaximize(object obj)
        {
            Application.Current.MainWindow.WindowState = WindowState.Maximized;
        }

        private bool CanWindowsMaximize(object obj)
        {
            return true;
        }

        private void ExecuteApplicationClose(object obj)
        {
            Application.Current.Shutdown();
        }

        private bool CanApplicationClose(object obj)
        {
            return true;
        }

        private void ExecuteSelectFile(object obj)
        {
            if (SelectedFile != null)
            {
                List<Plan> eduPlanList = excelManager.ReadExcelToEduPlanGridInfo(SelectedFile.FilePath);
                EduPlanGridInfoList.Clear();

                foreach (var item in eduPlanList)
                {
                    EduPlanGridInfoList.Add(item);
                }
            }
        }

        private bool CanSelectFile(object obj)
        {
            return true;
        }

        private void ExecuteOpenFileButton(object obj)
        {
            Log.Info("FILE DIALOG OPENED");
            OpenFileDialog dialog = new OpenFileDialog
            {
                Multiselect = true,
                Filter = "Excel 파일 (*.xlsx)|*.xlsx",
                //InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.Desktop)
            };

            if (dialog.ShowDialog() == true)
            {
                foreach (string file in dialog.FileNames)
                {
                    var fileInfo = new Model.Files
                    {
                        FileName = Path.GetFileNameWithoutExtension(file),
                        FilePath = Path.GetFullPath(file),
                    };

                    Files.Add(fileInfo);
                }
            }
        }

        private bool CanOpenFileButton(object obj)
        {
            return true;
        }
    }
}
