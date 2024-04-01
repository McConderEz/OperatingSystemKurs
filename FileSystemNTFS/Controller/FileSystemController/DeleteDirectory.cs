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
        /// <summary>
        /// Рекурсивное удаление директории
        /// </summary>
        /// <param name="dirName">имя директории</param>
        /// <returns></returns>
        public bool DeleteDirectory(string dirName)
        {
            var fullPath = Path.Combine(CurrentPath, dirName);
            var mftItem = FileSystem.MFTController.MFT.Entries.SingleOrDefault(x => x.Attributes.FullPath.Equals(fullPath));

            if(!Directory.Exists(dirName) || mftItem == null)
            {
                return false;
            }

            DeleteAllMFTItemOfDirectory(mftItem);
            Directory.Delete(fullPath, true);

            Save();
            return true;
        }

        public void DeleteAllMFTItemOfDirectory(MFTEntry entry)
        {
            for(int i = 0;i < entry.Attributes.AttributesRefs.Count;i++)
            {
                var deletedItem = FileSystem.MFTController.MFT.Entries.SingleOrDefault(x => x.Attributes.ParentsDirectory.Equals(entry.Attributes.FileName));
                FileSystem.SuperblockController.FreeAllClustersMFTEntry(deletedItem);
                FileSystem.MFTController.MFT.Entries.Remove(deletedItem);
            }
            FileSystem.MFTController.MFT.Entries.Remove(entry);
        }
    }
}
