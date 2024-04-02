using System;
using System.Collections.Generic;
using System.IO.MemoryMappedFiles;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace InterprocessCommunication.BL
{
    class MemoryMappedFile<T> : IDisposable where T : unmanaged
    {
        private readonly MemoryMappedFile _memoryMappedFile;
        private readonly MemoryMappedViewAccessor _accessor;

        public MemoryMappedFile(string name, int capacity)
        {
            _memoryMappedFile = MemoryMappedFile.CreateOrOpen(name, capacity * Unsafe.SizeOf<T>());
            _accessor = _memoryMappedFile.CreateViewAccessor();
        }

        public void Write(T value)
        {
            _accessor.Write(0, ref value);
        }

        public T Read()
        {
            T value = default;
            _accessor.Read(0, out value);
            return value;
        }

        public void Dispose()
        {
            _accessor.Dispose();
            _memoryMappedFile.Dispose();
        }
    }
}
