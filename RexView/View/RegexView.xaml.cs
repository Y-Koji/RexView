using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace RexView.View
{
    /// <summary>
    /// RegexView.xaml の相互作用ロジック
    /// </summary>
    public partial class RegexView : UserControl
    {
        public RegexView()
        {
            InitializeComponent();
        }

        public bool IsRegexReferenceMode
        {
            get { return (bool)GetValue(IsRegexReferenceModeProperty); }
            set { SetValue(IsRegexReferenceModeProperty, value); }
        }

        // Using a DependencyProperty as the backing store for IsRegexReferenceMode.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty IsRegexReferenceModeProperty =
            DependencyProperty.Register("IsRegexReferenceMode", typeof(bool), typeof(RegexView), new PropertyMetadata(false));
        
        public bool IsReplaceExpressionReferenceMode
        {
            get { return (bool)GetValue(IsReplaceExpressionReferenceModeProperty); }
            set { SetValue(IsReplaceExpressionReferenceModeProperty, value); }
        }

        // Using a DependencyProperty as the backing store for IsReplaceExpressionReferenceMode.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty IsReplaceExpressionReferenceModeProperty =
            DependencyProperty.Register("IsReplaceExpressionReferenceMode", typeof(bool), typeof(RegexView), new PropertyMetadata(false));
    }
}
