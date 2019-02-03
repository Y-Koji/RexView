using RexView.Model;
using System;
using System.ComponentModel;
using System.Windows.Input;

namespace RexView.ViewModel
{
    public class MainViewModel : NotifyProperty, INotifyPropertyChanged, IDisposable
    {
        public ICommand InitializeCommand { get => GetValue(new ActionCommand(Initialize)); set => SetValue(value); }
        public ICommand DisposeCommand { get => GetValue(new ActionCommand(Dispose)); set => SetValue(value); }
        public ICommand ReferenceCommand { get => GetValue(new ActionCommand(Reference)); set => SetValue(value); }

        public string Title { get => GetValue("RexView"); set => SetValue(value); }

        public RegexModel Regex { get => GetValue(RegexModel.Load()); set => SetValue(value); }
        
        private void Initialize()
        {
            Regex.InitializeCommand.Execute(new object());
        }

        public void Reference()
        {
            string txt = Regex.RegexText;
            Regex.RegexText = txt;
        }

        public void Dispose()
        {
            Regex.Dispose();
        }
    }
}
