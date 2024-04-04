using FileSystemNTFS.BL.Controller;
using FileSystemNTFS.BL.Controller.MFTController;
using FileSystemNTFS.BL.Controller.SuperblockController;
using FileSystemNTFS.BL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace FileSystemNTFS.BL.FileSystemOperation
{
    public class FileSystem
    {
        public string FileSystemPath { get; set; } = "D:\\FileSystem";
        public SuperblockController SuperblockController { get; set; }
        public MFTController MFTController { get; set; }
        public UserController UserController { get; set; }
        public GroupController GroupController { get; set; }
        public Journal Journal { get; set; }

        public FileSystem()
        {
            GroupController = new GroupController();            
            SuperblockController = new SuperblockController();
            UserController = new UserController();
        }

        public bool Authorize(string login, string password)
        {
            UserController = new UserController(login, password);
            
            if(UserController.CurrentUser != null)
            {
                MFTController = new MFTController(UserController);
                return true;
            }

            return false;
        }

        //TODO:
        //Работу всей ФС с тестированием многопользовательской защиты
        //Сделать интерфейс
        //Протестировать большую вложенность директорий(3 и более)

    }
}
