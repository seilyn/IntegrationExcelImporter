using IntegrationExcelImporter.Core.DataAccess;
using IntegrationExcelImporter.Common.Utility;
using IntegrationExcelImporter.Core.DataAccess;
using IntegrationExcelImporter.Core.Model;
using IntegrationExcelImporter.Core.View;
using IntegrationExcelImporter.Core.View.Windows;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Threading;

using System.Windows;
using System.Windows.Input;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using IntegrationExcelImporter.Common.Constant;
using System.Reflection;

namespace IntegrationExcelImporter.Core.ViewModel
{
    public class MainViewModel : ObservableObjectBase<MainViewModel>
    {
        public event PropertyChangedEventHandler PropertyChanged;
        public event Action MergeCompleted;

        private ObservableCollection<Files> _files;
        public ObservableCollection<Files> Files
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

        private ObservableCollection<Plan> _mergedEduPlanGridList;
        public ObservableCollection<Plan> MergedEduPlanGridList
        {
            get => _mergedEduPlanGridList;
            set
            {
                _mergedEduPlanGridList = value;
                OnPropertyChanged(p => p.MergedEduPlanGridList);
            }
        }

        private double _progress;
        public double Progress
        {
            get => _progress;
            set
            {
                _progress = value;
                OnPropertyChanged(p => p.Progress);
            }
        }

        private string _currentFileName;
        public string CurrentFileName
        {
            get => _currentFileName;
            set
            {
                _currentFileName = value;
                OnPropertyChanged(p => p.CurrentFileName);
            }
        }

        private string _assemblyVersion;
        public string AssemblyVersion
        {
            get => _assemblyVersion;
            set
            {
                if (_assemblyVersion != value)
                {
                    _assemblyVersion = value;
                    OnPropertyChanged(nameof(AssemblyVersion));
                }
            }
        }

        public ICommand OpenFileCommand { get; set; }
        public ICommand SelectFileCommand { get; set; }
        public ICommand FileImportGridLoadingRowCommand { get; set; }
        public ICommand ApplicationCloseCommand { get; set; }
        public ICommand WindowsMaximizeCommand { get; set; }
        public ICommand WindowsMinimizeCommand { get; set; }
        public ICommand OpenSettingWindowCommand { get; set; }
        public ICommand MergeFileCommand { get; set; }
        public ICommand CreateExcelFileCommand { get; set; }

        private Files _selectedFile;
        public Files SelectedFile
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
            AssemblyVersion = GetAssemblyVersion();
            OpenFileCommand = new RelayCommand<object>(ExecuteOpenFileButton, CanOpenFileButton);
            SelectFileCommand = new RelayCommand<object>(ExecuteSelectFile, CanSelectFile);
            ApplicationCloseCommand = new RelayCommand<object>(ExecuteApplicationClose, CanApplicationClose);
            WindowsMaximizeCommand = new RelayCommand<object>(ExecuteWindowsMaximize, CanWindowsMaximize);
            WindowsMinimizeCommand = new RelayCommand<object>(ExecuteWindowsMinimize, CanWindowsMinimize);
            OpenSettingWindowCommand = new RelayCommand<object>(ExecuteOpenSettingWindow, CanOpenSettingWindow);
            MergeFileCommand = new RelayCommand<object>(ExecuteMergeFile, CanExecuteMergeFile);
            CreateExcelFileCommand = new RelayCommand<object>(ExecuteCreateExcelFile, CanCreateExcelFile);
            EduPlanGridInfoList = new ObservableCollection<Plan>();
            MergedEduPlanGridList = new ObservableCollection<Plan>();
            
        }

        private string GetAssemblyVersion()
        {
            var version = Assembly.GetExecutingAssembly().GetName().Version;
            return version != null ? version.ToString() : "Version not found";
        }

        private void ExecuteCreateExcelFile(object obj)
        {
            string filePath = Setting.Instance.SaveFilePath + "/" + Setting.Instance.SaveFileName + ".xlsx";

            if (File.Exists(filePath))
            {
                AlertView alert = new AlertView(GlobalConstantText.ALERT_TYPE_ERROR, GlobalErrorMessage.DUPLICATE_FILENAME_EXCEPTION);
                alert.ShowDialog();
                return;
            }
            else
            {
                if (ExcelManager.Instance.WriteMergedDataToNewSheet(MergedEduPlanGridList, filePath))
                {
                    AlertView alert = new AlertView(GlobalConstantText.ALERT_TYPE_INFO, GlobalSuccessMessage.SUCCESS_CREATE_MERGED_EXCELDATA);
                    alert.ShowDialog();

                }
                else
                {
                    AlertView alert = new AlertView(GlobalConstantText.ALERT_TYPE_INFO, GlobalSuccessMessage.FAILURE_CREATE_MERGED_EXCELDATA);
                    alert.ShowDialog();
                    return;
                }
          
                //LoadingSpinner spinner = new LoadingSpinner();
                //spinner.Show();

                //bool isSuccess = false;

                //try
                //{
                //    isSuccess = await Task.Run(() => ExcelManager.Instance.WriteMergedDataToNewSheet(MergedEduPlanGridList, filePath));
                //}
                //finally
                //{
                //    // Dispatcher를 사용하여 UI 스레드에서 spinner.Close()를 호출
                //    Application.Current.Dispatcher.Invoke(() => spinner.Close());
                //}

                //// Dispatcher를 사용하여 UI 스레드에서 AlertView를 표시
                //Application.Current.Dispatcher.Invoke(() =>
                //{
                //    if (isSuccess)
                //    {
                //        AlertView alert = new AlertView(GlobalConstantText.ALERT_TYPE_INFO, GlobalSuccessMessage.SUCCESS_CREATE_MERGED_EXCELDATA);
                //        alert.ShowDialog();
                //    }
                //});
            }
        }



        private bool CanCreateExcelFile(object obj)
        {
            return true;
        }

        private async void ExecuteMergeFile(object obj)
        {
            if (Files.Count == 0)
            {
                AlertView alert = new AlertView("warning", "병합할 데이터가 없습니다.");
                alert.ShowDialog();
                return;
            }

            var loadingView = new LoadingView
            {
                DataContext = this
            };

            loadingView.Show();

            try
            {
                List<Plan> mergedPlanList = await Task.Run(() => ExcelManager.Instance.MergeAllFileData(Files));
                HashSet<Plan> uniqueHashSet = new HashSet<Plan>(MergedEduPlanGridList);

                int totalFiles = Files.Count;
                int processedFiles = 0;

                

                foreach (var file in Files)
                {
                    CurrentFileName = file.FileName;
                    List<Plan> plans = await Task.Run(() => ExcelManager.Instance.ReadEachExcelData(file.FilePath));

                    foreach (var plan in plans)
                    {
                        if (uniqueHashSet.Add(plan))
                        {
                            MergedEduPlanGridList.Add(plan);
                        }
                    }

                    processedFiles++;
                    Progress = (double)processedFiles / totalFiles * 100;
                    await Task.Delay(100); 
                }
            }
            catch (Exception ex)
            {
                Log.Error(ex);
            }
            finally
            {
                loadingView.Close();
                MergeCompleted?.Invoke();
         
            }
        }

        private bool CanExecuteMergeFile(object obj)
        {
            return true;
        }

        private void ExecuteOpenSettingWindow(object obj)
        {
            SettingView settingView = new SettingView();
            settingView.ShowDialog();
        }

        private bool CanOpenSettingWindow(object obj)
        {
            return true;
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
                List<Plan> eduPlanList = ExcelManager.Instance.ReadEachExcelData(SelectedFile.FilePath);
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
