using FileSystemNTFS.BL.Controller.MFTController;
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


        public GroupController(UserController userController)
        {
            Groups = Load();
            _userController = userController;
        }

        public List<Group> Load()
        {
            return Load<List<Group>>() ?? new List<Group>();
        }

        public void SetCurrentUser(User user)
        {
            _userController.CurrentUser = user;
            _userController.Update();
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

        public void AddNewGroup(string name, MFTController.MFTController mftController)
        {
            var group = Groups.SingleOrDefault(g => g.Name.Equals(name));

            if(group == null)
            {
                Groups.Add(new Group(name, new List<User>() { _userController.CurrentUser }));
                _userController.CurrentUser.Groups.Add(group);
                _userController.Users.SingleOrDefault(x => x.Login.Equals(_userController.CurrentUser.Login)).Groups.Add(Groups.SingleOrDefault(x=>x.Name.Equals(name)));

                mftController.MFT.Entries.Where(x => x.Attributes.OwnerId.Equals(_userController.CurrentUser.Id));              
                _userController.Save();
                Save();
                mftController.MFT.Entries.ForEach(mftController.Update);
                mftController.Save();
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
                group.Users.Remove(group.Users.FirstOrDefault(u => u.Login.Equals(user.Login)));
                user.Groups.Remove(user.Groups.FirstOrDefault(g => g.Name.Equals(group.Name)));
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

        
        public void DeleteGroup(string name, MFTController.MFTController mftController)
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

                var mftEntries = mftController.MFT.Entries
                                                  .Where(e => e.Attributes?.Groups?.Any(x => x != null && x.Name.Contains(group.Name)) == true)
                                                  .ToList();
                mftEntries.ForEach(x => x.Attributes.Groups.RemoveAll(x => x.Name.Equals(name)));
                mftController.Save();

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
