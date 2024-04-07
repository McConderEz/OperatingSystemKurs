using FileSystemNTFS.BL;
using FileSystemNTFS.BL.Controller.FileSystemController;
using FileSystemNTFS.BL.Models;
using InterprocessCommunication.BL;
using MultiuserProtection.Domain;
using Spectre.Console;
using System;
using System.Diagnostics;
using System.Text;
using System.Text.RegularExpressions;



FileSystemController fileSystemController = new FileSystemController();
AnsiConsole.Write(new FigletText($"{fileSystemController.FileSystem.SuperblockController.Superblock.OperatingSystemName}").Color(Color.Yellow1));
Console.WriteLine();
bool exit = false;


AnsiConsole.Status().Start("Загрузка системы", action => Thread.Sleep(1000));
Console.Clear();
l1:

var choice = AnsiConsole.Prompt(new SelectionPrompt<string>()
    .Title("[green]Выберите опцию для входа[/]")
    .PageSize(10)
    .AddChoices(new[] { "Войти как пользователь", "Войти как гость", "Создать нового пользователя" } ));


if (choice.Equals("Войти как пользователь"))
{
    Console.WriteLine("Авторизуйтесь в систему");
    Console.WriteLine("Введите логин:");
    string login = Console.ReadLine();
    Console.WriteLine("Введите пароль:");
    string password = Console.ReadLine();
    Console.Clear();
    while (!fileSystemController.FileSystem.Authorize(login, password))
    {
        Console.WriteLine("Авторизуйтесь в систему(чтобы выйти из авторизации введите в оба поля exit)");
        Console.WriteLine("Введите логин:");
        login = Console.ReadLine();
        Console.WriteLine("Введите пароль:");
        password = Console.ReadLine();
        Console.Clear();

        if (login.Equals("exit") && password.Equals("exit"))
        {
            goto l1;
        }
    }
}
else if (choice.Equals("Войти как гость"))
{
    fileSystemController.FileSystem.Authorize("guest","guest");
}
else if (choice.Equals("Создать нового пользователя"))
{
    Console.WriteLine("Введите логин:");
    string login = Console.ReadLine();
    Console.WriteLine("Введите пароль:");
    string password = Console.ReadLine();
    fileSystemController.FileSystem.UserController.AddNewUser(login, password);
    goto l1;
}
else
{
    goto l1;
}

