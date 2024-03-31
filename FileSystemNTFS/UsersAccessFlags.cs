using Newtonsoft.Json;
using System.Runtime.Serialization;

namespace FileSystemNTFS.BL
{
    /// <summary>
    /// Права доступа для U-user(владельца файла)
    /// Права доступа для g-group(группы владельца файла)
    /// Права доступа для o-other(других пользователей системы)
    /// </summary>
    [DataContract]
    public class UsersAccessFlags
    {
        public AttributeFlags U { get; set; }
        public AttributeFlags G { get; set; }
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