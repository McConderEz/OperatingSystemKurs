using System;
using System.Collections.Generic;
using System.IO.Pipes;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InterprocessCommunication.BL
{
    public class NamedPipeServer : IDisposable
    {
        private readonly NamedPipeServerStream _pipeServer;

        public NamedPipeServer(string pipeName)
        {
            _pipeServer = new NamedPipeServerStream(pipeName, PipeDirection.Out, 1);
            _pipeServer.WaitForConnection();
        }

        public void Send(int data)
        {
            if (!_pipeServer.IsConnected)
            {
                throw new InvalidOperationException("Pipe is not connected.");
            }

            _pipeServer.WriteByte((byte)data);
        }

        public void Close()
        {
            _pipeServer.Dispose();
        }

        public void Dispose()
        {
            _pipeServer.Dispose();
        }
    }
}