Thread.Sleep(2000);
Console.Clear();
fileSystemController.FileSystem.GroupController.SetCurrentUser(fileSystemController.FileSystem.UserController.CurrentUser);
while (true)
{
    try
    {
        var choiceOperationOfSystem = AnsiConsole.Prompt(new SelectionPrompt<string>()
        .Title("[yellow]Выберите действие[/]")
        .PageSize(10)
        .AddChoiceGroup("Операции над файлами", new[] { "Создание файла", "Запись в файл", "Чтение файла",
                        "Переименование файла", "Копирование файла", "Удаление файла",
                        "Перемещение файла" })
        .AddChoiceGroup("Операции над директориями", new[] {"Создать директорию", "Удалить директорию",
                        "Переместить директорию", "Переименовать директорию", "Копировать директорию"})
        .AddChoiceGroup("Операции над пользователями и правами", new[] {"Вывести список пользователей", "Смена прав доступа файла", "Добавление пользователей в группу",
                        "Удаление пользователя из группы", "Создание группы", "Удаление группы",
                        "Смена пользователя", "Текущий пользователь в системе", "Добавить нового пользователя",
                        "Посмотреть всех пользователей группы", "Посмотреть все группы, в которых состоит пользователь","Удалить пользователя"})
        .AddChoiceGroup("Демонстрация работы", new[] { "Демонстрация работы планировщика(относительные и статические приоритеты)", "Демонстрация работы планировщика(свопинг процессов)", "Демонстрация работы межпроцессного взаимодействия" })
        .AddChoiceGroup("Навигация и другое", new[] {
                        "Показать текущую директорию",
                        "Показать дерево директорий и файлов", "Список файлов директории", "Вернуться в корневую директорию",
                        "Перейти в другую директорию", "Форматирование системы", "Выход из системы"}));




        Console.WriteLine();

        switch (choiceOperationOfSystem)
        {
            case "Создание файла":
                Console.WriteLine("Введите имя файла:");
                string nameToCreateFile = Console.ReadLine();
                Console.WriteLine("Введите полный путь директории, где хотите создать файл");
                string fullPathDirToCreateFile = Console.ReadLine();
                fileSystemController.CreateFile(nameToCreateFile, fullPathDirToCreateFile);
                break;
            case "Запись в файл":
                Console.WriteLine("Введите имя файла:");
                string nameToWrite = Console.ReadLine();
                Console.WriteLine("Введите данные:");
                string data = Console.ReadLine();
                fileSystemController.WriteFile(nameToWrite, new StringBuilder(data));
                break;
            case "Чтение файла":
                Console.WriteLine("Введите имя файла:");
                string nameToRead = Console.ReadLine();
                Console.WriteLine("Данные с файла:");
                Console.WriteLine(fileSystemController.ReadFile(nameToRead));
                break;
            case "Переименование файла":
                Console.WriteLine("Введите имя файла:");
                string oldFileNameToRename = Console.ReadLine();
                Console.WriteLine("Введите новое имя файла:");
                string newFileNameToRename = Console.ReadLine();
                fileSystemController.RenameFile(oldFileNameToRename, newFileNameToRename);
                break;
            case "Копирование файла":
                Console.WriteLine("Введите имя файла:");
                string fileNameToCopy = Console.ReadLine();
                Console.WriteLine("Введите полный путь файла в копируемую директорию:");
                string fullPathToCopyFile = Console.ReadLine();
                fileSystemController.CopyTo(fileNameToCopy, fullPathToCopyFile);
                break;
            case "Удаление файла":
                Console.WriteLine("Введите имя файла, включая расширение:");
                string fileNameToDelete = Console.ReadLine();
                fileSystemController.DeleteFile(fileNameToDelete);
                break;
            case "Перемещение файла":
                Console.WriteLine("Введите имя файла:");
                string fileNameToMove = Console.ReadLine();
                Console.WriteLine("Введите полный путь в перемещаемую директорию:");
                string dirPath = Console.ReadLine();
                fileSystemController.MoveTo(fileNameToMove, dirPath);
                break;
            case "Создать директорию":
                Console.WriteLine("Введите имя директории:");
                string dirNameToCreate = Console.ReadLine();
                fileSystemController.CreateDirectory(dirNameToCreate);
                break;
            case "Удалить директорию":
                Console.WriteLine("Введите имя директории:");
                string dirNameToDelete = Console.ReadLine();
                fileSystemController.DeleteDirectory(dirNameToDelete);
                break;
            case "Переместить директорию":
                Console.WriteLine("Введите имя директории:");
                string dirNameToMove = Console.ReadLine();
                Console.WriteLine("Введите новый полный путь директории:");
                string newDirPath = Console.ReadLine();
                fileSystemController.MoveDirectory(dirNameToMove, newDirPath);
                break;
            case "Переименовать директорию":
                Console.WriteLine("Введите имя директории:");
                string oldDirName = Console.ReadLine();
                Console.WriteLine("Введите новое имя директории:");
                string dirNameToRename = Console.ReadLine();
                fileSystemController.RenameDirectory(oldDirName, dirNameToRename);
                break;
            case "Копировать директорию":
                Console.WriteLine("Введите имя директории:");
                string dirNameToCopy = Console.ReadLine();
                Console.WriteLine("Введите путь для копируемой директории:");
                string newFullDirPathToCopy = Console.ReadLine();
                fileSystemController.CopyDirTo(dirNameToCopy, newFullDirPathToCopy);
                break;
            case "Вывести список пользователей":
                var userTable = new Table();
                userTable.AddColumn("Имя пользователя").Centered();
                userTable.AddColumn("Роль").Centered();
                foreach (var user in fileSystemController.FileSystem.UserController.Users)
                {
                    userTable.AddRow(user.Login, user.AccountType.ToString());
                }
                AnsiConsole.Render(userTable);
                break;
            case "Смена прав доступа файла":
                Console.WriteLine("Введите полный путь к файлу, включая название и расширение:");
                string fullPathToFileCHMOD = Console.ReadLine();
                Console.WriteLine("Введите права доступа(3 значения через пробел):");
                string input = Console.ReadLine();
                string[] values = input.Split(' ');
                if (values.Length != 3)
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("Некорректный ввод. Пожалуйста, введите 3 числовых значения через пробел.");
                    Console.ResetColor();
                }
                else
                {
                    if (int.TryParse(values[0], out int value1) && int.TryParse(values[1], out int value2) && int.TryParse(values[2], out int value3))
                    {
                        fileSystemController.ChangeMode(fullPathToFileCHMOD, new UsersAccessFlags((AttributeFlags)value1, (AttributeFlags)value2, (AttributeFlags)value3));
                    }
                    else
                    {
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.WriteLine("Некорректный ввод. Пожалуйста, убедитесь, что введены корректные числовые значения.");
                        Console.ResetColor();
                    }
                }
                break;
            case "Добавление пользователей в группу":
                Console.WriteLine("Введите название группы: ");
                string groupNameForAddUser = Console.ReadLine();
                Console.WriteLine("Введите имя пользователя для добавления");
                string userLoginForAddInGroup = Console.ReadLine();
                fileSystemController.FileSystem.GroupController.AddNewUserInGroup(userLoginForAddInGroup, groupNameForAddUser);
                break;
            case "Удаление пользователя из группы":
                Console.WriteLine("Введите название группы: ");
                string groupNameForDeleteUser = Console.ReadLine();
                Console.WriteLine("Введите имя пользователя для удаления");
                string userLoginForDeleteFromGroup = Console.ReadLine();
                fileSystemController.FileSystem.GroupController.DeleteUserFormGroup(userLoginForDeleteFromGroup, groupNameForDeleteUser);
                break;
            case "Создание группы":
                Console.WriteLine("Введите название группы: ");
                string groupNameToCreate = Console.ReadLine();
                fileSystemController.FileSystem.GroupController.AddNewGroup(groupNameToCreate, fileSystemController.FileSystem.MFTController);
                break;
            case "Удаление группы":
                Console.WriteLine("Введите название группы: ");
                string groupNameToDelete = Console.ReadLine();
                fileSystemController.FileSystem.GroupController.DeleteGroup(groupNameToDelete, fileSystemController.FileSystem.MFTController);
                break;
            case "Смена пользователя":
                Console.Clear();
                Console.WriteLine("Авторизуйтесь в систему");
                Console.WriteLine("Введите логин:");
                string changeAccLogin = Console.ReadLine();
                Console.WriteLine("Введите пароль:");
                string changeAccPassword = Console.ReadLine();
                while (!fileSystemController.FileSystem.Authorize(changeAccLogin, changeAccPassword))
                {
                    Console.Clear();
                    Console.WriteLine("Авторизуйтесь в систему");
                    Console.WriteLine("Введите логин:");
                    changeAccLogin = Console.ReadLine();
                    Console.WriteLine("Введите пароль:");
                    changeAccPassword = Console.ReadLine();
                }
                break;
            case "Текущий пользователь в системе":
                Console.WriteLine($"{fileSystemController.FileSystem.UserController.CurrentUser.Login} - {fileSystemController.FileSystem.UserController.CurrentUser.AccountType}");
                break;
            case "Добавить нового пользователя":
                Console.WriteLine("Введите имя пользователя:");
                string newUserLogin = Console.ReadLine();
                Console.WriteLine("Введите пароль:");
                string newUserPassword = Console.ReadLine();
                fileSystemController.FileSystem.UserController.AddNewUser(newUserLogin, newUserPassword);
                break;
            case "Удалить пользователя":
                Console.WriteLine("Введите имя пользователя:");
                string userDeleteLogin = Console.ReadLine();
                fileSystemController.FileSystem.UserController.DeleteUser(userDeleteLogin);
                break;
            case "Посмотреть всех пользователей группы":
                Console.WriteLine("Введите название группы:");
                string groupNameToShowUsers = Console.ReadLine();
                var users = fileSystemController.FileSystem.GroupController.GetAllUserOfGroup(groupNameToShowUsers);
                var usersFromGroupTable = new Table();
                usersFromGroupTable.AddColumns("Имя группы", "Имя пользователя", "Роль").Centered();
                foreach (var user in users)
                {
                    usersFromGroupTable.AddRow(groupNameToShowUsers, user.Login, user.AccountType.ToString());
                }
                AnsiConsole.Render(usersFromGroupTable);
                break;
            case "Посмотреть все группы, в которых состоит пользователь":
                Console.WriteLine("Введите имя пользователя:");
                string loginToShowAllGroup = Console.ReadLine();
                var allGroupOfUserTable = new Table();
                allGroupOfUserTable.AddColumns("Имя пользователя", "Роль", "Группы").Centered();
                var groups = fileSystemController.FileSystem.GroupController.GetAllGroupOfUser(loginToShowAllGroup);
                foreach (var group in groups)
                {
                    if (group != null)
                        allGroupOfUserTable.AddRow(loginToShowAllGroup, fileSystemController.FileSystem.UserController.CurrentUser.AccountType.ToString(), group.Name);
                }
                AnsiConsole.Render(allGroupOfUserTable);
                break;
            case "Показать текущую директорию":
                Console.ForegroundColor = Color.Yellow;
                Console.WriteLine($"Текущая директория: {fileSystemController.CurrentPath}");
                Console.ResetColor();
                break;
            case "Показать дерево директорий и файлов":
                RenderDirectory(new DirectoryInfo(@"D:\FileSystem"));
                break;
            case "Список файлов директории":
                Console.WriteLine("Введите полный путь директории:");
                string fullDirPath = Console.ReadLine();
                var filesFromDirTable = new Table();
                filesFromDirTable.AddColumns("Имя файла", "Размер", "Права доступа").Centered();

                foreach (var objectFS in fileSystemController.GetAllFilesFromDirectory(fullDirPath))
                {
                    filesFromDirTable.AddRow(objectFS.FileName, objectFS.Length.ToString(), objectFS.AccessFlags.ToString());
                }
                AnsiConsole.Render(filesFromDirTable);
                break;
            case "Вернуться в корневую директорию":
                fileSystemController.CurrentPath = @"D:\FileSystem";
                break;
            case "Перейти в другую директорию":
                Console.WriteLine("Введите полный путь директории: ");
                string directoryPath = Console.ReadLine();
                if (Directory.Exists(directoryPath))
                {
                    fileSystemController.CurrentPath = directoryPath;
                }
                else
                {
                    Console.ForegroundColor = Color.Red;
                    Console.WriteLine("Директория не найдена!");
                    Console.ResetColor();
                }
                break;
            case "Форматирование системы":
                if (fileSystemController.FileSystem.UserController.CurrentUser.AccountType == AccountType.Administrator)
                    fileSystemController.Formatting();
                else
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("Недостаточно прав для форматирование ФС");
                    Console.ResetColor();
                }
                break;
            case "Выход из системы":
                exit = true;
                break;
            case "Демонстрация работы межпроцессного взаимодействия":
                InterProcessCommunication interProcessCommunication = new InterProcessCommunication();
                interProcessCommunication.Start();
                break;
            case "Демонстрация работы планировщика(относительные и статические приоритеты)":
                var scheduler = new Scheduler(quantum: 1, maxMemory: 10);

                scheduler.AddProcess(new MyProcess { PID = 1, Name = "Process 1", ExecutionTime = 5, RemainingTime = 5, MemorySize = 3, Priority = ProcessPriority.High });
                scheduler.AddProcess(new MyProcess { PID = 2, Name = "Process 2", ExecutionTime = 10, RemainingTime = 10, MemorySize = 4, Priority = ProcessPriority.Medium });
                scheduler.AddProcess(new MyProcess { PID = 3, Name = "Process 3", ExecutionTime = 7, RemainingTime = 7, MemorySize = 2, Priority = ProcessPriority.Low });
                scheduler.Run();
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine("Все процессы были выполнены.");
                Console.ResetColor();
                break;
            case "Демонстрация работы планировщика(свопинг процессов)":
                int timeQuantum = 3;
                int memorySize = 4;
                Channel channel = new Channel();
                SharedMemory sharedMemory = new SharedMemory();
                ProcessScheduler schedulerSP = new ProcessScheduler(timeQuantum, memorySize, channel, sharedMemory);
                schedulerSP.GenerateProcesses(10);
                schedulerSP.Run();
                schedulerSP.PrintCompletedProcesses();
                break;
            default:
                Console.WriteLine("Неизвестная команда!");
                break;
        }

        if (exit == true)
        {
            AnsiConsole.Status().Start("Завершение работы", action => Thread.Sleep(3000));
            break;
        }

        Console.WriteLine("Нажимите клавишу, чтобы продолжить...");
        Console.ReadKey();
        Console.Clear();
    }
    catch (Exception ex) 
    {
        Console.ForegroundColor = ConsoleColor.Red;
        Console.WriteLine(ex.ToString());
        Console.WriteLine("Произошло непредвиденная ошибка!Если файловая система повреждена - форматируйте!");
        Console.ResetColor();
        Console.ReadKey();
    }
}





static int ConvertInt()
{
    int result;

    while (!int.TryParse(Console.ReadLine(), out result))
    {
        Console.WriteLine("Неверно задано значение переменной!");
    }

    return result;
}

static void RenderDirectory(DirectoryInfo directory)
{
    var tree = new Tree($"[red]{directory.Name}[/]");

    foreach (var files in directory.GetFiles())
    {
        tree.AddNode($"[green]{files.Name}[/]");
    }

    foreach(var subDirectory in directory.GetDirectories())
    {
        var subtree = tree.AddNode($"[yellow]{subDirectory.Name}[/]");
        RenderSubDirectory(subDirectory, subtree);
    }
    AnsiConsole.Render(tree);
}

static void RenderSubDirectory(DirectoryInfo? directory, TreeNode? parentNode)
{
    foreach (var files in directory.GetFiles())
    {
        parentNode.AddNode($"[green]{files.Name}[/]");
    }

    foreach (var subDirectory in directory.GetDirectories())
    {
        var subtree = parentNode.AddNode($"[yellow]{subDirectory.Name}[/]");
        RenderSubDirectory(subDirectory, subtree);
    }
}