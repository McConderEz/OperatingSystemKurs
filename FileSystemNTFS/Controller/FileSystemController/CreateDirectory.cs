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
            string fullPath = Path.Combine(CurrentPath, name);

            if(Directory.Exists(fullPath))
            {
                return false;
            }
            Directory.CreateDirectory(fullPath);

            FileSystem.MFTController.Create(new DirectoryInfo(fullPath), FileSystemObject.Directory);

            Save();

            return true;
        }
    }
}
