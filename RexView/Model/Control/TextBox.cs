using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;

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

            if (range.Text != Text)
            {
                Text = range.Text.Trim();
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
            if (range.Text.Trim() != e.NewValue.ToString())
            {
                range.Text = e.NewValue.ToString();
            }
        }
    }
}
