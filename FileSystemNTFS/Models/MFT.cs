using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace FileSystemNTFS.BL.Models
{
    [DataContract]
    public class MFT
    {
        [DataMember]
        public List<MFTEntry> Entries { get; private set; }

        public MFT()
        {
            Entries = new List<MFTEntry>();
        }

        [JsonConstructor]
        public MFT(List<MFTEntry> entries)
        {
            Entries = entries;
        }

        //TODO: Добавление, удаление, изменение Entries
    }
}
