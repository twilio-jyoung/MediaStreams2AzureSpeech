using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace MediaStreams.SpeechToText
{
    public class EchoStream : MemoryStream
    {
        private readonly ManualResetEvent _DataReady = new ManualResetEvent(false);
        private readonly ConcurrentQueue<byte[]> _Buffers = new ConcurrentQueue<byte[]>();

        public bool DataAvailable { get { return !_Buffers.IsEmpty; } }

        public override void Write(byte[] buffer, int offset, int count)
        {
            _Buffers.Enqueue(buffer.Take(count).ToArray()); // add new data to buffer
            _DataReady.Set(); // allow waiting reader to proceed
        }

        public override int Read(byte[] buffer, int offset, int count)
        {
            _DataReady.WaitOne(); // block until there's something new to read

            byte[] lBuffer;

            if (!_Buffers.TryDequeue(out lBuffer)) // try to read
            {
                _DataReady.Reset();
                return -1;
            }

            if (!DataAvailable)
                _DataReady.Reset();

            Array.Copy(lBuffer, buffer, lBuffer.Length);
            return lBuffer.Length;
        }
    }
}
