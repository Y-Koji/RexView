using RexView.Model;
using System;
using System.ComponentModel;
using System.Windows.Input;

namespace RexView.ViewModel
{
    public class MainViewModel : NotifyProperty, INotifyPropertyChanged, IDisposable
    {
        public ICommand InitializeCommand { get => GetValue<ICommand>(); set => SetValue(value); }
        public ICommand DisposeCommand { get => GetValue<ICommand>(); set => SetValue(value); }
        public ICommand ReferenceCommand { get => GetValue<ICommand>(); set => SetValue(value); }

        public string Title { get => GetValue<string>(); set => SetValue(value); }

        public RegexModel Regex { get => GetValue<RegexModel>(); set => SetValue(value); }

        public MainViewModel()
        {
            InitializeCommand = new ActionCommand(Initialize);
            DisposeCommand = new ActionCommand(Dispose);
            ReferenceCommand = new ActionCommand(Reference);
            Regex = RegexModel.Load();

            Title = "RexView";
        }

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
