using FileSystemNTFS.BL;
using FileSystemNTFS.BL.Controller.FileSystemController;
using FileSystemNTFS.BL.Models;
using InterprocessCommunication.BL;
using MultiuserProtection.Domain;
using Spectre.Console;
using System;
using System.Diagnostics;
using System.Text;
using TaskPlanner.BL;



#region
//InterProcessCommunication interProcessCommunication = new InterProcessCommunication();
//interProcessCommunication.Start();

//Scheduler scheduler = new Scheduler(3, 200);

//MyProcess process1 = new MyProcess(1, "Process 1", 5, 50, ProcessPriority.Medium);
//MyProcess process2 = new MyProcess(2, "Process 2", 3, 150, ProcessPriority.Low);
//MyProcess process3 = new MyProcess(3, "Process 3", 8, 50, ProcessPriority.Low);
//MyProcess process4 = new MyProcess(4, "Process 4", 15, 50, ProcessPriority.Low);
//MyProcess process5 = new MyProcess(5, "Process 5", 12,  150, ProcessPriority.High);
//MyProcess process6 = new MyProcess(6, "Process 6", 13, 50, ProcessPriority.Low);
//MyProcess process7 = new MyProcess(7, "Process 7", 14, 50, ProcessPriority.Medium);
//MyProcess process8 = new MyProcess(8, "Process 8", 17, 50, ProcessPriority.Medium);
//MyProcess process9 = new MyProcess(9, "Process 9", 6, 50, ProcessPriority.High);

//scheduler.AddProcess(process1);
//scheduler.AddProcess(process2);
//scheduler.AddProcess(process3);
//scheduler.AddProcess(process4);
//scheduler.AddProcess(process5);
//scheduler.AddProcess(process6);
//scheduler.AddProcess(process7);
//scheduler.AddProcess(process8);
//scheduler.AddProcess(process9);

//scheduler.Run();


//Superblock superblock = new Superblock();

//FileSystemController fileSystemController = new FileSystemController("root","root");

//fileSystemController.CreateFile("text1", @"D:\FileSystem");
//fileSystemController.CreateFile("text2", @"D:\FileSystem");
//fileSystemController.CreateFile("text3", @"D:\FileSystem");
//fileSystemController.CreateDirectory("Dir1");
//fileSystemController.MoveTo("text1", @"D:\FileSystem\Dir1");


//fileSystemController.CreateDirectory("Dir2");
//fileSystemController.CreateFile("text2", @"D:\FileSystem\Dir2");
//fileSystemController.CreateFile("text3", @"D:\FileSystem\Dir2");

//fileSystemController.CurrentPath = @"D:\FileSystem\Dir2";
//fileSystemController.CreateDirectory("Dir3");
//fileSystemController.CreateFile("text2", @"D:\FileSystem\Dir2\Dir3");
//fileSystemController.CreateFile("text3", @"D:\FileSystem\Dir2\Dir3");
//fileSystemController.CurrentPath = @"D:\FileSystem";

//fileSystemController.CurrentPath = @"D:\FileSystem\Dir2\Dir3";
//fileSystemController.CreateDirectory("Dir4");
//fileSystemController.CreateFile("text5", @"D:\FileSystem\Dir2\Dir3\Dir4");
//fileSystemController.CreateFile("text6", @"D:\FileSystem\Dir2\Dir3\Dir4");
//fileSystemController.CurrentPath = @"D:\FileSystem";

//fileSystemController.CopyDirTo("Dir2", @"D:\FileSystem\Dir1");

//foreach (var item in fileSystemController.GetAllFilesFromDirectory(@"D:\FileSystem"))
//{
//    Console.WriteLine(item);
//}

//var table = new Table();
//table.AddColumn(new TableColumn("Path").Centered());
//table.AddColumn(new TableColumn("Sizes").Centered());
//foreach (var item in fileSystemController.GetAllFilesFromDirectory(@"D:\FileSystem"))
//{
//    //Console.WriteLine(item);
//    table.AddRow(item, "test size");
//}

//AnsiConsole.Write(table);

#endregion


//RenderDirectory(new DirectoryInfo(@"D:\FileSystem"));
//Console.ReadLine();

FileSystemController fileSystemController = new FileSystemController();
AnsiConsole.Write(new FigletText($"{fileSystemController.FileSystem.SuperblockController.Superblock.OperatingSystemName}").Color(Color.Yellow1));
Console.WriteLine();
string tempPath = "";
string tempPath2 = "";
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

while (true)
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
            fileSystemController.CopyTo(fileNameToCopy,fullPathToCopyFile);
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
            fileSystemController.CopyTo(dirNameToCopy, newFullDirPathToCopy);
            break;
        case "Вывести список пользователей":
            var userTable = new Table();
            userTable.AddColumn("Имя пользователя").Centered();
            userTable.AddColumn("Роль").Centered();
            foreach(var user in fileSystemController.FileSystem.UserController.Users)
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
                    fileSystemController.ChangeMode(tempPath, new UsersAccessFlags((AttributeFlags)value1, (AttributeFlags)value2, (AttributeFlags)value3));
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
            //TODO:
            break;
        case "Удаление пользователя из группы":
            //TODO:
            break;
        case "Создание группы":
            Console.WriteLine("Введите название группы: ");
            string groupNameToCreate = Console.ReadLine();
            fileSystemController.FileSystem.GroupController.AddNewGroup(groupNameToCreate);
            break;
        case "Удаление группы":
            //TODO:
            break;
        case "Смена пользователя":
            Console.Clear();
            Console.WriteLine("Авторизуйтесь в систему");
            Console.WriteLine("Введите логин:");
            string changeAccLogin = Console.ReadLine();
            Console.WriteLine("Введите пароль:");
            string changeAccPassword = Console.ReadLine();
            while(!fileSystemController.FileSystem.Authorize(changeAccLogin, changeAccPassword))
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
            //TODO:
            break;
        case "Посмотреть все группы, в которых состоит пользователь":
            Console.WriteLine("Введите имя пользователя:");
            string loginToShowAllGroup = Console.ReadLine();
            var allGroupOfUserTable = new Table();
            allGroupOfUserTable.AddColumns("Имя пользователя", "Роль", "Группы").Centered();
            var groups = fileSystemController.FileSystem.GroupController.GetAllGroupOfUser(loginToShowAllGroup);
            foreach (var group in groups)
            {
                allGroupOfUserTable.AddRow(fileSystemController.FileSystem.UserController.CurrentUser.Login, fileSystemController.FileSystem.UserController.CurrentUser.AccountType.ToString(), group.Name);
            }
            AnsiConsole.Render(allGroupOfUserTable);
            break;
        case "Показать текущую директорию":
            Console.ForegroundColor= Color.Yellow;
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

            foreach(var objectFS in fileSystemController.GetAllFilesFromDirectory(fullDirPath))
            {
                filesFromDirTable.AddRow(objectFS.FileName, objectFS.Length.ToString(), objectFS.AccessFlags.ToString());
            }
            AnsiConsole.Render(filesFromDirTable);
            break;
        case "Вернуться в корневую директорию":
            break;
        case "Перейти в другую директорию":
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