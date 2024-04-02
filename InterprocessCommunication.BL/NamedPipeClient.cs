using System;
using System.Collections.Generic;
using System.IO.Pipes;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InterprocessCommunication.BL
{
    public class NamedPipeClient : IEnumerable<int>, IDisposable
    {
        private readonly NamedPipeClientStream _pipeClient;

        public NamedPipeClient(string pipeName)
        {
            _pipeClient = new NamedPipeClientStream(".", pipeName, PipeDirection.In);
            _pipeClient.Connect();
        }

        public void Dispose()
        {
            _pipeClient.Dispose();
        }

        public IEnumerator<int> GetEnumerator()
        {
            while (_pipeClient.IsConnected)
            {
                int data = _pipeClient.ReadByte();
                yield return data;
            }
        }

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public int Receive()
        {
            return _pipeClient.ReadByte();
        }
    }
}
