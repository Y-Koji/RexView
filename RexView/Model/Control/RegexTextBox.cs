using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Media;

namespace RexView.Model.Control
{
    public class RegexTextBox : TextBox
    {
        private static Brush RoundBracketBrush { get; } = new SolidColorBrush(Color.FromRgb(0, 0, 255));
        private static Brush SquareBracketBrush { get; } = new SolidColorBrush(Color.FromRgb(255, 0, 0));
        private static Brush CurlyBracketBrush { get; } = new SolidColorBrush(Color.FromRgb(255, 165, 0));
        private static Brush EscapeBrush { get; } = new SolidColorBrush(Color.FromRgb(255, 0, 255));
        private static Brush SpecialBrush { get; } = new SolidColorBrush(Color.FromRgb(255, 255, 0));
        private static Brush TextBrush { get; } = new SolidColorBrush(Color.FromRgb(50, 50, 50));
        private static char[] BracketStartKeywords { get; } = "([{".ToCharArray();
        private static char[] BracketEndKeywords { get; } = ")]}".ToCharArray();
        private static char[] RepeatKeywords { get; } = "+*".ToCharArray();
        private static char[] SpecialWords { get; } = "^$?.".ToCharArray();

        public RegexTextBox()
        {
            HorizontalScrollBarVisibility = ScrollBarVisibility.Hidden;
            Document.PageWidth = 10000;

            TextChanged += OnTextChanged;
        }
        
        private void OnTextChanged(object sender, TextChangedEventArgs e)
        {
            TextChanged -= OnTextChanged;

            TextRange range = new TextRange(Document.ContentStart, Document.ContentEnd);
            
            IList<TextPointer> pointers = new List<TextPointer>();
            for (TextPointer pointer = range.Start; null != pointer; pointer = pointer.GetNextInsertionPosition(LogicalDirection.Forward))
            {
                pointers.Add(pointer);
            }

            Stack<string> keywordStack = new Stack<string>();
            Stack<Brush> colorStack = new Stack<Brush>();
            StringBuilder sb = new StringBuilder();
            List<Brush> colorHistory = new List<Brush>();
            bool isEscaping = false;
            for (int i = 0; i < pointers.Count() - 1; i++)
            {
                TextRange word = new TextRange(pointers[i], pointers[i + 1]);

                if ("(" == word.Text && !isEscaping)
                {
                    keywordStack.Push("(");
                    colorStack.Push(RoundBracketBrush);

                    word.ApplyPropertyValue(TextElement.ForegroundProperty, RoundBracketBrush);
                }
                else if ("[" == word.Text && !isEscaping)
                {
                    keywordStack.Push("[");
                    colorStack.Push(SquareBracketBrush);

                    word.ApplyPropertyValue(TextElement.ForegroundProperty, SquareBracketBrush);
                }
                else if  ("{" == word.Text && !isEscaping)
                {
                    keywordStack.Push("{");
                    colorStack.Push(CurlyBracketBrush);

                    word.ApplyPropertyValue(TextElement.ForegroundProperty, CurlyBracketBrush);
                }
                else if (RepeatKeywords.Contains(word.Text[0]) && !isEscaping)
                {
                    if ('\\' == sb[sb.Length - 2])
                    {
                        // エスケープ文字
                        word.ApplyPropertyValue(TextElement.ForegroundProperty, EscapeBrush);
                    }
                    else if (BracketEndKeywords.Contains(sb[sb.Length - 1]) && 0 < colorHistory.Count)
                    {
                        // 括弧閉じ後の "+*" は括弧の色にする
                        word.ApplyPropertyValue(TextElement.ForegroundProperty, colorHistory.Last());
                    }
                    else if (0 < colorStack.Count && 0 < colorStack.Count)
                    {
                        // 括弧内テキストは括弧の色にする
                        word.ApplyPropertyValue(TextElement.ForegroundProperty, colorStack.Peek());
                    }
                    else 
                    {
                        word.ApplyPropertyValue(TextElement.ForegroundProperty, TextBrush);
                    }
                }
                else if ("\\" == word.Text && !isEscaping)
                {
                    word.ApplyPropertyValue(TextElement.ForegroundProperty, EscapeBrush);

                    sb.Append(word.Text);
                    isEscaping = true;
                    continue;
                }
                else if (isEscaping)
                {
                    word.ApplyPropertyValue(TextElement.ForegroundProperty, EscapeBrush);
                }
                else if (0 < keywordStack.Count)
                { 
                    if (")" == word.Text)
                    {
                        if ("(" == keywordStack.Peek())
                        {
                            keywordStack.Pop();
                            colorHistory.Add(colorStack.Pop());
                            word.ApplyPropertyValue(TextElement.ForegroundProperty, colorHistory.Last());
                        }
                    }
                    else if ("]" == word.Text)
                    {
                        if ("[" == keywordStack.Peek())
                        {
                            keywordStack.Pop();
                            colorHistory.Add(colorStack.Pop());
                            word.ApplyPropertyValue(TextElement.ForegroundProperty, colorHistory.Last());
                        }
                    }
                    else if ("}" == word.Text)
                    {
                        if ("{" == keywordStack.Peek())
                        {
                            keywordStack.Pop();
                            colorHistory.Add(colorStack.Pop());
                            word.ApplyPropertyValue(TextElement.ForegroundProperty, colorHistory.Last());
                        }
                    }
                    else if (0 < colorStack.Count)
                    {
                        word.ApplyPropertyValue(TextElement.ForegroundProperty, colorStack.Peek());
                    }
                }
                else
                {
                    word.ApplyPropertyValue(TextElement.ForegroundProperty, TextBrush);
                }

                if (SpecialWords.Contains(word.Text[0]) && !isEscaping)
                {
                    word.ApplyPropertyValue(TextElement.BackgroundProperty, SpecialBrush);
                }
                else
                {
                    word.ApplyPropertyValue(TextElement.BackgroundProperty, new SolidColorBrush(Color.FromArgb(0, 0, 0, 0)));
                }
                
                isEscaping = false;
                sb.Append(word.Text);
            }


            TextChanged += OnTextChanged;
            //CaretPosition = caret;
        }
    }
}
