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
        public bool CreateDirectory(string name)
        {
            if (FileSystem.UserController.CurrentUser.AccountType != AccountType.Guest)
            {
                string fullPath = Path.Combine(CurrentPath, name);

                if (Directory.Exists(fullPath))
                {
                    Console.ForegroundColor = ConsoleColor.Yellow;
                    Console.WriteLine("Директория с таким именем уже существует.");
                    Console.ResetColor();
                    return false;
                }
                Directory.CreateDirectory(fullPath);

                FileSystem.MFTController.Create(new DirectoryInfo(fullPath), FileSystemObject.Directory);

                var mftItem = FileSystem.MFTController.MFT.Entries.SingleOrDefault(x => x.Attributes.FullPath.Equals(fullPath));
                AddMFTDataToDir(mftItem);

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
