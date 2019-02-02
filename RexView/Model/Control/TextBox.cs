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
                SetValue(TextProperty, range.Text.TrimEnd());
            }
        }
        
        public string Text
        {
            get { return (string)GetValue(TextProperty); }
            set
            {
                SetValue(TextProperty, value);

                TextRange range = new TextRange(Document.ContentStart, Document.ContentEnd);
                range.Text = value;
            }
        }

        // Using a DependencyProperty as the backing store for Text.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty TextProperty =
            DependencyProperty.Register("Text", typeof(string), typeof(TextBox), new FrameworkPropertyMetadata(string.Empty));
    }
}
