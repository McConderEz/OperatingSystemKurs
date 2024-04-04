using FileSystemNTFS.BL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileSystemNTFS.BL.Controller.SuperblockController
{
    public partial class SuperblockController
    {
        public void MarkClustersAsUsedForLargeFile(MFTEntry? mftItem, byte[] dataBytes, int dataSize, int clusterIndex)
        {
            int l = 0; // Индекс массива данных на запись 
            for (int j = clusterIndex; j < Superblock.ClusterBitmap.Length; j++)
            {
                if (IsClusterFree(j))
                {
                    for (int k = 0; k < dataBytes.Length; k++)
                    {
                        if (k < 4096 && l < dataBytes.Length) // Запись байт в кластер
                        {
                            Superblock.ClusterBitmap[j][k] = dataBytes[l++];
                            dataSize--;
                        }
                        else
                        {
                            // Кластер заполнен и MFT запись получает индексы на область карты кластеров, принадлежащие данному файлу
                            mftItem.Attributes.IndexesOnClusterBitmap.Add(new Indexer(j));
                            break;
                        }
                    }
                }

                if (dataSize == 0) // Вся информация записана в кластеры
                {
                    break;
                }

            }
        }
    }
}
