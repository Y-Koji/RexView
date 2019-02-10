using System.Collections.Generic;

namespace RexView.Model.Serialize
{
    public interface IRegexCollectionItem<T> : IReferenceItem where T: IReferenceItem
    {
        ICollection<T> RegexReplaceExpressionCollection { get; set; }
    }
}
