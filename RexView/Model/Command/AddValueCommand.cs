using RexView.Model.DataObject;
using RexView.Model.DataType;
using RexView.Model.Serialize;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace RexView.Model.Command
{
    public class AddValueCommand : ICommand
    {
        public event EventHandler CanExecuteChanged;

        public bool CanExecute(object parameter)
            => true;

        public void Execute(object parameter)
        {
            if (parameter is AddValueCommandParameter param)
            {
                if (param.List is RegexCollection regexCollection)
                {
                    param.List.Add(new RegexCollectionItem
                    {
                        Id = param.Value.Id ?? Guid.NewGuid().ToString("N"),
                        Index = param.Value.Index,
                        Name = param.Value.Name ?? string.Empty,
                        Value = param.Value.Value ?? string.Empty,
                        RegexReplaceExpressionCollection =
                            new DispatchObservableCollection<IReferenceItem>(),
                    });
                }
                else if (param.Value is ICloneable cloneable)
                {
                    param.List?.Add(cloneable.Clone());
                }
            }
        }
    }
}
