﻿using System;
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
        private static string REGEX_DIR { get; } = "regex";
        private static string REGEX_PATH_FORMAT { get; } = $"{REGEX_DIR}\\{{0}}.json";
        private static DataContractJsonSerializer Serializer { get; } = new DataContractJsonSerializer(typeof(RegexModel));

        [DataMember]
        public string Name { get => GetValue(nameof(RegexModel)); set => SetValue(value); }
        [DataMember]
        public string Text { get => GetValue(string.Empty); set => SetValue(value, OnTextChanged); }
        [DataMember]
        public string RegexText { get => GetValue(string.Empty); set => SetValue(value, OnTextChanged); }
        [DataMember]
        public string ReplaceExpression { get => GetValue(string.Empty); set => SetValue(value, OnTextChanged); }
        [DataMember]
        public string ReplacedText { get => GetValue(string.Empty); set => SetValue(value); }

        private bool IsDeleted { get; set; } = false;
        
        public RegexModel(string name)
        {
            Name = name;
        }

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

        public static RegexModel Load(string name)
        {
            string fileName = string.Format(REGEX_PATH_FORMAT, name);

            if (!Directory.Exists(REGEX_DIR))
            {
                Directory.CreateDirectory(REGEX_DIR);
            }

            if (!File.Exists(fileName))
            {
                return new RegexModel(name);
            }

            try
            {
                using (FileStream fs = File.OpenRead(fileName))
                {
                    RegexModel model =  Serializer.ReadObject(fs) as RegexModel;
                    model.Name = name;

                    return model;
                }
            }
            catch (Exception e)
            {
                File.Delete(fileName);

                return new RegexModel(name);
            }
        }

        public static IEnumerable<RegexModel> Loads()
        {
            if (!Directory.Exists(REGEX_DIR))
            {
                Directory.CreateDirectory(REGEX_DIR);
            }

            foreach (var fileName in Directory.GetFiles(REGEX_DIR, "*.json"))
            {
                yield return Load(Path.GetFileNameWithoutExtension(fileName));
            }
        }

        public void Save()
        {
            if (!Directory.Exists(REGEX_DIR))
            {
                Directory.CreateDirectory(REGEX_DIR);
            }

            using (FileStream fs = File.Create(string.Format(REGEX_PATH_FORMAT, Name)))
            {
                Serializer.WriteObject(fs, this);
            }
        }

        public void Delete()
        {
            if (File.Exists(string.Format(REGEX_PATH_FORMAT, Name)))
            {
                File.Delete(string.Format(REGEX_PATH_FORMAT, Name));
            }

            IsDeleted = true;

            Dispose();
        }

        public void Dispose()
        {
            if (!IsDeleted)
            {
                Save();
            }

            Clear();
            MatchOptions.Clear();
        }
    }
}
