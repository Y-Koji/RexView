using RexView.Model.DataObject;
using RexView.Model.Serialize;
using System.Collections;
using System.Windows;

namespace RexView.Model.Command
{
    public class RemoveValueCommandParameter : Freezable
    {
        public IList List
        {
            get { return (IList)GetValue(ListProperty); }
            set { SetValue(ListProperty, value); }
        }

        // Using a DependencyProperty as the backing store for List.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ListProperty =
            DependencyProperty.Register("List", typeof(IList), typeof(RemoveValueCommandParameter), new PropertyMetadata(null));
        
        public object Value
        {
            get { return (object)GetValue(ValueProperty); }
            set { SetValue(ValueProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Value.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ValueProperty =
            DependencyProperty.Register("Value", typeof(object), typeof(RemoveValueCommandParameter), new PropertyMetadata(null));
        
        protected override Freezable CreateInstanceCore()
        {
            return new RemoveValueCommandParameter();
        }
    }
}
