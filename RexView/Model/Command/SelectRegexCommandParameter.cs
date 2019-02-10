using RexView.Model.DataObject;
using System.Windows;

namespace RexView.Model.Command
{
    public class SelectRegexCommandParameter : Freezable
    {
        public RegexCollectionItem Regex
        {
            get { return (RegexCollectionItem)GetValue(RegexProperty); }
            set { SetValue(RegexProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Regex.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty RegexProperty =
            DependencyProperty.Register("Regex", typeof(RegexCollectionItem), typeof(SelectRegexCommandParameter), new PropertyMetadata(null));
        
        public RegexModel RegexModel
        {
            get { return (RegexModel)GetValue(RegexModelProperty); }
            set { SetValue(RegexModelProperty, value); }
        }

        // Using a DependencyProperty as the backing store for RegexModel.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty RegexModelProperty =
            DependencyProperty.Register("RegexModel", typeof(RegexModel), typeof(SelectRegexCommandParameter), new PropertyMetadata(null));
        
        protected override Freezable CreateInstanceCore()
        {
            return new SelectRegexCommandParameter();
        }
    }
}
