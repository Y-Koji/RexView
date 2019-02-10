using System;
using System.Runtime.Serialization;

namespace RexView.Model.Serialize
{
    [DataContract]
    public class ReferenceItemModel : IReferenceItem
    {
        [DataMember]
        public string Id { get; set; } = string.Empty;
        [DataMember]
        public int Index { get; set; } = 0;
        [DataMember]
        public string Name { get; set; } = string.Empty;
        [DataMember]
        public string Value { get; set; } = string.Empty;

        public ReferenceItemModel(IReferenceItem item)
        {
            this.Id = item.Id ?? Guid.NewGuid().ToString("N");
            this.Index = item.Index;
            this.Name = item.Name ?? string.Empty;
            this.Value = item.Value ?? string.Empty;
        }
    }
}
