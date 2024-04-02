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
        public void RenameDirectory(string oldDirName, string newDirName)
        {
            var oldfullPath = Path.Combine(CurrentPath, oldDirName);
            var newfullPath = Path.Combine(CurrentPath, newDirName);

            var mftItem = FileSystem.MFTController.MFT.Entries.SingleOrDefault(x => x.Attributes.FullPath.Equals(oldfullPath));

            if(!Directory.Exists(oldfullPath) || mftItem == null)
                return;

            if (FileSystem.UserController.CurrentUser.AccountType == AccountType.Administrator ||
                        mftItem.Attributes.AccessFlags.O == AttributeFlags.Modify ||
                        mftItem.Attributes.AccessFlags.O == AttributeFlags.FullControl ||
                        (mftItem.Attributes.OwnerId == FileSystem.UserController.CurrentUser.Id && (mftItem.Attributes.AccessFlags.U == AttributeFlags.FullControl || mftItem.Attributes.AccessFlags.U == AttributeFlags.Modify)) ||
                        (mftItem.Attributes.Groups.Any(FileSystem.UserController.CurrentUser.Groups.Contains) && (mftItem.Attributes.AccessFlags.G == AttributeFlags.FullControl || mftItem.Attributes.AccessFlags.G == AttributeFlags.Modify)))
            {
                UpdateMFTEntryFromDirectory(mftItem);
                Directory.Move(oldfullPath, newfullPath);
                mftItem.Attributes.FullPath = newfullPath;
                mftItem.Attributes.ParentsDirectory = mftItem.Attributes.GetParentsDir(newfullPath);
                FileSystem.MFTController.Update(mftItem);

                Save();
            }
            else
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("Недостаточно прав!");
                Console.ResetColor();
            }
        }

        public void UpdateMFTEntryFromDirectory(MFTEntry entry)
        {
            for(int i = 0; i < entry.Attributes.AttributesRefs.Count; i++)
            {
                var updatedEntry = FileSystem.MFTController.MFT.Entries.SingleOrDefault(x => x.Attributes.ParentsDirectory.Equals(entry.Attributes.FileName));
                updatedEntry.Attributes.FullPath = entry.Attributes.FullPath + updatedEntry.Attributes.FileName;
                updatedEntry.Attributes.ParentsDirectory = updatedEntry.Attributes.GetParentsDir(updatedEntry.Attributes.FullPath);
                //TODO:Проверить изменяемость ссылок на атрибуты атрибутов мфт записей файлов
            }
        }
    }
}
