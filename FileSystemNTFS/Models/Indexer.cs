using Newtonsoft.Json;
using System.Runtime.Serialization;

namespace FileSystemNTFS.BL
{
    [DataContract]
    public class Indexer
    {
        [DataMember]
        public int Index { get; set; }

        public Indexer()
        {
            Index = -1;
        }

        [JsonConstructor]
        public Indexer(int index)
        {
            Index = index;
        }
    }
}