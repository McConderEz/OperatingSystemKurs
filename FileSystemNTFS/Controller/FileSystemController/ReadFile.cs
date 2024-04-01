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

            string data = "";

            using(FileStream fs = new FileStream(fullPath, FileMode.Open, FileAccess.Read))
            {
                using(StreamReader sr = new StreamReader(fs))
                {
                    data = sr.ReadToEnd();
                }
            }

            FileSystem.MFTController.Update(mftItem);

            return new StringBuilder(data);
        }
    }
}
