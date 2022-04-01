using ProtoBuf;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Websocket.ProtobufPacket
{
    [BinaryType(1)]
    [ProtoContract]
    public class User
    {
        [ProtoMember(1)]
        public string Name { get; set; }
        [ProtoMember(2)]
        public string Email { get; set; }
        [ProtoMember(3)]
        public DateTime ResultTime { get; set; }
    }
}
