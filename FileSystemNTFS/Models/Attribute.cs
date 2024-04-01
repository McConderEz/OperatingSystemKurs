using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace FileSystemNTFS.BL.Models
{
    [DataContract]
    public class Attribute
    {
        [DataMember]
        public string FileName { get; set; }
        [DataMember]
        public string FullPath { get; set; }
        [DataMember]
        public string ParentsDirectory { get; set; }
        [DataMember]
        public ulong Length { get; set; }
        [DataMember]
        public uint OwnerId { get; private set; }
        [DataMember]
        public List<uint> GroupId { get; private set; }
        [DataMember]
        public uint BlocksCount { get; set; }
        [DataMember]
        public TimeMarks TimeMarks { get; private set; }
        [DataMember]
        public List<Indexer> IndexesOnClusterBitmap { get; private set; }
        [DataMember]
        public UsersAccessFlags AccessFlags { get; set; }
        public List<Attribute> AttributesRefs { get; set; }

        public Attribute(string fileName, string fullPath,
                         ulong length, uint ownerId, List<uint> groupId, uint blocksCount,
                         TimeMarks timeMarks, List<Indexer> indexesOnClusterBitmap, UsersAccessFlags accessFlags, List<Attribute> attributesRefs)
        {
            FileName = fileName;
            FullPath = fullPath;
            ParentsDirectory = GetParentsDir(fullPath);
            Length = length;
            OwnerId = ownerId;
            GroupId = groupId;
            BlocksCount = blocksCount;
            TimeMarks = timeMarks;
            IndexesOnClusterBitmap = indexesOnClusterBitmap;
            AccessFlags = accessFlags;
            AttributesRefs = attributesRefs;
        }

        public string? GetParentsDir(string fullPath)
        {
            char[] separators = { '\\', '/', '\\' };
            int lastIndex = fullPath.LastIndexOfAny(separators);
            if (lastIndex >= 0)
            {
                string result = fullPath.Substring(0, lastIndex);
                return result;
            }

            return "";
        }

        [JsonConstructor]
        public Attribute(string fileName, string fullPath, string parentsDirectory,
                         ulong length, uint ownerId, List<uint> groupId, uint blocksCount,
                         TimeMarks timeMarks, List<Indexer> indexesOnClusterBitmap, UsersAccessFlags accessFlags, List<Attribute> attributesRefs)
        {
            FileName = fileName;
            FullPath = fullPath;
            ParentsDirectory = parentsDirectory;
            Length = length;
            OwnerId = ownerId;
            GroupId = groupId;
            BlocksCount = blocksCount;
            TimeMarks = timeMarks;
            IndexesOnClusterBitmap = indexesOnClusterBitmap;
            AccessFlags = accessFlags;
            AttributesRefs = attributesRefs;
        }
    }
}
