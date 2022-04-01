using BeetleX.Http.Clients.WebSockets;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Websocket.ProtobufPacket.Client
{
    public class ProtobufClient : BinaryClient
    {
        public ProtobufClient(string host) : base(host) { }

        public static BinaryDataFactory BinaryDataFactory { get; private set; } = new BinaryDataFactory();

        protected override object OnDeserializeObject(ArraySegment<byte> data)
        {
            return BinaryDataFactory.Deserialize(data);
        }

        protected override ArraySegment<byte> OnSerializeObject(object data)
        {
            return BinaryDataFactory.Serializable(data);
        }
    }
}
