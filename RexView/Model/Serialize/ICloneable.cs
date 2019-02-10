using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RexView.Model.Serialize
{
    public interface ICloneable<T>
    {
        T Clone();
    }
}
