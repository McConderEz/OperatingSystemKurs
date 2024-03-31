using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileSystemNTFS.BL.Controller.SuperblockController
{
    public partial class SuperblockController
    {
        public int FindFreeCluster()
        {
            for(int i = 0;i < Superblock.ClusterBitmap.Length;i++)
            {
                if (Superblock.ClusterBitmap[i] == 0) 
                    return i;
            }

            return -1;
        }
    }
}
