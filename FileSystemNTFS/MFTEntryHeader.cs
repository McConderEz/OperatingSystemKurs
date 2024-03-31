using Microsoft.VisualBasic.FileIO;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace FileSystemNTFS.BL
{
    [DataContract]
    public class MFTEntryHeader
    { 
        [DataMember]
        public uint LogSequenceNumber { get; set; }
        [DataMember]
        public uint SequenceNumber { get; set; }
        [DataMember]
        public FileSystemObject FileSystemObject { get; set; }

        [JsonConstructor]
        public MFTEntryHeader(uint sequenceNumber, uint logSequenceNumber, FileSystemObject fileSystemObject = FileSystemObject.File)
        {
            
            if (sequenceNumber < 0)
            {
                throw new ArgumentException("Номер последовательности не может быть меньше нуля!", nameof(sequenceNumber));
            }

            SequenceNumber = sequenceNumber;
            LogSequenceNumber = logSequenceNumber;
            FileSystemObject = fileSystemObject;
        }
    }
}
