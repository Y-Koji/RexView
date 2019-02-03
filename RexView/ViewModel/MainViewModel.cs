﻿using RexView.Model;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows.Data;
using System.Windows.Input;

namespace RexView.ViewModel
{
    public class MainViewModel : NotifyProperty, INotifyPropertyChanged, IDisposable
    {
        public ICommand InitializeCommand { get => GetValue(new ActionCommand(Initialize)); set => SetValue(value); }
        public ICommand ReferenceCommand { get => GetValue(new ActionCommand(Reference)); set => SetValue(value); }
        public ICommand AddRegexCommand { get => GetValue(new ActionCommand(AddRegex)); set => SetValue(value); }
        public ICommand RemoveRegexCommand { get => GetValue(new ActionCommand(RemoveRegex)); set => SetValue(value); }
        public ICommand DisposeCommand { get => GetValue(new ActionCommand(Dispose)); set => SetValue(value); }

        public string Title { get => GetValue("RexView"); set => SetValue(value); }
        
        public ObservableCollection<RegexModel> Regexes
        {
            get => GetValue(new ObservableCollection<RegexModel>());
            set => SetValue(value);
        }
        
        private void Initialize()
        {
            foreach (var regex in RegexModel.Loads())
            {
                Regexes.Add(regex);
                regex.InitializeCommand.Execute(new object());
            }

            IEditableCollectionView regexesView = CollectionViewSource.GetDefaultView(Regexes) as IEditableCollectionView;
            regexesView.NewItemPlaceholderPosition = NewItemPlaceholderPosition.AtEnd;
        }

        public void Reference()
        {

        }

        private void AddRegex()
        {
            RegexModel model = new RegexModel("Regex_" + Regexes.Count);
            model.InitializeCommand.Execute(new object());
            Regexes.Add(model);
        }

        private void RemoveRegex(object obj)
        {
            RegexModel model = obj as RegexModel;
            Regexes.Remove(model);
            model.Delete();
            model.Dispose();
        }

        public void Dispose()
        {
            foreach (var regex in Regexes)
            {
                regex.Dispose();
            }
        }
    }
}
