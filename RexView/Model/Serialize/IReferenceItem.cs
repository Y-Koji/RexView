﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RexView.Model.Serialize
{
    public interface IReferenceItem
    {
        string Id { get; set; }
        int Index { get; set; }
        string Name { get; set; }
        string Value { get; set; }
    }
}
