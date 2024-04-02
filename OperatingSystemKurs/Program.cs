using FileSystemNTFS.BL.Controller.FileSystemController;
using FileSystemNTFS.BL.Models;
using Spectre.Console;
using System.Text;

Superblock superblock = new Superblock();


FileSystemController fileSystemController = new FileSystemController("root","root");

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

//fileSystemController.CopyDirTo("Dir2", @"D:\FileSystem\Dir1");

//foreach (var item in fileSystemController.GetAllFilesFromDirectory(@"D:\FileSystem"))
//{
//    Console.WriteLine(item);
//}

var table = new Table();
table.AddColumn(new TableColumn("Path").Centered());
table.AddColumn(new TableColumn("Sizes").Centered());
foreach (var item in fileSystemController.GetAllFilesFromDirectory(@"D:\FileSystem"))
{
    //Console.WriteLine(item);
    table.AddRow(item, "test size");
}

AnsiConsole.Write(table);