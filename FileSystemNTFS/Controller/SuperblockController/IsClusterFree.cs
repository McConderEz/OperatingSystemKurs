﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileSystemNTFS.BL.Controller.SuperblockController
{
    public partial class SuperblockController
    {
        public bool IsClusterFree(int clusterIndex) => Superblock.ClusterBitmap[clusterIndex][0] == 0;
    }
}
