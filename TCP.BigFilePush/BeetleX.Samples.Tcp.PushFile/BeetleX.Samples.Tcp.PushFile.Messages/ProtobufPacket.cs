using BeetleX.Buffers;
using BeetleX.Clients;
using BeetleX.Packets;
using System;
using System.Collections.Generic;
using System.Text;

namespace BeetleX.Samples.Tcp.PushFile.Messages
{
    public class ProtobufPacket : FixedHeaderPacket
    {
        static ProtobufPacket()
        {
            TypeHeaer.Register(typeof(ProtobufPacket).Assembly);
        }

        public static CustomTypeHeader TypeHeaer { get; private set; } = new CustomTypeHeader(MessageIDType.INT);

        public override IPacket Clone()
        {
            return new ProtobufPacket();
        }

        protected override object OnRead(ISession session, PipeStream stream)
        {
            Type type = TypeHeaer.ReadType(stream);
            return ProtoBuf.Meta.RuntimeTypeModel.Default.Deserialize(type, stream, null, null, CurrentSize - 4);
        }
        protected override void OnWrite(ISession session, object data, PipeStream stream)
        {
            TypeHeaer.WriteType(data, stream);
            ProtoBuf.Meta.RuntimeTypeModel.Default.Serialize(stream, data);
        }
    }

    public class ProtobufClientPacket : Packets.FixeHeaderClientPacket
    {

        static ProtobufClientPacket()
        {
            TypeHeaer.Register(typeof(ProtobufClientPacket).Assembly);
        }

        public static CustomTypeHeader TypeHeaer { get; private set; } = new CustomTypeHeader(MessageIDType.INT);

        public override IClientPacket Clone()
        {
            return new ProtobufClientPacket();
        }

        protected override object OnRead(IClient client, PipeStream stream)
        {
            Type type = TypeHeaer.ReadType(stream);
            return ProtoBuf.Meta.RuntimeTypeModel.Default.Deserialize(type, stream, null, null, CurrentSize - 4);
        }

        protected override void OnWrite(object data, IClient client, PipeStream stream)
        {
            TypeHeaer.WriteType(data, stream);
            ProtoBuf.Meta.RuntimeTypeModel.Default.Serialize(stream, data);
        }
    }

}
