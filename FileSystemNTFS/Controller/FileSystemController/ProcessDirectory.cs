using FileSystemNTFS.BL.FileSystemOperation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileSystemNTFS.BL.Controller.FileSystemController
{
    public partial class FileSystemController
    {
        public bool ProcessDirectory(string targetDirectory, string searchDirectory, int depth, int maxDepth)
        {
            if (string.Equals(targetDirectory, searchDirectory, StringComparison.OrdinalIgnoreCase))
            {
                return true;
            }

            if (depth >= maxDepth)
            {
                return false;
            }

            try
            {
                string[] subdirectoryEntries = Directory.GetDirectories(searchDirectory);
                foreach (string subdirectory in subdirectoryEntries)
                {
                    if (ProcessDirectory(targetDirectory, subdirectory, depth + 1, maxDepth))
                    {
                        FileSystem = new FileSystem();
                        FileSystem.FileSystemPath = subdirectory;
                        return true;
                    }
                }
            }
            catch (UnauthorizedAccessException)
            {

            }

            return false;
        }
    }
}
