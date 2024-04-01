using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FileSystemNTFS.BL.FileSystemOperation;

namespace FileSystemNTFS.BL.Controller.FileSystemController
{
    public partial class FileSystemController
    {
        public FileSystem FileSystem { get; private set; }
        public string CurrentPath { get; set; }

        public FileSystemController()
        {
            FileSystem = new FileSystem();
            CurrentPath = FileSystem.FileSystemPath;
        }


        public void Save()
        {
            FileSystem.SuperblockController.Save();
            FileSystem.MFTController.Save();
        }

       
    }
}
