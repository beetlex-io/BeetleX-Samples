using BeetleX.Packets;
using ProtoBuf;
using System;
using System.Collections.Generic;
using System.Text;

namespace BeetleX.Samples.Tcp.PushFile.Messages
{
    [MessageType(1)]
    [ProtoContract]
    public class FileContentBlock : IMessageSubmitHandler
    {
        [ProtoMember(1)]
        public string FileName { get; set; }
        [ProtoMember(2)]
        public bool Eof { get; set; }
        [ProtoMember(3)]
        public int Index { get; set; }
        [ProtoMember(4)]
        public byte[] Data { get; set; }

        public Action<FileContentBlock> Completed { get; set; }

        public void Execute(object sender, object message)
        {
            Completed?.Invoke(this);
        }
    }
}
