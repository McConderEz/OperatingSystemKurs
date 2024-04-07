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
        public StringBuilder? ReadFile(string name)
        {
            var fullPath = Path.Combine(CurrentPath, name + ".bin");

            var mftItem = FileSystem.MFTController.MFT.Entries.SingleOrDefault(x => x.Attributes.FullPath.Equals(fullPath));

            if(mftItem == null || !File.Exists(fullPath))
                return null;

            if (FileSystem.UserController.CurrentUser.AccountType == AccountType.Administrator ||
                        mftItem.Attributes.AccessFlags.O == AttributeFlags.Read ||
                        mftItem.Attributes.AccessFlags.O == AttributeFlags.Modify ||
                        mftItem.Attributes.AccessFlags.O == AttributeFlags.FullControl ||
                        (mftItem.Attributes.OwnerId == FileSystem.UserController.CurrentUser.Id && (mftItem.Attributes.AccessFlags.U == AttributeFlags.Read
                        || mftItem.Attributes.AccessFlags.U == AttributeFlags.Modify || mftItem.Attributes.AccessFlags.U == AttributeFlags.FullControl)) ||
                        (mftItem.Attributes.Groups.Any(FileSystem.UserController.CurrentUser.Groups.Contains) && (mftItem.Attributes.AccessFlags.G == AttributeFlags.Read
                        || mftItem.Attributes.AccessFlags.G == AttributeFlags.Modify || mftItem.Attributes.AccessFlags.G == AttributeFlags.FullControl)))
            {

                string data = "";

                using (FileStream fs = new FileStream(fullPath, FileMode.Open, FileAccess.Read))
                {
                    using (StreamReader sr = new StreamReader(fs))
                    {
                        data = sr.ReadToEnd();
                    }
                }

                FileSystem.MFTController.Update(mftItem);

                return new StringBuilder(data);
            }
            else
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("Недостаточно прав");
                Console.ResetColor();
                return null;
            }
        }
    }
}
