using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileSystemNTFS.BL.Controller.FileSystemController
{
    public partial class FileSystemController
    {
        public void WriteBytesToFile(string fullPath, MemoryStream stream, byte[] dataBytes, int dataSize)
        {
            using (BinaryWriter writer = new BinaryWriter(stream))
            {
                writer.Write(dataBytes);
                using (FileStream fileStream = new FileStream(fullPath, FileMode.Append, FileAccess.Write
                    , FileShare.None, dataSize, FileOptions.WriteThrough))
                {
                    stream.WriteTo(fileStream);
                    fileStream.Close();
                }
                writer.Flush();
                writer.Close();
            }
        }
    }
}
