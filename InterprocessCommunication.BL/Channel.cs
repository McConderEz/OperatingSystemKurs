using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InterprocessCommunication.BL
{
    public class Channel<T> : IEnumerable<T>
    {
        private readonly BlockingCollection<T> _collection = new BlockingCollection<T>();

        public void Send(T item)
        {
            _collection.Add(item);
        }

        public void Close()
        {
            _collection.CompleteAdding();
        }

        public IEnumerator<T> GetEnumerator()
        {
            return _collection.GetConsumingEnumerable().GetEnumerator();
        }

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}
