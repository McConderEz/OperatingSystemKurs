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
        public bool MoveTo(string fileName, string newDirPath)
        {
            var oldFullPath = Path.Combine(CurrentPath, fileName + ".bin");
            var mftItem = FileSystem.MFTController.MFT.Entries.SingleOrDefault(x => x.Attributes.FullPath.Equals(oldFullPath));

            if (!File.Exists(oldFullPath) || mftItem == null)
                return false;

            if (FileSystem.UserController.CurrentUser.AccountType == AccountType.Administrator ||
                            mftItem.Attributes.AccessFlags.O == AttributeFlags.Modify ||
                            mftItem.Attributes.AccessFlags.O == AttributeFlags.FullControl ||
                            (mftItem.Attributes.OwnerId == FileSystem.UserController.CurrentUser.Id && (mftItem.Attributes.AccessFlags.U == AttributeFlags.FullControl || mftItem.Attributes.AccessFlags.U == AttributeFlags.Modify)) ||
                            (mftItem.Attributes.Groups.Any(FileSystem.UserController.CurrentUser.Groups.Contains) && (mftItem.Attributes.AccessFlags.G == AttributeFlags.FullControl || mftItem.Attributes.AccessFlags.G == AttributeFlags.Modify)))
            {
                DeleteMFTDataFromDir(mftItem);
                var newFullPath = Path.Combine(newDirPath, fileName + ".bin");
                File.Move(oldFullPath, newFullPath);
                mftItem.Attributes.FullPath = newFullPath;
                mftItem.Attributes.ParentsDirectory = mftItem.Attributes.GetParentsDir(newFullPath);
                AddMFTDataToDir(mftItem);
                FileSystem.MFTController.Update(mftItem);
                Save();

                return true;
            }
            else
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("Недостаточно прав!");
                Console.ResetColor();
                return false;
            }
        }
    }
}
