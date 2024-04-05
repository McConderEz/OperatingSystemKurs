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
        public void ChangeMode(string fullPath, UsersAccessFlags usersAccessFlags)
        {
            var mftEntry = FileSystem.MFTController.MFT.Entries.SingleOrDefault(e => e.Attributes.FullPath.Equals(fullPath));
            if (mftEntry != null)
            {
                if (FileSystem.UserController.CurrentUser.AccountType == AccountType.Administrator ||
                    mftEntry.Attributes.AccessFlags.O == AttributeFlags.ChangeMode ||
                    mftEntry.Attributes.AccessFlags.O == AttributeFlags.FullControl ||
                    (mftEntry.Attributes.OwnerId == FileSystem.UserController.CurrentUser.Id && (mftEntry.Attributes.AccessFlags.U == AttributeFlags.FullControl || mftEntry.Attributes.AccessFlags.U == AttributeFlags.ChangeMode)) ||
                    (mftEntry.Attributes.Groups.Any(FileSystem.UserController.CurrentUser.Groups.Contains) && (mftEntry.Attributes.AccessFlags.G == AttributeFlags.FullControl || mftEntry.Attributes.AccessFlags.G == AttributeFlags.ChangeMode)))
                {
                    mftEntry.Attributes.ChangeMode(usersAccessFlags);
                    var mftDir = FileSystem.MFTController.MFT.Entries.SingleOrDefault(x => x.Attributes.FullPath.Equals(mftEntry.Attributes.ParentsDirectory));
                    mftDir.Attributes.AttributesRefs.SingleOrDefault(x => x.FileName.Equals(mftEntry.Attributes.FileName)).AccessFlags = usersAccessFlags;
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
}
