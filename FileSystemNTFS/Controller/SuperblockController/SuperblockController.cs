using FileSystemNTFS.BL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileSystemNTFS.BL.Controller.SuperblockController
{
    public partial class SuperblockController : ControllerBase
    {
        public Superblock Superblock { get; private set; }

        public SuperblockController()
        {
            Superblock = Load();
        }

        public Superblock Load()
        {
            return Load<Superblock>() ?? new Superblock();
        }

        public void Save()
        {
            Save(Superblock);
        }
    }
}
