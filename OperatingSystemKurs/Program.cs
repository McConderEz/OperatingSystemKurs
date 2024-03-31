using FileSystemNTFS.BL.Controller.FileSystemController;
using FileSystemNTFS.BL.Models;
using System.Text;

Superblock superblock = new Superblock();


FileSystemController fileSystemController = new FileSystemController();
fileSystemController.CreateFile("newFile", "D:\\FileSystem");
fileSystemController.WriteFile("newFile", new StringBuilder("Я помню чудное мгновенье:"));
fileSystemController.WriteFile("newFile", new StringBuilder("Передо мной явилась ты,"));
fileSystemController.WriteFile("newFile", new StringBuilder("Как мимолетное виденье,"));
fileSystemController.WriteFile("newFile", new StringBuilder("Как гений чистой красоты."));