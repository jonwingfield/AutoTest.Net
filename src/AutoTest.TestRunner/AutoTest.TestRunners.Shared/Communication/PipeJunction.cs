﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO.Pipes;
using System.Threading;
using System.IO;

namespace AutoTest.TestRunners.Shared.Communication
{
    public class PipeJunction
    {
        private List<Thread> _pipes = new List<Thread>();
        private Stack<byte[]> _messages = new Stack<byte[]>();

        public PipeJunction(string pipeName)
        {
            var handler = new Thread(startServer);
            _pipes.Add(handler);
            handler.Start(pipeName);
        }

        public void Combine(string pipe)
        {
            var handler = new Thread(run);
            _pipes.Add(handler);
            handler.Start(pipe);
        }

        private void run(object state)
        {
            new PipeClient().Listen(state.ToString(), (x) => { queue(x); });
        }

        private void queue(byte[] item)
        {
            _messages.Push(item);
        }

        private void startServer(object state)
        {
            try
            {
                var pipe = state.ToString();
                using (var server = new NamedPipeServerStream(pipe, PipeDirection.Out))
                {
                    server.WaitForConnection();
                    while (server.IsConnected)
                    {
                        if (_messages.Count == 0)
                        {
                            Thread.Sleep(100);
                            continue;
                        }
                        var bytes = _messages.Pop();
                        var buffer = new byte[bytes.Length + 1];
                        Array.Copy(bytes, buffer, bytes.Length);
                        buffer[buffer.Length - 1] = 0;
                        server.Write(buffer, 0, buffer.Length);
                    }
                }
            }
            catch
            {
            }
        }
    }
}
