using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Text.RegularExpressions;

namespace RexView.Model
{
    public class RegexOption : NotifyProperty, INotifyPropertyChanged
    {
        public ObservableCollection<RegexOption> RegexOptions
        {
            get => GetValue<ObservableCollection<RegexOption>>();
            set => SetValue(value);
        }

        public string OptionName { get => GetValue<string>(); set => SetValue(value); }

        public RegexOptions OptionValue { get => GetValue<RegexOptions>(); set => SetValue(value); }

        public bool IsChecked
        {
            get => GetValue<bool>();
            set
            {
                try
                {
                    if (value)
                    {
                        Regex.Match("test", "test", RegexOptions.Aggregate() | OptionValue);
                    }

                    SetValue(value);
                }
                catch (Exception)
                {
                    throw;
                }
            }
        }
    }
}
