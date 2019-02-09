using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace RexView.Model.Command
{
    public class AddRegexCommandParameter : Freezable
    {
        public RegexCollection Collection
        {
            get { return (RegexCollection)GetValue(CollectionProperty); }
            set { SetValue(CollectionProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Collection.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty CollectionProperty =
            DependencyProperty.Register("Collection", typeof(RegexCollection), typeof(AddRegexCommandParameter), new PropertyMetadata(null));
        
        public RegexCollectionItem Regex
        {
            get { return (RegexCollectionItem)GetValue(RegexProperty); }
            set { SetValue(RegexProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Regex.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty RegexProperty =
            DependencyProperty.Register("Regex", typeof(RegexCollectionItem), typeof(AddRegexCommandParameter), new PropertyMetadata(null));
        
        protected override Freezable CreateInstanceCore()
        {
            return new AddRegexCommandParameter();
        }
    }
}
