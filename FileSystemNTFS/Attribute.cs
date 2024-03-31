using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileSystemNTFS.BL
{
    public class Attribute
    {
        public string FileName { get; private set; }
        public string FullPath { get; private set; }
        public string ParentsDirectory { get; private set; }
        public ulong Length { get; set; }
        public uint OwnerId { get; private set; }
        public List<uint> GroupId { get; private set; }
        public uint BlocksCount { get; private set; }
        public TimeMarks TimeMarks { get; private set; }
        public List<Indexer> IndexesOnClusterBitmap { get; private set; }
        public UsersAccessFlags AccessFlags { get; private set; }


        public Attribute(string fileName, string fullPath,
                         ulong length, uint ownerId, List<uint> groupId, uint blocksCount,
                         TimeMarks timeMarks, List<Indexer> indexesOnClusterBitmap, UsersAccessFlags accessFlags)
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
        }

        private string? GetParentsDir(string fullPath)
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
                         TimeMarks timeMarks, List<Indexer> indexesOnClusterBitmap, UsersAccessFlags accessFlags)
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
        }
    }
}
