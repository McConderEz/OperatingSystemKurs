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
        public void CopyDirTo(string dirName, string newFullPath)
        {
            var fullPath = Path.Combine(CurrentPath, dirName);
            var mftItem = FileSystem.MFTController.MFT.Entries.SingleOrDefault(x => x.Attributes.FullPath.Equals(fullPath));

            if (mftItem == null || !Directory.Exists(fullPath))
                return;

            if (FileSystem.UserController.CurrentUser.AccountType == AccountType.Administrator ||
                            mftItem.Attributes.AccessFlags.O == AttributeFlags.Modify ||
                            mftItem.Attributes.AccessFlags.O == AttributeFlags.FullControl ||
                            (mftItem.Attributes.OwnerId == FileSystem.UserController.CurrentUser.Id && (mftItem.Attributes.AccessFlags.U == AttributeFlags.FullControl || mftItem.Attributes.AccessFlags.U == AttributeFlags.Modify)) ||
                            (mftItem.Attributes.Groups.Any(FileSystem.UserController.CurrentUser.Groups.Contains) && (mftItem.Attributes.AccessFlags.G == AttributeFlags.FullControl || mftItem.Attributes.AccessFlags.G == AttributeFlags.Modify)))
            {

                string oldCurrentPath = CurrentPath;
                CurrentPath = newFullPath;
                CreateDirectory(dirName);

                CurrentPath = fullPath;
                foreach (string file in Directory.GetFiles(fullPath))
                {
                    string targetFile = Path.Combine(newFullPath, dirName, Path.GetFileName(file));
                    CopyTo(Path.GetFileNameWithoutExtension(file), targetFile);
                }

                foreach (string directory in Directory.GetDirectories(fullPath))
                {
                    string targetSubdirectory = Path.Combine(newFullPath, dirName, Path.GetFileName(directory));
                    CopyDirTo(directory, targetSubdirectory);
                }

                CurrentPath = oldCurrentPath;
                Save();
            }
            else
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("Недостаточно прав");
                Console.ResetColor();
            }
        }
    }
}
