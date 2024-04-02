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
        public void MoveDirectory(string dirName, string newDirPath)
        {
            var oldfullPath = Path.Combine(CurrentPath, dirName);
            var mftItem = FileSystem.MFTController.MFT.Entries.SingleOrDefault(x => x.Attributes.FullPath.Equals(oldfullPath));

            if (mftItem == null || !Directory.Exists(oldfullPath))
                return;

            if (FileSystem.UserController.CurrentUser.AccountType == AccountType.Administrator ||
                            mftItem.Attributes.AccessFlags.O == AttributeFlags.Modify ||
                            mftItem.Attributes.AccessFlags.O == AttributeFlags.FullControl ||
                            (mftItem.Attributes.OwnerId == FileSystem.UserController.CurrentUser.Id && (mftItem.Attributes.AccessFlags.U == AttributeFlags.FullControl || mftItem.Attributes.AccessFlags.U == AttributeFlags.Modify)) ||
                            (mftItem.Attributes.Groups.Any(FileSystem.UserController.CurrentUser.Groups.Contains) && (mftItem.Attributes.AccessFlags.G == AttributeFlags.FullControl || mftItem.Attributes.AccessFlags.G == AttributeFlags.Modify)))
            {
                UpdateMFTEntryFromDirectory(mftItem);
                UpdateMFTEntryFromDirectory(mftItem);
                mftItem.Attributes.FullPath = newDirPath;
                mftItem.Attributes.ParentsDirectory = mftItem.Attributes.GetParentsDir(newDirPath);
                FileSystem.MFTController.Update(mftItem);
                Directory.Move(oldfullPath, newDirPath);
                Save();
            }
            else
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("Недостаточно прав!");
                Console.ResetColor();
            }
        }
    }
}
