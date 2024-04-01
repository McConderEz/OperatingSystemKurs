using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileSystemNTFS.BL.Controller.SuperblockController
{
    public partial class SuperblockController
    {
        public void ClearBitmap()
        {
            Superblock.ClusterBitmap = new byte[32000][];

            for (int i = 0; i < 32000; i++)
            {
                Superblock.ClusterBitmap[i] = new byte[4096];
                for (int j = 0; j < 4096; j++)
                {
                    Superblock.ClusterBitmap[i][j] = 0;
                }
            }
        }
    }
}
