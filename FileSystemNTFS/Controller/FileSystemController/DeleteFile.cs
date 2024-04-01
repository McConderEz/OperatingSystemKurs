using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileSystemNTFS.BL.Controller.FileSystemController
{
    public partial class FileSystemController
    {
        public bool DeleteFile(string name)
        {
            var fullPath = Path.Combine(CurrentPath, name);

            if(!File.Exists(fullPath))
            {
                return false;
            }

            var mftItem = FileSystem.MFTController.MFT.Entries.SingleOrDefault(x => x.Attributes.FullPath.Equals(fullPath, StringComparison.OrdinalIgnoreCase));
            if(mftItem == null)
               throw new ArgumentNullException("Такой записи не существует!",nameof(mftItem));

            FileSystem.SuperblockController.FreeAllClustersMFTEntry(mftItem);
            DeleteMFTDataFromDir(mftItem);
            FileSystem.MFTController.MFT.Entries.Remove(mftItem);
            File.Delete(fullPath);

            Save();

            return true;
        }
    }
}
