using FileSystemNTFS.BL.Models;
using MultiuserProtection.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileSystemNTFS.BL.Controller.FileSystemController
{
    public partial class FileSystemController
    {

        public void WriteFile(string name, StringBuilder data)
        {
            var fullPath = Path.Combine(CurrentPath, name + ".bin");

            if (!File.Exists(fullPath))
            {
                throw new FileNotFoundException("Файл не найден!");
            }

            var mftItem = FileSystem.MFTController.MFT.Entries.SingleOrDefault(x => x.Attributes.FullPath.Equals(fullPath, StringComparison.OrdinalIgnoreCase));

            if(mftItem == null)
                throw new ArgumentNullException("Запись MFT не может быть пустой!",nameof(mftItem));

            if (FileSystem.UserController.CurrentUser.AccountType == AccountType.Administrator ||
                        mftItem.Attributes.AccessFlags.O == AttributeFlags.Modify ||
                        mftItem.Attributes.AccessFlags.O == AttributeFlags.Write ||
                        mftItem.Attributes.AccessFlags.O == AttributeFlags.FullControl ||
                        (mftItem.Attributes.OwnerId == FileSystem.UserController.CurrentUser.Id && (mftItem.Attributes.AccessFlags.U == AttributeFlags.FullControl || mftItem.Attributes.AccessFlags.U == AttributeFlags.Modify || mftItem.Attributes.AccessFlags.U == AttributeFlags.Write)) ||
                        (mftItem.Attributes.Groups.Any(FileSystem.UserController.CurrentUser.Groups.Contains) && (mftItem.Attributes.AccessFlags.G == AttributeFlags.FullControl || mftItem.Attributes.AccessFlags.G == AttributeFlags.Modify || mftItem.Attributes.AccessFlags.U == AttributeFlags.Write)))
            {

                using (MemoryStream stream = new MemoryStream())
                {
                    byte[] dataBytes = Encoding.UTF8.GetBytes(data.ToString());
                    int dataSize = dataBytes.Length;
                    for (var i = 0; i < FileSystem.SuperblockController.Superblock.ClusterBitmap.Length; i++)
                    {
                        if (FileSystem.SuperblockController.IsClusterFree(i) && dataSize <= 4096) // Запись данных в файл, размер которых не превышает размер одного кластера(4кб)
                        {
                            FileSystem.SuperblockController.MarkClusterAsUsed(dataBytes, i);
                            mftItem.Attributes.IndexesOnClusterBitmap.Add(new Indexer(i)); //В индекс будем записывать первое значение - индекс кластера, второе - фактический размер занятого кластера из 4 кб
                            WriteBytesToFile(fullPath, stream, dataBytes, dataSize);
                            break;
                        }
                        else if (FileSystem.SuperblockController.IsClusterFree(i) && dataSize > 4096) // Запись данных в файл, размер которых больше одного кластера(4кб)
                        {
                            FileSystem.SuperblockController.MarkClustersAsUsedForLargeFile(mftItem, dataBytes, dataSize, i);
                            WriteBytesToFile(fullPath, stream, dataBytes, dataSize);
                            break;
                        }
                    }
                    mftItem.Attributes.Length = (ulong)new FileInfo(fullPath).Length;
                    mftItem.Attributes.TimeMarks.ModificationTime = DateTime.Now;
                    stream.Dispose();
                    stream.Close();
                }

                FileSystem.MFTController.Update(mftItem);

                Save();
            }
            else
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("Недостаточно прав");
                Console.ResetColor();
            }

        }

    }
}
