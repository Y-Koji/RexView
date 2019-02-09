using RexView.Model.Serialize;
using System;
using System.Runtime.Serialization;
using System.Windows;

namespace RexView.Model
{
    public class RegexCollectionItem : Freezable, ICloneable, IRegexCollectionItem
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
        
        public string Regex
        {
            get { return (string)GetValue(RegexProperty); }
            set { SetValue(RegexProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Regex.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty RegexProperty =
            DependencyProperty.Register("Regex", typeof(string), typeof(RegexCollectionItem), new PropertyMetadata(string.Empty));


        object ICloneable.Clone()
        {
            return new RegexCollectionItem
            {
                Id = Guid.NewGuid().ToString("N"),
                Name = this.Name,
                Regex = this.Regex,
            };
        }

        protected override Freezable CreateInstanceCore()
        {
            return new RegexCollectionItem();
        }
    }
}
