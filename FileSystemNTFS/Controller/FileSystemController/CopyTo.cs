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

            if(!File.Exists(fullPath) || mftItem == null || File.Exists(newFilePath))
            {
                return false;
            }

            CreateFile(Path.GetFileName(newFilePath), Path.GetDirectoryName(newFilePath));

            if(mftItem.Attributes.Length > 0)
            {
                var data = ReadFile(firstFileName);
                CurrentPath = Path.GetDirectoryName(newFilePath);
                WriteFile(Path.GetFileName(newFilePath), data);
                CurrentPath = Path.GetDirectoryName(mftItem.Attributes.FullPath);
            }

            Save();

            return true;
        }
    }
}
