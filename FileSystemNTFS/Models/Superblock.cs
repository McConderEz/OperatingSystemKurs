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
    public class Superblock
    {        
        public readonly string OperatingSystemName = "Stellar OS";
        public readonly ulong TotalDiskSize = 131072000;

        public const uint ClusterUnitSize = 4096;

        [DataMember]
        public string Id { get; } = Guid.NewGuid().ToString();

        [DataMember]
        public ulong UsedMemory { get; private set; } = 0;
        public int ClusterBitmapIndex { get; private set; } = 0;
        public long BitmapOffset { get; set; } = 0;
        [DataMember]
        public byte[][] ClusterBitmap { get; set; }

        public Superblock()
        {
            ClusterBitmap = new byte[32000][];
            InitClusterBitmap();
            
        }

        [JsonConstructor]
        public Superblock(byte[][] clusterBitmap, string id, ulong userMemory)
        {
            ClusterBitmap = clusterBitmap;
            Id = id;
            UsedMemory = userMemory;
        }

        public void InitClusterBitmap()
        {
            for(int i = 0; i < ClusterBitmap.Length; i++)
            {
                ClusterBitmap[i] = new byte[ClusterUnitSize];
            }
        }
    }
}
