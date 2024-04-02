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
        public bool DeleteFile(string name)
        {
            var fullPath = Path.Combine(CurrentPath, name);


            if (!File.Exists(fullPath))
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("Файл с таким именем не существует.");
                Console.ResetColor();
                return false;
            }

            var mftItem = FileSystem.MFTController.MFT.Entries.SingleOrDefault(x => x.Attributes.FullPath.Equals(fullPath, StringComparison.OrdinalIgnoreCase));
            if (mftItem == null)
                throw new ArgumentNullException("Такой записи не существует!", nameof(mftItem));

            if (FileSystem.UserController.CurrentUser.AccountType == AccountType.Administrator ||
                        mftItem.Attributes.AccessFlags.O == AttributeFlags.Modify ||
                        mftItem.Attributes.AccessFlags.O == AttributeFlags.FullControl ||
                        (mftItem.Attributes.OwnerId == FileSystem.UserController.CurrentUser.Id &&
                        (mftItem.Attributes.AccessFlags.U == AttributeFlags.FullControl || mftItem.Attributes.AccessFlags.U == AttributeFlags.Modify)) ||
                        (mftItem.Attributes.Groups.Any(FileSystem.UserController.CurrentUser.Groups.Contains) &&
                        (mftItem.Attributes.AccessFlags.G == AttributeFlags.FullControl || mftItem.Attributes.AccessFlags.G == AttributeFlags.Modify)))
            {
                FileSystem.SuperblockController.FreeAllClustersMFTEntry(mftItem);
                DeleteMFTDataFromDir(mftItem);
                FileSystem.MFTController.MFT.Entries.Remove(mftItem);
                File.Delete(fullPath);
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
    }
}
