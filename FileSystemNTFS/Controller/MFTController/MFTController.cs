using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FileSystemNTFS.BL.Models;

namespace FileSystemNTFS.BL.Controller.MFTController
{
    public partial class MFTController
    {
        public MFT MFT { get; private set; }

        public MFTController()
        {
            MFT = new MFT();
        }
        
    }
}
