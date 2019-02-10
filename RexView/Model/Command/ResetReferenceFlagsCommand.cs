using RexView.View;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace RexView.Model.Command
{
    class ResetReferenceFlagsCommand : ICommand
    {
        public event EventHandler CanExecuteChanged;

        public bool CanExecute(object parameter)
            => true;

        public void Execute(object parameter)
        {
            if (parameter is RegexView view)
            {
                view.IsRegexReferenceMode = false;
                view.IsReplaceExpressionReferenceMode = false;
            }
        }
    }
}
