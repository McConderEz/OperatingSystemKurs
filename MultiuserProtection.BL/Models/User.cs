using MultiuserProtection.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace MultiuserProtection.BL.Models
{
    [DataContract]
    public class User
    {
        [DataMember]
        public uint Id { get; init; }
        [DataMember]
        public List<Group> Groups { get; set; }
        [DataMember]
        public string Login { get; set; }
        [DataMember]
        public string HashPassword { get; set; }
        [DataMember]
        public AccountType AccountType { get; set; }
        [DataMember]
        public DateTime LastLoginDate { get; set; }

        [JsonConstructor]
        public User(uint id, List<Group> groups, string login, string hashPassword, DateTime lastLoginDate, AccountType accountType = AccountType.Normal)
        {
            if (id < 0)
            {
                throw new ArgumentException("Id не может быть меньше 0!", nameof(id));
            }

            if (string.IsNullOrWhiteSpace(hashPassword))
            {
                throw new ArgumentNullException("HashPassword не может быть пустым!", nameof(hashPassword));
            }

            if (string.IsNullOrWhiteSpace(login))
            {
                throw new ArgumentNullException("Login не может быть пустым!", nameof(login));
            }

            Id = id;
            Groups = groups;
            Login = login;
            HashPassword = hashPassword;
            AccountType = accountType;
            LastLoginDate = lastLoginDate;
        }


        public User(string login, string hashPassword)
        {
            Login = login;
            HashPassword = hashPassword;
        }

        public override string ToString()
        {
            return $"{Id}\t{Login}\t{AccountType}\t{LastLoginDate}";
        }
    }
}
