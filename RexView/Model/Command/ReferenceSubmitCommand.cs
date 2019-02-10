using RexView.View;
using System;
using System.Windows.Input;

namespace RexView.Model.Command
{
    public class ReferenceSubmitCommand : ICommand
    {
        public event EventHandler CanExecuteChanged;

        public bool CanExecute(object parameter)
            => true;

        public void Execute(object parameter)
        {
            if (parameter is ReferenceView view)
            {
                if (view.Command?.CanExecute(new object()) ?? false)
                {
                    view.Command.Execute(view.CommandParameter);
                }
            }
        }
    }
}
