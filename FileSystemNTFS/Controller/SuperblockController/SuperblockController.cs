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
        public Superblock Superblock { get; set; }

        public SuperblockController()
        {
            Superblock = new Superblock();
        }

    }
}
