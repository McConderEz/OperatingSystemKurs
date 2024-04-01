using FileSystemNTFS.BL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileSystemNTFS.BL.Controller.FileSystemController
{
    public partial class FileSystemController
    {
        public void Formatting()
        {
            if (!Directory.Exists(@"D:\FileSystem"))
            {
                FileSystem.FileSystemPath = @"D:\FileSystem";
                Directory.CreateDirectory(FileSystem.FileSystemPath);
                FileSystem.MFTController.MFT.Entries.Clear();
                FileSystem.SuperblockController.ClearBitmap();
            }
            else
            {
                FileSystem.FileSystemPath = @"D:\FileSystem";
                Directory.Delete(FileSystem.FileSystemPath, true);
                Directory.CreateDirectory(FileSystem.FileSystemPath);
                FileSystem.MFTController.MFT.Entries.Clear();
                FileSystem.SuperblockController.ClearBitmap();
                Save();
            }
        }
    }
}
