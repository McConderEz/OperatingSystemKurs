using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileSystemNTFS.BL.Controller.FileSystemController
{
    public partial class FileSystemController
    {
        public bool DeleteFile(string name)
        {
            var fullPath = Path.Combine(CurrentPath, name);

            if(!File.Exists(fullPath))
            {
                return false;
            }

            File.Delete(fullPath);



            return true;
        }
    }
}
