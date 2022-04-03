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
        public static HttpApiServer UseProtobufController(this HttpApiServer server, Action<WebSocketReceiveArgs> handler = null)
        {
            server.WebSocketReceive = async (o, e) =>
            {
                try
                {
                    var msg = e.Frame.Body;
                    var action = ProtobufFrameSerializer.BinaryDataFactory.GetHandler(msg);
                    if (action != null)
                    {
                        if (!action.IsVoid)
                        {
                            if (action.IsTaskResult)
                            {

                                Task task = (Task)action.Execute(e, msg);
                                await task;
                                if (action.HasTaskResultData)
                                {
                                    var result = action.GetTaskResult(task);
                                    e.ResponseBinary(result);
                                }
                            }
                            else
                            {
                                var result = action.Execute(e, msg);
                                e.ResponseBinary(result);
                            }
                        }
                    }
                    else
                    {
                        handler?.Invoke(e);
                    }
                }
                catch (Exception e_)
                {
                    e.Request.Server.GetLog(BeetleX.EventArgs.LogType.Warring)
                    ?.Log(BeetleX.EventArgs.LogType.Error, e.Request.Session, $"Websocket packet process error {e_.Message}");
                }
            };
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
