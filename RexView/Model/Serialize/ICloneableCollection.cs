using System.Collections.Generic;

namespace RexView.Model.Serialize
{
    public interface ICloneableCollection<T> : ICollection<T>, ICloneable<ICloneableCollection<T>>
    {
    }
}
