using FileSystemNTFS.BL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Attribute = FileSystemNTFS.BL.Models.Attribute;

namespace FileSystemNTFS.BL.Controller.MFTController
{
    public partial class MFTController
    {
        public void Create(FileSystemInfo fileInfo, FileSystemObject fileSystemObject)
        {
            uint logSequence = MFT.Entries.Count == 0 ? 1 : MFT.Entries.Select(x => x.Header.LogSequenceNumber)
                                          .Max() + 1;

            MFTEntryHeader header = new MFTEntryHeader((uint)(MFT.Entries.Count + 1), logSequence, fileSystemObject);
            //TODO:Времено ownderid и groupid  = 1
            Attribute attribute = new Attribute(fileInfo.Name, fileInfo.FullName, 0, 1,
                                                new List<uint>(), 0, new TimeMarks(),
                                                new List<Indexer>(), new UsersAccessFlags(), new List<Attribute>());

            MFT.Entries.Add(new MFTEntry(header, attribute));
        }
    }
}
