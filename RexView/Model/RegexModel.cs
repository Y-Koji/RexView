using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Json;
using System.Text.RegularExpressions;
using System.Windows.Input;

namespace RexView.Model
{
    [DataContract]
    public class RegexModel : NotifyProperty, INotifyPropertyChanged, IDisposable
    {
        private static string FILE_NAME { get; } = $"{nameof(RegexModel)}.json";
        private static DataContractJsonSerializer Serializer { get; } = new DataContractJsonSerializer(typeof(RegexModel));

        [DataMember]
        public string Text { get => GetValue(string.Empty); set => SetValue(value, OnTextChanged); }
        [DataMember]
        public string RegexText { get => GetValue(string.Empty); set => SetValue(value, OnTextChanged); }
        [DataMember]
        public string ReplaceExpression { get => GetValue(string.Empty); set => SetValue(value, OnTextChanged); }
        [DataMember]
        public string ReplacedText { get => GetValue(string.Empty); set => SetValue(value); }

        public ObservableCollection<ObservableCollection<Group>> MatchGroups
        {
            get => GetValue(new ObservableCollection<ObservableCollection<Group>>());
        }
        
        [DataMember]
        public ObservableCollection<RegexOption> MatchOptions
        {
            get => GetValue(new ObservableCollection<RegexOption>());
            set => SetValue(value);
        }

        public ICommand InitializeCommand { get => GetValue<ICommand>(new ActionCommand(Initialize)); set => SetValue(value); }
        public ICommand ErrorCommand { get => GetValue<ICommand>(); set => SetValue(value); }
        public ICommand MatchCommand { get => GetValue<ICommand>(); set => SetValue(value); }
        
        private void Initialize()
        {
            if (0 < MatchOptions.Count)
            {
                foreach (var regexOption in MatchOptions)
                {
                    if (0 == regexOption.RegexOptions.Count)
                    {
                        MatchOptions.ToList().ForEach(regexOption.RegexOptions.Add);
                    }

                    regexOption.SetHandler(nameof(RegexOption.IsChecked), OnTextChanged);
                }
            }
            else
            {
                foreach (RegexOptions option in Enum.GetValues(typeof(RegexOptions)))
                {
                    RegexOption regexOption = new RegexOption
                    {
                        RegexOptions = MatchOptions,
                        OptionName = option.ToString(),
                        OptionValue = option,
                        IsChecked = false,
                    };

                    regexOption.SetHandler(nameof(RegexOption.IsChecked), OnTextChanged);

                    MatchOptions.Add(regexOption);
                }
            }
        }

        private void Clear()
        {
            foreach (var match in MatchGroups)
            {
                match.Clear();
            }

            MatchGroups?.Clear();
        }

        private bool Test(string text, string regex, RegexOptions options)
        {
            try
            {
                return Regex.IsMatch(text, regex, options);
            }
            catch (ArgumentOutOfRangeException e)
            {
                // 正規表現オプション 例外
                return false;
            }
            catch (Exception e)
            {
                // 正規表現 例外
                return false;
            }
        }
        
        public void OnTextChanged()
        {
            if (null == MatchGroups)
            {
                return;
            }

            Clear();

            var options = MatchOptions.Aggregate();

            if (!Test(Text, RegexText, options))
            {
                return;
            }

            foreach (Match match in Regex.Matches(Text, RegexText, options))
            {
                while (MatchGroups.Count < match.Groups.Count)
                {
                    MatchGroups.Add(new ObservableCollection<Group>());
                }

                for (int i = 0;i < match.Groups.Count;i++)
                {
                    MatchGroups[i].Add(match.Groups[i]);
                }
            }

            ReplacedText = Regex.Replace(Text, RegexText, Regex.Unescape(ReplaceExpression));
        }

        public static RegexModel Load()
        {
            if (!File.Exists(FILE_NAME))
            {
                return new RegexModel();
            }

            try
            {
                using (FileStream fs = File.OpenRead(FILE_NAME))
                {
                    return Serializer.ReadObject(fs) as RegexModel;
                }
            }
            catch (Exception e)
            {
                File.Delete(FILE_NAME);

                return new RegexModel();
            }
        }

        public void Save()
        {
            using (FileStream fs = File.Create(FILE_NAME))
            {
                Serializer.WriteObject(fs, this);
            }
        }

        public void Dispose()
        {
            Save();
            Clear();
            MatchOptions.Clear();
        }
    }
}
