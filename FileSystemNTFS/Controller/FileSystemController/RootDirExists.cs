﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileSystemNTFS.BL.Controller.FileSystemController
{
    public partial class FileSystemController
    {
        public bool RootDirExists(string path)
        {
            return ProcessDirectory(path, "D:\\", 1, 5);
        }
    }
}
