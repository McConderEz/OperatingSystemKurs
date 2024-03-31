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
        public void Create(FileInfo fileInfo)
        {
            uint logSequence = MFT.Entries.Count == 0 ? 1 : MFT.Entries.Select(x => x.Header.LogSequenceNumber)
                                          .Max() + 1;

            MFTEntryHeader header = new MFTEntryHeader((uint)(MFT.Entries.Count + 1), logSequence, FileSystemObject.File);
            //TODO:Времено ownderid и groupid  = 1
            Attribute attribute = new Attribute(fileInfo.Name, fileInfo.FullName, 0, 1,
                                                new List<uint>(), 0, new TimeMarks(),
                                                new List<Indexer>(), new UsersAccessFlags());

            MFT.Entries.Add(new MFTEntry(header, attribute));
        }
    }
}
