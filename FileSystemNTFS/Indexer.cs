using Newtonsoft.Json;
using System.Runtime.Serialization;

namespace FileSystemNTFS.BL
{
    [DataContract]
    public class Indexer
    {
        public int StartIndex { get; set; }
        public int EndIndex { get; set; }

        public Indexer()
        {
            StartIndex = -1;
            EndIndex = -1;
        }

        [JsonConstructor]
        public Indexer(int startIndex, int endIndex)
        {
            StartIndex = startIndex;
            EndIndex = endIndex;
        }
    }
}