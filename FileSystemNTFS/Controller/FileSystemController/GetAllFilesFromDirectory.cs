using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileSystemNTFS.BL.Controller.FileSystemController
{
    public partial class FileSystemController
    {
        public List<string> GetAllFilesFromDirectory(string fullDirPath)
        {
            var mftItme = FileSystem.MFTController.MFT.Entries.SingleOrDefault(x => x.Attributes.FullPath.Equals(fullDirPath));

            if (mftItme == null || !Directory.Exists(fullDirPath))
                return null;

            List<string> fileObjectInDir = new List<string>();

            if (mftItme.Attributes.AttributesRefs.Count > 0)
            {
                foreach (var item in mftItme.Attributes.AttributesRefs)
                {
                    fileObjectInDir.Add(item.FullPath);
                }
            }
            return fileObjectInDir;
        }
    }
}
