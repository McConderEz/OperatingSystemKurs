using Newtonsoft.Json;
using System.Runtime.Serialization;

namespace FileSystemNTFS.BL.Models
{
    /// <summary>
    /// Права доступа для U-user(владельца файла)
    /// Права доступа для g-group(группы владельца файла)
    /// Права доступа для o-other(других пользователей системы)
    /// </summary>
    [DataContract]
    public class UsersAccessFlags
    {
        [DataMember]
        public AttributeFlags U { get; set; }
        [DataMember]
        public AttributeFlags G { get; set; }
        [DataMember]
        public AttributeFlags O { get; set; }

        [JsonConstructor]
        public UsersAccessFlags(AttributeFlags u = AttributeFlags.FullControl,
            AttributeFlags g = AttributeFlags.Modify, AttributeFlags o = AttributeFlags.Modify)
        {
            U = u;
            G = g;
            O = o;
        }

        public override string ToString()
        {
            return $"{U}-{G}-{O}";
        }
    }
}