using FileSystemNTFS.BL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileSystemNTFS.BL.Controller.MFTController
{
    public partial class MFTController
    {
        public void Update(MFTEntry entry)
        {
            uint logSequence = MFT.Entries.Count == 0 ? 1 : MFT.Entries.Select(x => x.Header.LogSequenceNumber)
                                          .Max() + 1;

            var mftItem = MFT.Entries.SingleOrDefault(x => x.Attributes.FullPath.Equals(entry.Attributes.FullPath, StringComparison.OrdinalIgnoreCase));

            if(mftItem != null)
            {
                mftItem.Attributes.Groups = User.CurrentUser.Groups;
                mftItem.Header.LogSequenceNumber = logSequence;
                mftItem.Attributes.TimeMarks.AccessTime = DateTime.Now;
                mftItem.Attributes.BlocksCount = (uint)mftItem.Attributes.IndexesOnClusterBitmap.Count;
            }
        }
    }
}
