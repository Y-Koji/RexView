using RexView.Model.Serialize;
using System;
using System.Collections.Generic;
using System.Windows;

namespace RexView.Model
{
    public class RegexCollectionItem : Freezable, ICloneable, IRegexCollectionItem<IReferenceItem>
    {
        public string Id
        {
            get { return (string)GetValue(IdProperty); }
            set { SetValue(IdProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Id.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty IdProperty =
            DependencyProperty.Register("Id", typeof(string), typeof(RegexCollectionItem), new PropertyMetadata(Guid.NewGuid().ToString("N")));

        public int Index
        {
            get { return (int)GetValue(IndexProperty); }
            set { SetValue(IndexProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Index.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty IndexProperty =
            DependencyProperty.Register("Index", typeof(int), typeof(RegexCollectionItem), new PropertyMetadata(0));

        public string Name
        {
            get { return (string)GetValue(NameProperty); }
            set { SetValue(NameProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Name.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty NameProperty =
            DependencyProperty.Register("Name", typeof(string), typeof(RegexCollectionItem), new PropertyMetadata(string.Empty));
        
        public string Value
        {
            get { return (string)GetValue(ValueProperty); }
            set { SetValue(ValueProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Value.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ValueProperty =
            DependencyProperty.Register("Value", typeof(string), typeof(RegexCollectionItem), new PropertyMetadata(string.Empty));
        
        public ICollection<IReferenceItem> RegexReplaceExpressionCollection
        {
            get { return (ICloneableCollection<IReferenceItem>)GetValue(RegexReplaceExpressionCollectionProperty); }
            set { SetValue(RegexReplaceExpressionCollectionProperty, value); }
        }

        // Using a DependencyProperty as the backing store for RegexReplaceExpressionCollection.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty RegexReplaceExpressionCollectionProperty =
            DependencyProperty.Register("RegexReplaceExpressionCollection", typeof(ICollection<IReferenceItem>), typeof(RegexCollectionItem), new PropertyMetadata(new DispatchObservableCollection<IReferenceItem>()));
        
        object ICloneable.Clone()
        {
            return new RegexCollectionItem
            {
                Id = Guid.NewGuid().ToString("N"),
                Name = this.Name,
                Value = this.Value,
                RegexReplaceExpressionCollection = (RegexReplaceExpressionCollection as ICloneableCollection<IReferenceItem>).Clone(),
            };
        }

        protected override Freezable CreateInstanceCore()
        {
            return new RegexCollectionItem();
        }
    }
}
