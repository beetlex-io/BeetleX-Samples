using MessagePack;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Websocket.MessagePackPacket
{
    [BinaryType(1)]
    [MessagePackObject]
    public class User
    {
        [Key(0)]
        public string Name { get; set; }

        [Key(1)]
        public string Email { get; set; }

        [Key(2)]
        public DateTime ResultTime { get; set; }
    }
}
