using RexView.Model.DataType;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Media;

namespace RexView.Model.Control
{
    public class TextBox : RichTextBox
    {
        public TextBox()
        {
            TextChanged += OnTextChanged;
        }

        private void OnTextChanged(object sender, TextChangedEventArgs e)
        {
            TextRange range = new TextRange(Document.ContentStart, Document.ContentEnd);
            string text = range.Text.TrimEnd(new char[] { '\r', '\n', });

            if (text != Text)
            {
                Text = text;
            }
        }
        
        public string Text
        {
            get { return (string)GetValue(TextProperty); }
            set { SetValue(TextProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Text.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty TextProperty =
            DependencyProperty.Register(
                "Text", 
                typeof(string), 
                typeof(TextBox), 
                new FrameworkPropertyMetadata(string.Empty, TextPropertyChanged));

        private static void TextPropertyChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e)
        {
            TextBox textBox = sender as TextBox;

            TextRange range = new TextRange(textBox.Document.ContentStart, textBox.Document.ContentEnd);
            string text = range.Text.TrimEnd(new char[] { '\r', '\n', });
            if (text != e.NewValue.ToString())
            {
                range.Text = e.NewValue.ToString();
            }
        }

        public DispatchObservableCollection<DispatchObservableCollection<Group>> MatchGroups
        {
            get { return (DispatchObservableCollection<DispatchObservableCollection<Group>>)GetValue(MatchGroupsProperty); }
            set { SetValue(MatchGroupsProperty, value); }
        }

        // Using a DependencyProperty as the backing store for MatchGroups.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty MatchGroupsProperty =
            DependencyProperty.Register("MatchGroups", typeof(DispatchObservableCollection<DispatchObservableCollection<Group>>), typeof(TextBox), new PropertyMetadata(new DispatchObservableCollection<DispatchObservableCollection<Group>>(), (d, e) => 
            {
                // マッチグループが更新された際に
                // 入力テキストのマッチしている個所を黄色でハイライトする処理
                // ・・・のはずだがTextRangeがなんか文字数のオフセットから取得できない．．．

                //if (d is TextBox textBox)
                //{
                //    TextRange range = new TextRange(textBox.Document.ContentStart, textBox.Document.ContentEnd);

                //    if (e.NewValue is DispatchObservableCollection<DispatchObservableCollection<Group>> groupsCollection)
                //    {
                //        groupsCollection.CollectionChanged += (_, __) =>
                //        {
                //            if (0 == groupsCollection.Count)
                //            {
                //                return;
                //            }

                //            var groups = groupsCollection.First();
                //            groups.CollectionChanged += (___, ____) =>
                //            {
                //                foreach (var group in groups)
                //                {
                //                    TextRange selection = new TextRange(textBox.Document.ContentStart, textBox.Document.ContentEnd);
                //                    selection.Select(
                //                        selection.Start.GetPositionAtOffset(group.Index, LogicalDirection.Forward),
                //                        selection.Start.GetPositionAtOffset(group.Index + group.Length, LogicalDirection.Backward));

                //                    selection.ApplyPropertyValue(TextElement.BackgroundProperty,
                //                        new SolidColorBrush(Color.FromRgb(255, 255, 0)));
                //                }
                //            };
                //        };
                //    }
                //}
            }));
    }
}
