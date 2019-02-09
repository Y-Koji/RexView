using RexView.View;
using System;
using System.Windows.Controls.Primitives;
using System.Windows.Input;

namespace RexView.Model.Command
{
    class ReferenceModeExitCommand : ICommand
    {
        public event EventHandler CanExecuteChanged;

        public bool CanExecute(object parameter)
            => true;

        public void Execute(object parameter)
        {
            if (parameter is RegexView view)
            {
                view.IsReferenceMode = false;
            }
        }
    }
}
