using RexView.Model.Serialize;
using System;
using System.Collections.Generic;

namespace RexView.Model.DataType
{
    [Serializable]
    public class CloneableList<T> : List<T>, ICloneableCollection<T>
    {
        public ICloneableCollection<T> Clone()
        {
            ICloneableCollection<T> collection = new CloneableList<T>();

            this.ForEach(collection.Add);

            return collection;
        }
    }
}
