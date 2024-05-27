using System;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
namespace IntegrationExcelImporter.Common
{
    public class RelayCommand<T> : ICommand
    {
        private readonly Action<T> _execute;
        private readonly Predicate<T> _predicate;

        public event EventHandler CanExecuteChanged
        {
            add { CommandManager.RequerySuggested += value; }
            remove { CommandManager.RequerySuggested -= value; }
        }

        public RelayCommand(Action<T> action, Predicate<T> predicate)
        {
            _execute = action;
            _predicate = predicate;
        }

        public RelayCommand(Action<T> action) : this(action, null) { }

        public bool CanExecute(object parameter)
        {
            return _predicate == null || _predicate((T)parameter);
        }

        public void Execute(object parameter)
        {
            _execute((T)parameter);
        }
    }
}
