using RexView.Model.DataObject;
using System.Collections;
using System.Windows;

namespace RexView.Model.Command
{
    public class SelectRefelenceValueCommandParameter : Freezable
    {
        public string Value
        {
            get { return (string)GetValue(ValueProperty); }
            set { SetValue(ValueProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Value.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ValueProperty =
            DependencyProperty.Register("Value", typeof(string), typeof(SelectRefelenceValueCommandParameter), new PropertyMetadata(string.Empty));
        
        public string NewValue
        {
            get { return (string)GetValue(NewValueProperty); }
            set { SetValue(NewValueProperty, value); }
        }

        // Using a DependencyProperty as the backing store for NewValue.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty NewValueProperty =
            DependencyProperty.Register("NewValue", typeof(string), typeof(SelectRefelenceValueCommandParameter), new PropertyMetadata(string.Empty));

        protected override Freezable CreateInstanceCore()
        {
            return new SelectRefelenceValueCommandParameter();
        }
    }
}
