using RexView.Model.DataObject;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace RexView.Model.Command
{
    public class AddValueCommandParameter : Freezable
    {
        public IList List
        {
            get { return (IList)GetValue(ListProperty); }
            set { SetValue(ListProperty, value); }
        }

        // Using a DependencyProperty as the backing store for List.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ListProperty =
            DependencyProperty.Register("List", typeof(IList), typeof(AddValueCommandParameter), new PropertyMetadata(null));
        
        public ReferenceItem Value
        {
            get { return (ReferenceItem)GetValue(ValueProperty); }
            set { SetValue(ValueProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Value.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ValueProperty =
            DependencyProperty.Register("Value", typeof(ReferenceItem), typeof(AddValueCommandParameter), new PropertyMetadata(null));
        
        protected override Freezable CreateInstanceCore()
        {
            return new AddValueCommandParameter();
        }
    }
}
