using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace MultiuserProtection.BL.Models
{
    [DataContract]
    public class Group
    {
        [DataMember]
        public string Name { get; set; }
        [DataMember]
        public List<User> Users { get; set; }

        [JsonConstructor]
        public Group(string name, List<User> users)
        {
            Name = name;
            Users = users;
        }

        public Group()
        {

        }
    }
}
