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
        public bool CopyTo(string firstFileName, string newFilePath)
        {
            var fullPath = Path.Combine(CurrentPath, firstFileName + ".bin");
            var mftItem = FileSystem.MFTController.MFT.Entries.SingleOrDefault(x => x.Attributes.FullPath.Equals(fullPath));

            if (FileSystem.UserController.CurrentUser.AccountType == AccountType.Administrator ||
                            mftItem.Attributes.AccessFlags.O == AttributeFlags.Modify ||
                            mftItem.Attributes.AccessFlags.O == AttributeFlags.FullControl ||
                            (mftItem.Attributes.OwnerId == FileSystem.UserController.CurrentUser.Id && (mftItem.Attributes.AccessFlags.U == AttributeFlags.FullControl || mftItem.Attributes.AccessFlags.U == AttributeFlags.Modify)) ||
                            (mftItem.Attributes.Groups.Any(FileSystem.UserController.CurrentUser.Groups.Contains) && (mftItem.Attributes.AccessFlags.G == AttributeFlags.FullControl || mftItem.Attributes.AccessFlags.G == AttributeFlags.Modify)))
            {

                if (!File.Exists(fullPath) || mftItem == null || File.Exists(newFilePath))
                {
                    return false;
                }

                CreateFile(Path.GetFileNameWithoutExtension(newFilePath), Path.GetDirectoryName(newFilePath));

                if (mftItem.Attributes.Length > 0)
                {
                    var data = ReadFile(firstFileName);
                    CurrentPath = Path.GetDirectoryName(newFilePath);
                    WriteFile(Path.GetFileName(newFilePath), data);
                    CurrentPath = Path.GetDirectoryName(mftItem.Attributes.FullPath);
                }

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
