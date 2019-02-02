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
            Regex = new RegexModel();

            Title = "RexView";
        }

        private void Initialize()
        {

        }

        public void Reference()
        {
            Regex.Text = "hao123_hello345";
            Regex.RegexText = @"hao(?<num1>\d+)_hello(?<num2>\d+)";
        }

        public void Dispose()
        {
            Regex.Dispose();
        }
    }
}
