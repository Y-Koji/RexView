using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace RexView.Model.Serialize
{
    [DataContract]
    public class RegexCollectionItemModel : IRegexCollectionItem<ReferenceItemModel>
    {
        [DataMember]
        public string Id { get; set; }
        [DataMember]
        public int Index { get; set; }
        [DataMember]
        public string Name { get; set; }
        [DataMember]
        public string Value { get; set; }
        [DataMember]
        public ICollection<ReferenceItemModel> RegexReplaceExpressionCollection { get; set; }
    }
}
