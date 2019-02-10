using RexView.Model.DataObject;
using RexView.Model.DataType;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Json;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Input;

namespace RexView.Model
{
    [DataContract]
    public class RegexModel : NotifyProperty, INotifyPropertyChanged, INotifyDataErrorInfo, IDisposable
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

        public DispatchObservableCollection<DispatchObservableCollection<Group>> MatchGroups
        {
            get => GetValue(new DispatchObservableCollection<DispatchObservableCollection<Group>>());
        }
       
        [DataMember]
        public DispatchObservableCollection<RegexOption> MatchOptions
        {
            get => GetValue(new DispatchObservableCollection<RegexOption>());
            set => SetValue(value);
        }

        public RegexCollection RegexCollection { get => GetValue<RegexCollection>(null); set => SetValue(value); }
        
        public ICommand FileDropCommand { get => GetValue<ICommand>(new ActionCommand(FileDrop)); set => SetValue(value); }
        public ICommand ErrorCommand { get => GetValue<ICommand>(); set => SetValue(value); }
        public ICommand MatchCommand { get => GetValue<ICommand>(); set => SetValue(value); }
        
        public event EventHandler<DataErrorsChangedEventArgs> ErrorsChanged;
        
        public IDictionary<string, IList<string>> Errors { get => GetValue(new Dictionary<string, IList<string>>()); set => SetValue(value); }
        
        public bool HasErrors => 0 < Errors.Count;
        
        public void Initialize()
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

        private void FileDrop(object obj)
        {
            if (obj is FileInfo[] files)
            {
                StringBuilder sb = new StringBuilder();
                foreach (var file in files)
                {
                    using (StreamReader sr = file.OpenText())
                    {
                        sb.Append(sr.ReadToEnd());
                    }
                }

                Text = sb.ToString();
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

        private async Task<bool> Test(string text, string regex, RegexOptions options)
        {
            if (Errors.ContainsKey(nameof(RegexText)))
            {
                Errors.Remove(nameof(RegexText));
            }

            try
            {
                return await Task.Run(() => Regex.IsMatch(text, regex, options));
            }
            catch (ArgumentOutOfRangeException e)
            {
                // 正規表現オプション 例外
                Errors[nameof(RegexText)] = new List<string> { e.Message, };
                ErrorsChanged.Invoke(this, new DataErrorsChangedEventArgs(nameof(RegexText)));
                return false;
            }
            catch (Exception e)
            {
                // 正規表現 例外
                Errors[nameof(RegexText)] = new List<string> { e.Message, };
                ErrorsChanged.Invoke(this, new DataErrorsChangedEventArgs(nameof(RegexText)));
                return false;
            }
        }
        
        public async void OnTextChanged()
        {
            if (null == MatchGroups)
            {
                return;
            }

            Clear();

            var options = MatchOptions.Aggregate();

            if (!await Test(Text, RegexText, options))
            {
                return;
            }

            foreach (Match match in Regex.Matches(Text, RegexText, options))
            {
                while (MatchGroups.Count < match.Groups.Count)
                {
                    MatchGroups.Add(new DispatchObservableCollection<Group>());
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

        public IEnumerable GetErrors(string propertyName)
        {
            if (!string.IsNullOrWhiteSpace(propertyName))
            {
                if (Errors.ContainsKey(propertyName))
                {
                    return Errors[propertyName];
                }
            }

            return new List<string>();
        }
    }
}
