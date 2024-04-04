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
        public bool CreateFile(string name, string path)
        {
            if (FileSystem.UserController.CurrentUser.AccountType != AccountType.Guest)
            {
                var fullPath = Path.Combine(path, name + ".bin");

                if (File.Exists(fullPath))
                {
                    Console.ForegroundColor = ConsoleColor.Yellow;
                    Console.WriteLine("Файл с таким именем уже существует.");
                    Console.ResetColor();
                    return false;
                }

                File.Create(fullPath).Close();

                FileSystem.MFTController.Create(new FileInfo(fullPath), FileSystemObject.File);

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
