using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace RexView.Model
{
    public class ActionCommand : ICommand
    {
        private Func<object, bool> _canExecute = _ => false;
        private Action<object> _execute = _ => { };

        public event EventHandler CanExecuteChanged;

        public ActionCommand(Action<object> execute)
        {
            _canExecute = _ => true;
            _execute = execute;
        }

        public ActionCommand(Action execute)
        {
            _canExecute = _ => true;
            _execute = _ => execute?.Invoke();
        }

        public ActionCommand(Func<object, bool> canExecute, Action<object> execute)
        {
            _canExecute = canExecute;
            _execute = execute;
        }

        public ActionCommand(Func<object, bool> canExecute, Action execute)
        {
            _canExecute = canExecute;
            _execute = _ => execute?.Invoke();
        }

        public bool CanExecute(object parameter)
            => _canExecute?.Invoke(parameter) ?? false;

        public void Execute(object parameter)
            => _execute?.Invoke(parameter);

        public void RaiseCanExecuteChanged(EventArgs e = null)
            => CanExecuteChanged?.Invoke(this, e ?? new EventArgs());
    }
}
