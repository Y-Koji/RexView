using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
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
    /// ReferenceView.xaml の相互作用ロジック
    /// </summary>
    public partial class ReferenceView : UserControl
    {
        public ReferenceView()
        {
            InitializeComponent();
        }

        public string Text
        {
            get { return (string)GetValue(TextProperty); }
            set { SetValue(TextProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Text.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty TextProperty =
            DependencyProperty.Register("Text", typeof(string), typeof(ReferenceView), new PropertyMetadata(string.Empty));
        
        public ToggleButton ToggleButton
        {
            get { return (ToggleButton)GetValue(ToggleButtonProperty); }
            set { SetValue(ToggleButtonProperty, value); }
        }

        // Using a DependencyProperty as the backing store for ToggleButton.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ToggleButtonProperty =
            DependencyProperty.Register("ToggleButton", typeof(ToggleButton), typeof(ReferenceView), new PropertyMetadata(null));
        
        public ICommand Command
        {
            get { return (ICommand)GetValue(CommandProperty); }
            set { SetValue(CommandProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Command.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty CommandProperty =
            DependencyProperty.Register("Command", typeof(ICommand), typeof(ReferenceView), new PropertyMetadata(null));

        public object CommandParameter
        {
            get { return (object)GetValue(CommandParameterProperty); }
            set { SetValue(CommandParameterProperty, value); }
        }

        // Using a DependencyProperty as the backing store for CommandParameter.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty CommandParameterProperty =
            DependencyProperty.Register("CommandParameter", typeof(object), typeof(ReferenceView), new PropertyMetadata(null));
    }
}
