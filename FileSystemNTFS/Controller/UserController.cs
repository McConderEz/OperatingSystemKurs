using MultiuserProtection.BL.Controllers;
using MultiuserProtection.BL.Models;
using MultiuserProtection.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;


namespace FileSystemNTFS.BL.Controller
{
    public class UserController : ControllerBase
    {
        public List<User> Users { get; set; }

        public User CurrentUser { get; set; }

        public bool IsNewUser { get; } = false;
        private CryptographyController _cryptographyController { get; set; }

        public UserController(string login, string password)
        {
            _cryptographyController = new CryptographyController();

            if (string.IsNullOrWhiteSpace(login))
            {
                throw new ArgumentNullException(nameof(login), "Имя пользователя не может быть пустым");
            }

            if (string.IsNullOrWhiteSpace(password))
            {
                throw new ArgumentNullException(nameof(password), "Пароль не может быть пустым");
            }

            Users = GetUsersData();

            var root = Users.SingleOrDefault(u => u.Login == "root" && _cryptographyController.ValidatePassword("root", u.HashPassword));

            if (root == null)
            {
                Users.Add(new User(0, new List<Group>(), "root", _cryptographyController.GenerateHash("root"), DateTime.Now, AccountType.Administrator));
                Save();
                Users = GetUsersData();
            }

            CurrentUser = Authorize(login, password, root);

            if (CurrentUser == null)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("Пользователь с такими данными не найден");
                Console.ResetColor();
            }
            else
            {
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine($"Добро пожаловать, {CurrentUser.Login}!");
                Console.ResetColor();
            }
        }

        public UserController()
        {
            Users = GetUsersData();
            _cryptographyController = new CryptographyController();
        }

        private User? Authorize(string login, string password, User? root)
        {
            User user;
            if (login.Equals("root") && password.Equals("root"))
            {
                root = Users.SingleOrDefault(u => u.Login == "root" && _cryptographyController.ValidatePassword("root", u.HashPassword));
                user = root;

            }
            else if (login.Equals("guest") && password.Equals("guest"))
            {
                user = new User(0, new List<Group>(), login, _cryptographyController.GenerateHash(password), DateTime.Now, AccountType.Guest);
            }
            else
            {
                user = Users.SingleOrDefault(u => u.Login == login && _cryptographyController.ValidatePassword(password, u.HashPassword));
            }

            return user;
        }

        public void AddNewUser(string login, string password)
        {
            try
            {
                if (login.Equals("root"))
                {
                    Console.WriteLine("Данное имя зарезервировано");
                    return;
                }

                var flag = Users.Exists(u => u.Login.Equals(login));

                if (string.IsNullOrWhiteSpace(login))
                {
                    throw new ArgumentNullException($"{login} is not a valid login", nameof(login));
                }

                if (string.IsNullOrWhiteSpace(password))
                {
                    throw new ArgumentNullException($"{password} is not a valid login", nameof(password));
                }

                if (flag)
                {
                    Console.WriteLine("Аккаунт с таким именем уже существует");
                }
                else
                {
                    uint id;
                    if (Users.Count == 0)
                    {
                        id = 1;
                    }
                    else
                    {
                        id = (uint)(Users.Count + 1);
                    }
                    var hashPassword = _cryptographyController.GenerateHash(password);
                    Users.Add(new User(id, new List<Group>(), login, hashPassword, DateTime.Now));
                    Console.ForegroundColor = ConsoleColor.Green;
                    Console.WriteLine("Новый пользователь добавлен");
                    Console.ResetColor();
                    Save();
                }
            }
            catch (Exception ex)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine(ex.Message);
                Console.ResetColor();
            }
        }

        public void DeleteUser(string login)
        {
            var user = Users.SingleOrDefault(u => u.Login.Equals(login));

            if (user != null && !login.Equals("root"))
            {
                Users.Remove(user);
                Save();
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine("Пользователь удалён из системы");
                Console.ResetColor();
            }
            else
            {
                Console.ForegroundColor= ConsoleColor.Red;
                Console.WriteLine("Пользователь не найден");
                Console.ResetColor();
            }
        }

        private List<User> GetUsersData()
        {
            return Load<List<User>>() ?? new List<User>();
        }

        public void Save()
        {
            Save(Users);
        }

        public void Update()
        {
            Users = GetUsersData();
        }
    }
}
