using FileSystemNTFS.BL.Controller.FileSystemController;
using FileSystemNTFS.BL.Models;
using System.Text;

Superblock superblock = new Superblock();


FileSystemController fileSystemController = new FileSystemController();

Console.WriteLine(fileSystemController.ReadFile("TestFile").ToString());