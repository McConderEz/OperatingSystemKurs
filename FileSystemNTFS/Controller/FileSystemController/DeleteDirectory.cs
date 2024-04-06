using FileSystemNTFS.BL.Models;
using MultiuserProtection.Domain;
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

            if(!Directory.Exists(fullPath) || mftItem == null)
            {
                return false;
            }

            if (FileSystem.UserController.CurrentUser.AccountType == AccountType.Administrator ||
                        mftItem.Attributes.AccessFlags.O == AttributeFlags.Modify ||
                        mftItem.Attributes.AccessFlags.O == AttributeFlags.FullControl ||
                        (mftItem.Attributes.OwnerId == FileSystem.UserController.CurrentUser.Id &&
                        (mftItem.Attributes.AccessFlags.U == AttributeFlags.FullControl || mftItem.Attributes.AccessFlags.U == AttributeFlags.Modify)) ||
                        (mftItem.Attributes.Groups.Any(FileSystem.UserController.CurrentUser.Groups.Contains) &&
                        (mftItem.Attributes.AccessFlags.G == AttributeFlags.FullControl || mftItem.Attributes.AccessFlags.G == AttributeFlags.Modify)))
            {

                DeleteAllMFTItemOfDirectory(mftItem);
                Directory.Delete(fullPath, true);

                Save();
                return true;
            }
            else
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("Недостаточно прав");
                Console.ResetColor();
                return false;
            }
        }

        public void DeleteAllMFTItemOfDirectory(MFTEntry entry)
        {
            for(int i = 0;i < entry.Attributes.AttributesRefs.Count;i++)
            {
                var deletedItem = FileSystem.MFTController.MFT.Entries.FirstOrDefault(x => x.Attributes.ParentsDirectory.Equals(entry.Attributes.FullPath));
                if(deletedItem.Attributes.AttributesRefs.Count > 0 )
                {
                    DeleteAllMFTItemOfDirectory(deletedItem);
                }
                FileSystem.SuperblockController.FreeAllClustersMFTEntry(deletedItem);
                FileSystem.MFTController.MFT.Entries.Remove(deletedItem);
            }
            FileSystem.MFTController.MFT.Entries.Remove(entry);
        }
    }
}
