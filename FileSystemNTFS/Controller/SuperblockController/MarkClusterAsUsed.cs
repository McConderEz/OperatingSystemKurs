using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileSystemNTFS.BL.Controller.SuperblockController
{
    public partial class SuperblockController
    {
        public void MarkClusterAsUsed(byte[] dataBytes, int clusterIndex)
        {
            for (int j = 0; j < dataBytes.Length; j++)
            {
                Superblock.ClusterBitmap[clusterIndex][j] = dataBytes[j];
            }
        }
    }
}
