using IntegrationExcelImporter.Common;
using IntegrationExcelImporter.Model;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace IntegrationExcelImporter.ViewModel
{
    public class MainViewModel : ObservableObjectBase<MainViewModel>
    {
        public event PropertyChangedEventHandler PropertyChanged;

        private ObservableCollection<FileProperty> _files = null;
        public ObservableCollection<FileProperty> Files 
        {
            get
            {
                if (_files == null)
                {
                    _files = new ObservableCollection<FileProperty>();
                }
                return _files;
            }
            set
            {
                _files = value;
                OnPropertyChanged(p => p.Files);
            }
        }

        public ICommand OpenFileCommand { get; set; }
        public MainViewModel()
        {
            OpenFileCommand = new RelayCommand<object>(ExecuteOpenFileButton, CanOpenFileButton);
        }

        private void ExecuteOpenFileButton(object obj)
        {
            Log.Info("FILE DIALOG OPENED");
            OpenFileDialog dialog = new OpenFileDialog
            {
                Multiselect = true
            };

            if (dialog.ShowDialog() == true)
            {
                foreach (string file in dialog.FileNames)
                {
                    var fileInfo = new FileProperty
                    {
                        FileName = Path.GetFileNameWithoutExtension(file),
                        FileExtension = Path.GetExtension(file)
                    };

                    Files.Add(fileInfo);
                }
            }
        }

        private bool CanOpenFileButton(object obj)
        {
            if (obj == null)
            {
                return true;
            }
            return obj is MainViewModel;
        }

    }
}
