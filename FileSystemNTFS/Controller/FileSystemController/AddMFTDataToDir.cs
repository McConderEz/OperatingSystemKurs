using FileSystemNTFS.BL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileSystemNTFS.BL.Controller.FileSystemController
{
    public partial class FileSystemController
    {
        public bool AddMFTDataToDir(MFTEntry entry)
        {
            var dirMft = FileSystem.MFTController.MFT.Entries.SingleOrDefault(x => x.Attributes.FullPath.Equals(entry.Attributes.ParentsDirectory));
            if (dirMft == null)
            {
                return false;
            }

            dirMft.Attributes.AttributesRefs.Add(entry.Attributes);

            return true;
        }
    }
}
