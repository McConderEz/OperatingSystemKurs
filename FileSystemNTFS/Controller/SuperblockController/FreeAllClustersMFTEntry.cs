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
        public void FreeAllClustersMFTEntry(MFTEntry entry)
        {
            for(int i = 0; i < entry.Attributes.IndexesOnClusterBitmap.Count; i++)
            {
                FreeClusterBitmap(entry.Attributes.IndexesOnClusterBitmap[i].Index);
            }
        }
    }
}
