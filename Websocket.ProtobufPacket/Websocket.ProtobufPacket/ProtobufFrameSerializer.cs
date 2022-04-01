using BeetleX.Buffers;
using BeetleX.FastHttpApi.WebSockets;
using BeetleX.FastHttpApi;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Websocket.ProtobufPacket
{
    public static class ProtobufExtensions
    {
        public static HttpApiServer RegisterProtobuf<T>(this HttpApiServer server)
        {
            ProtobufFrameSerializer.BinaryDataFactory.RegisterComponent<T>();
            return server;
        }
    }
    public class ProtobufFrameSerializer : IDataFrameSerializer
    {
        public static BinaryDataFactory BinaryDataFactory { get; set; } = new BinaryDataFactory();
        public object FrameDeserialize(DataFrame data, PipeStream stream)
        {
            var buffers = new byte[data.Length];
            stream.Read(buffers, 0, buffers.Length);
            return BinaryDataFactory.Deserialize(buffers);
        }

        public void FrameRecovery(byte[] buffer)
        {

        }

        public ArraySegment<byte> FrameSerialize(DataFrame packet, object body)
        {
            return BinaryDataFactory.Serializable(body);
        }
    }
}
