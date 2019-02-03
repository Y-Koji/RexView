using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text.RegularExpressions;
using System.Windows.Input;

namespace RexView.Model
{
    public class RegexModel : NotifyProperty, INotifyPropertyChanged, IDisposable
    {
        public string Text { get => GetValue<string>(); set => SetValue(value, OnTextChanged); }
        public string RegexText { get => GetValue<string>(); set => SetValue(value, OnTextChanged); }
        public string ReplaceExpression { get => GetValue<string>(); set => SetValue(value, OnTextChanged); }
        public string ReplacedText { get => GetValue<string>(); set => SetValue(value); }

        public ObservableCollection<ObservableCollection<Group>> MatchGroups { get; }
        public ObservableCollection<RegexOption> MatchOptions { get; }

        public ICommand ErrorCommand { get => GetValue<ICommand>(); set => SetValue(value); }
        public ICommand MatchCommand { get => GetValue<ICommand>(); set => SetValue(value); }
        
        public RegexModel() : base()
        {
            MatchGroups = new ObservableCollection<ObservableCollection<Group>>();
            MatchOptions = new ObservableCollection<RegexOption>();
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

        private void Clear()
        {
            foreach (var match in MatchGroups)
            {
                match.Clear();
            }

            MatchGroups.Clear();
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

            ReplacedText = Regex.Replace(Text, RegexText, ReplaceExpression);
        }

        public void Dispose()
        {
            Clear();
            MatchOptions.Clear();
        }
    }
}
