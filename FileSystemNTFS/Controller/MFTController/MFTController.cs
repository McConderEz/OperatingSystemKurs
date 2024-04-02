using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FileSystemNTFS.BL.Models;

namespace FileSystemNTFS.BL.Controller.MFTController
{
    public partial class MFTController : ControllerBase
    {
        public MFT MFT { get; private set; }
        public UserController User { get; private set; }

        public MFTController(UserController user)
        {
            MFT = Load();
            User = user;
        }

        public MFT Load()
        {
            return Load<MFT>() ?? new MFT();
        }

        public void Save()
        {
            Save(MFT);
        }
        
    }
}
