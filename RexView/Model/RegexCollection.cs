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
        private static DataContractJsonSerializer Serializer = new DataContractJsonSerializer(typeof(RegexCollectionItemModel[]));

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
                    return Serializer.ReadObject(fs) as RegexCollectionItemModel[];
                }
            });

            foreach (var model in models)
            {
                collection.Add(new RegexCollectionItem
                {
                    Id = model.Id,
                    Index = model.Index,
                    Name = model.Name,
                    Regex = model.Regex,
                });
            }

            return collection;
        }

        public void Save()
        {
            using (FileStream fs = File.Create(FILE_NAME))
            {
                var model = this.Select(x => new RegexCollectionItemModel
                {
                    Id = x.Id,
                    Index = x.Index,
                    Name = x.Name,
                    Regex = x.Regex,
                }).ToArray();

                Serializer.WriteObject(fs, model);
            }
        }

        public void Dispose()
        {
            Save();
        }
    }
}
