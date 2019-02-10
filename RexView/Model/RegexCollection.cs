using RexView.Model.Serialize;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Json;
using System.Text;
using System.Threading.Tasks;

namespace RexView.Model
{
    public class RegexCollection : DispatchObservableCollection<RegexCollectionItem>, IDisposable
    {
        private static string FILE_NAME { get; } = "map.json";
        private static DataContractJsonSerializer Serializer = new DataContractJsonSerializer(typeof(RegexCollectionItemModel[]), new Type[] { typeof(CloneableList<ReferenceItemModel>) });

        private RegexCollection() { }

        public static async Task<RegexCollection> LoadAsync()
        {
            var collection = new RegexCollection();

            var models = await Task.Run(() =>
            {
                if (!File.Exists(FILE_NAME))
                {
                    return Enumerable.Empty<RegexCollectionItemModel>();
                }

                using (var fs = File.OpenRead(FILE_NAME))
                {
                    try
                    {
                        return Serializer.ReadObject(fs) as RegexCollectionItemModel[];
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine(e.Message);
                        Console.WriteLine(e.StackTrace);

                        return Enumerable.Empty<RegexCollectionItemModel>();
                    }
                }
            });

            foreach (var model in models)
            {
                var item = new RegexCollectionItem
                {
                    Id = model.Id ?? string.Empty,
                    Index = model.Index,
                    Name = model.Name ?? string.Empty,
                    Value = model.Value ?? string.Empty,
                    RegexReplaceExpressionCollection = new DispatchObservableCollection<IReferenceItem>(),
                };

                foreach (var reference in model.RegexReplaceExpressionCollection ?? new DispatchObservableCollection<ReferenceItemModel>())
                {
                    item.RegexReplaceExpressionCollection.Add(reference);
                }

                collection.Add(item);
            }

            return collection;
        }

        public void Save()
        {
            var model = this.Select(x => new RegexCollectionItemModel
            {
                Id = x.Id,
                Index = x.Index,
                Name = x.Name,
                Value = x.Value,
                RegexReplaceExpressionCollection =
                    x.RegexReplaceExpressionCollection
                     .OfType<ReferenceItemModel>()
                     .Aggregate(new CloneableList<ReferenceItemModel>(),
                                (collection, item) => { collection.Add(item); return collection; }),
            }).ToArray();

            using (FileStream fs = File.Create(FILE_NAME))
            {
                Serializer.WriteObject(fs, model);
            }
        }

        public void Dispose()
        {
            Save();
        }
    }
}
