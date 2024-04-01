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

            using (var fs = new FileStream(fullPath, FileMode.Open, FileAccess.Write))
            {
                using(var bw = new BinaryWriter(fs))
                {
                    if (FileSystem.SuperblockController.IsClusterFree(i) && dataSize <= 4096) // Запись данных в файл, размер которых не превышает размер одного кластера(4кб)
                    {
                        FileSystem.SuperblockController.MarkClusterAsUsed(dataBytes, i);
                        mftItem.Attributes.IndexesOnClusterBitmap.Add(new Indexer(i, dataSize)); //В индекс будем записывать первое значение - индекс кластера, второе - фактический размер занятого кластера из 4 кб
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
                mftItem.Attributes.Length = new FileInfo(fullPath).Length;
                stream.Dispose();
                stream.Close();
            }

                    mftItem.Attributes.IndexesOnClusterBitmap.Add(new Indexer((int)startIndex, (int)(FileSystem.SuperblockController.Superblock.BitmapOffset - 1)));
                    mftItem.Attributes.Length = (ulong)(new FileInfo(fullPath)).Length;
                    FileSystem.MFTController.Update(mftItem);
                }
            }


        }

    }
}
