using System;
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

        public ObservableCollection<ObservableCollection<Group>> MatchGroups { get; private set; }

        public ICommand ErrorCommand { get => GetValue<ICommand>(); set => SetValue(value); }
        public ICommand MatchCommand { get => GetValue<ICommand>(); set => SetValue(value); }
        
        public RegexModel() : base()
        {
            MatchGroups = new ObservableCollection<ObservableCollection<Group>>();
        }

        private void Clear()
        {
            foreach (var match in MatchGroups)
            {
                match.Clear();
            }

            MatchGroups.Clear();
        }

        private bool Test(string text, string regex)
        {
            try
            {
                return Regex.IsMatch(text, regex);
            }
            catch (Exception e)
            {
                return false;
            }
        }

        public void OnTextChanged()
        {
            Clear();
            
            if (!Test(Text, RegexText))
            {
                return;
            }

            foreach (Match match in Regex.Matches(Text, RegexText))
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

        }
    }
}
