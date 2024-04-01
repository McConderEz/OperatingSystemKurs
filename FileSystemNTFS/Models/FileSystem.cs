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
        public Journal Journal { get; set; }

        public FileSystem()
        {
            MFTController = new MFTController();
            SuperblockController = new SuperblockController();
        }

        //TODO:
        //Протестировать действия над каталогами и файлами(перемещение, копирование, переименование)
        //Сделать журналирование
        //Сделать многопользовательскую защиту
        //Протестировать большую вложенность директорий(3 и более)

        //Копирование каталога

        //Список файла данной директории
    }
}
