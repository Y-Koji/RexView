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
    }
}
