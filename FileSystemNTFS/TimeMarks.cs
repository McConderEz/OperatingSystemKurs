using Newtonsoft.Json;
using System.Runtime.Serialization;

namespace FileSystemNTFS.BL
{
    [DataContract]
    public class TimeMarks
    {
        public DateTime CreationTime { get; init; }
        public DateTime ModificationTime { get; set; }
        public DateTime AccessTime { get; set; }


        public TimeMarks()
        {
            CreationTime = DateTime.Now;
            ModificationTime = DateTime.Now;
            AccessTime = DateTime.Now;
        }

        [JsonConstructor]
        public TimeMarks(DateTime creationTime, DateTime modificationTime, DateTime accessTime)
        {
            CreationTime = creationTime;
            ModificationTime = modificationTime;
            AccessTime = accessTime;
        }
    }
}