using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileSystemNTFS.BL.Controller.FileSystemController
{
    public partial class FileSystemController
    {
        public void MoveDirectory(string dirName, string newDirPath)
        {
            var oldfullPath = Path.Combine(CurrentPath, dirName);
            var mftItem = FileSystem.MFTController.MFT.Entries.SingleOrDefault(x => x.Attributes.FullPath.Equals(oldfullPath));

            if (mftItem == null || !Directory.Exists(oldfullPath))
                return;

            UpdateMFTEntryFromDirectory(mftItem);
            UpdateMFTEntryFromDirectory(mftItem);
            mftItem.Attributes.FullPath = newDirPath;
            mftItem.Attributes.ParentsDirectory = mftItem.Attributes.GetParentsDir(newDirPath);
            FileSystem.MFTController.Update(mftItem);
            Directory.Move(oldfullPath, newDirPath);            
            Save();
        }
    }
}
