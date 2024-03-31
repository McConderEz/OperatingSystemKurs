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
        public readonly ulong TotalDiskSize = 2147483648; // TODO: указать максимальное количество свободного места

        public const uint ClusterUnitSize = 4096;

        public string Id { get; } = Guid.NewGuid().ToString();

        public ulong UsedMemory { get; private set; } = 0;
        public int ClusterBitmapIndex { get; private set; } = 0;
        public long BitmapOffset { get; set; } = 0;
        public byte[] ClusterBitmap { get; private set; }

        public Superblock()
        {
            ClusterBitmap = new byte[TotalDiskSize / ClusterUnitSize];
        }

        [JsonConstructor]
        public Superblock(byte[] clusterBitmap, string id, ulong userMemory)
        {
            ClusterBitmap = clusterBitmap;
            Id = id;
            UsedMemory = userMemory;
        }

        //TODO: Проверка свободен ли кластер, Пометить кластер как занятый,
        //Пометить кластер как занятый для большого файла,
        //Пометить кластер как свободный
        //Найти свободный кластер
        //Очистить Карту кластеров
    }
}
