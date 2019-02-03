using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.Serialization;
using System.Text.RegularExpressions;

namespace RexView.Model
{
    [DataContract]
    public class RegexOption : NotifyProperty, INotifyPropertyChanged
    {
        public ObservableCollection<RegexOption> RegexOptions
        {
            get => GetValue(new ObservableCollection<RegexOption>());
            set => SetValue(value);
        }

        [DataMember]
        public string OptionName { get => GetValue(string.Empty); set => SetValue(value); }

        [DataMember]
        public RegexOptions OptionValue { get => GetValue<RegexOptions>(); set => SetValue(value); }

        [DataMember]
        public bool IsChecked
        {
            get => GetValue(false);
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
