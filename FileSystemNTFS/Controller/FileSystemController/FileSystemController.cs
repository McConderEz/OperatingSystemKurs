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
        public FileSystem FileSystem { get; set; }
        public string CurrentPath { get; set; }

        public FileSystemController()
        {
            
            if (RootDirExists(@"D:\FileSystem"))
            {
                
            }
            else
            {
                DeleteMetaData();
                FileSystem = new FileSystem();
                Formatting();
                Save();
            }


            CurrentPath = FileSystem.FileSystemPath;
        }


        public void Save()
        {
            FileSystem.SuperblockController.Save();
            FileSystem.MFTController.Save();
        }

        public void DeleteMetaData()
        {
            var mftPath = Path.Combine(Environment.CurrentDirectory, "MFT.json");
            var superblockPath = Path.Combine(Environment.CurrentDirectory, "Superblock.json");
            var usersPath = Path.Combine(Environment.CurrentDirectory, "List`1.json");

            if (File.Exists(mftPath))
            {
                File.Delete(mftPath);
            }

            if (File.Exists(superblockPath))
            {
                File.Delete(superblockPath);
            }

            if (File.Exists(usersPath))
            {
                File.Delete(usersPath);
            }
        }
       
    }
}
