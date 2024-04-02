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
    public class MFTEntry
    {
        [DataMember]
        public string Id { get; } = Guid.NewGuid().ToString();
        [DataMember]
        public MFTEntryHeader Header { get; private set; }
        [DataMember]
        public Attribute Attributes { get; private set; }

        [JsonConstructor]
        public MFTEntry(string id,MFTEntryHeader header, Attribute attributes)
        {
            #region
            if (string.IsNullOrWhiteSpace(id)) throw new ArgumentNullException("id can`t be null!");
            if (header == null) throw new ArgumentNullException("header can`t be null");
            if (attributes == null) throw new ArgumentNullException("attribute can`t be null!");           
            #endregion

            Header = header;
            Attributes = attributes;
            Id = id;
        }

        public MFTEntry(MFTEntryHeader header, Attribute attribute)
        {
            #region
            if (header == null) throw new ArgumentNullException("header can`t be null");
            if (attribute == null) throw new ArgumentNullException("attribute can`t be null!");
            #endregion
            Header = header;
            Attributes = attribute;
        }
    }
}
