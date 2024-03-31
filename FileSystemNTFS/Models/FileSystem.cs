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
    [DataContract]
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

        //TODO:ФС        
        //Создание файл
        //Запись в конец файла
        //Чтение файла
        //Удаление файла
        //Переименование файла
        //Копирование файла
        
        //Сделать создание бинарного файла с данными

        //Создание каталога
        //Удаление каталога
        //Переименование каталога
        //Перемещение каталога

    }
}
