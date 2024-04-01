using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileSystemNTFS.BL.Controller.FileSystemController
{
    public partial class FileSystemController
    {
        public bool RenameFile(string oldName, string newName)
        {
            if (string.IsNullOrWhiteSpace(oldName) || string.IsNullOrWhiteSpace(newName))
                return false;

            var fullOldPath = Path.Combine(CurrentPath, oldName + ".bin");
            var fullNewPath = Path.Combine(CurrentPath, newName + ".bin");

            File.Move(fullOldPath, fullNewPath);
            var mftItem = FileSystem.MFTController.MFT.Entries.SingleOrDefault(x => x.Attributes.FullPath.Equals(fullOldPath));

            if(mftItem != null)
            {
                mftItem.Attributes.TimeMarks.ModificationTime = DateTime.Now;
                mftItem.Attributes.FileName = newName;
                mftItem.Attributes.FullPath = fullNewPath;
                FileSystem.MFTController.Update(mftItem);
            }

            Save();

            return true;
        }
    }
}
