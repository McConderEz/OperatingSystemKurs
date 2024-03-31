using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileSystemNTFS.BL.Models
{
    public class MFTEntry
    {
        public string Id { get; } = Guid.NewGuid().ToString();
        public MFTEntryHeader Header { get; private set; }
        public Attribute Attributes { get; private set; }

        [JsonConstructor]
        public MFTEntry(MFTEntryHeader header, Attribute attribute, string id)
        {
            #region
            if (header == null) throw new ArgumentNullException("header can`t be null");
            if (attribute == null) throw new ArgumentNullException("attribute can`t be null!");
            if (string.IsNullOrWhiteSpace(id)) throw new ArgumentNullException("id can`t be null!");
            #endregion

            Header = header;
            Attributes = attribute;
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

        //TODO: Переопределить ToString
    }
}
