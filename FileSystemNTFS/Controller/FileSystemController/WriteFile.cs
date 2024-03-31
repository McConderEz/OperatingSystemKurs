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
                    byte[] dataBytes = Encoding.UTF8.GetBytes(data.ToString());
                    long startIndex = bw.Seek((int)FileSystem.SuperblockController.Superblock.BitmapOffset, SeekOrigin.Begin);
                    bw.Write(dataBytes, 0, dataBytes.Length);
                    FileSystem.SuperblockController.Superblock.BitmapOffset = bw.Seek(0, SeekOrigin.End) + 1;

                    mftItem.Attributes.IndexesOnClusterBitmap.Add(new Indexer((int)startIndex, (int)(FileSystem.SuperblockController.Superblock.BitmapOffset - 1)));
                    mftItem.Attributes.Length = (ulong)(new FileInfo(fullPath)).Length;
                    FileSystem.MFTController.Update(mftItem);
                }
            }


        }

    }
}
