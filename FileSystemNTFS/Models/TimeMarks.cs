using Newtonsoft.Json;
using System.Runtime.Serialization;

namespace FileSystemNTFS.BL.Models
{
    [DataContract]
    public class TimeMarks
    {
        [DataMember]
        public DateTime CreationTime { get; init; }
        [DataMember]
        public DateTime ModificationTime { get; set; }
        [DataMember]
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