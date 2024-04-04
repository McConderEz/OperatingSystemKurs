using FileSystemNTFS.BL.Models;
using MultiuserProtection.BL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace FileSystemNTFS.BL.Controller
{
    public class GroupController : ControllerBase
    {
        public List<Group> Groups { get; set; }
        private UserController _userController { get; set; }

        public GroupController()
        {
            Groups = Load();
            _userController = new UserController();
        }

        public List<Group> Load()
        {
            return Load<List<Group>>() ?? new List<Group>();
        }

        public List<Group> GetAllGroupOfUser(string login)
        {
            var user = _userController.Users.SingleOrDefault(u => u.Login.Equals(login));

            if(user != null)
            {
                return user.Groups;
            }
            else
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("Пользователь не найден");
                Console.ResetColor();
                return null;
            }
        }

        public List<User> GetAllUserOfGroup(string name)
        {
            var group = Groups.SingleOrDefault(g => g.Name.Equals(name));

            if(group != null)
            {
                return group.Users;
            }
            else
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("Группа не найдена");
                Console.ResetColor();
                return null;
            }
        }

        public void AddNewGroup(string name)
        {
            var group = Groups.SingleOrDefault(g => g.Name.Equals(name));

            if(group == null)
            {
                Groups.Add(new Group(name, new List<User>()));
                Save();
            }
            else
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("Группа с таким названием уже существует!");
                Console.ResetColor();
            }
        }

        public void AddNewUserInGroup(string login, string name)
        {
            var group = Groups.SingleOrDefault(g => g.Name.Equals(name));
            var user = _userController.Users.SingleOrDefault(u => u.Login.Equals(login));

            if (group != null && user != null)
            {
                group.Users.Add(user);
                user.Groups.Add(group);
                _userController.Save();
                Save();
            }
            else
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("Неверные данные группы или пользователя!");
                Console.ResetColor();
            }
        }

        public void DeleteUserFormGroup(string login, string name)
        {
            var group = Groups.SingleOrDefault(g => g.Name.Equals(name));
            var user = _userController.Users.SingleOrDefault(u => u.Login.Equals(login));

            if (group != null && user != null)
            {
                group.Users.Remove(user);
                user.Groups.Remove(group);
                _userController.Save();
                Save();
            }
            else
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("Неверные данные группы или пользователя!");
                Console.ResetColor();
            }
        }

        //TODO:Удалять группу из mftEntry
        public void DeleteGroup(string name)
        {
            var group = Groups.SingleOrDefault(g => g.Name.Equals(name));

            if (group != null)
            {
                foreach(var user in _userController.Users)
                {
                    var groupToRemove = user.Groups.SingleOrDefault(g => g.Name == name);
                    if (groupToRemove != null)
                    {
                        user.Groups.Remove(groupToRemove);
                    }
                } 

                Groups.Remove(group);
                _userController.Save();
                Save();
            }
            else
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("Неверные данные группы или пользователя!");
                Console.ResetColor();
            }
        }

        public void Save()
        {
            Save(Groups);
        }
    }
}
