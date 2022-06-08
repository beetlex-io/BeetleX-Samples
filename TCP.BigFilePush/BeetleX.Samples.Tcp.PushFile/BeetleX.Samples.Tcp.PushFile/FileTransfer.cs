using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace BeetleX.Samples.Tcp.PushFile
{
    public class FileTransfer : IDisposable
    {
        public FileTransfer(string name)
        {
            Name = name;
            Stream = System.IO.File.Open(name, FileMode.OpenOrCreate);
        }

        public string Name { get; set; }

        public Stream Stream { get; private set; }

        public void Dispose()
        {
            Stream?.Flush();
            Stream?.Dispose();

        }
    }
}
