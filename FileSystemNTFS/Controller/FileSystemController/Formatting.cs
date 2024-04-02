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
                FileSystem.FileSystemPath = @"D:\";
                CurrentPath = FileSystem.FileSystemPath;
                FileSystem.MFTController.MFT.Entries.Clear();
                FileSystem.SuperblockController.ClearBitmap();
                CreateDirectory("FileSystem");
                FileSystem.FileSystemPath = @"D:\FileSystem";
                CurrentPath = FileSystem.FileSystemPath;
                
            }
            else
            {
                FileSystem.FileSystemPath = @"D:\FileSystem";
                CurrentPath = FileSystem.FileSystemPath;
                Directory.Delete(FileSystem.FileSystemPath, true);
                FileSystem.MFTController.MFT.Entries.Clear();
                FileSystem.SuperblockController.ClearBitmap();
                FileSystem.FileSystemPath = @"D:\";
                CurrentPath = FileSystem.FileSystemPath;
                CreateDirectory("FileSystem");
                FileSystem.FileSystemPath = @"D:\FileSystem";
                CurrentPath = FileSystem.FileSystemPath;
                Save();
            }
        }
    }
}
