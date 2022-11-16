using BeetleX.MQTT.Messages;
using System;
using System.Text;

namespace BeetleX.MQTT.Server
{
    class Program : IApplication
    {
        private static ServerBuilder<Program, MQTTUser, MQTTPacket> server;
        static void Main(string[] args)
        {
            server = new ServerBuilder<Program, MQTTUser, MQTTPacket>();
            server.ConsoleOutputLog = true;
            server.SetOptions(option =>
            {
                option.DefaultListen.Port = 9090;
                option.DefaultListen.Host = "127.0.0.1";
                option.LogLevel = EventArgs.LogType.Trace;
            })
            .OnMessageReceive<CONNECT>(e =>
            {
                e.GetLoger(EventArgs.LogType.Info)?.Log(EventArgs.LogType.Info, e.NetSession, $"{e.NetSession.RemoteEndPoint} connect name:{e.Message.UserName} password:{e.Message.Password}");
                e.Session.UserName = e.Message.UserName;
                e.Session.ID = e.Message.ClientID;
                CONNACK ack = new CONNACK();
                e.Return(ack);
            })
            .OnMessageReceive<SUBSCRIBE>(e =>
            {
                e.GetLoger(EventArgs.LogType.Info)?.Log(EventArgs.LogType.Info, e.NetSession, $"{e.Session.ID} subscribe {e.Message}");
                SUBACK ack = new SUBACK();
                ack.Identifier = e.Message.Identifier;
                ack.Status = QoSType.MostOnce;
                e.Return(ack);
            })
            .OnMessageReceive<UNSUBSCRIBE>(e =>
            {
                e.GetLoger(EventArgs.LogType.Info)?.Log(EventArgs.LogType.Info, e.NetSession, $"{e.Session.ID} unsubscribe {e.Message}");
                UNSUBACK ack = new UNSUBACK();
                e.Return(ack);
            })
            .OnMessageReceive<PUBLISH>(e =>
            {

                var data = Encoding.UTF8.GetString(e.Message.PayLoadData.Array, e.Message.PayLoadData.Offset, e.Message.PayLoadData.Count);
                e.GetLoger(EventArgs.LogType.Info)?.Log(EventArgs.LogType.Info, e.NetSession, $"{e.Session.ID} publish {e.Message.Topic}@ {e.Message.Identifier} data:{data}");
                PUBACK ack = new PUBACK();
                ack.Identifier = e.Message.Identifier;
                e.Return(ack);

            })
            .OnMessageReceive<PINGREQ>(e =>
            {
                PINGRESP resp = new PINGRESP();
                e.Return(resp);
            })
            .OnMessageReceive(e =>
            {

            })
            .Run();
            Console.Read();
        }

        public void Init(IServer server)
        {
            server.GetLoger(EventArgs.LogType.Info)?.Log(EventArgs.LogType.Info, null, "application data init");
        }
    }

    public class MQTTUser : ISessionToken
    {
        public string ID { get; set; }

        public string UserName { get; set; }
        public void Dispose()
        {

        }

        public void Init(IServer server, ISession session)
        {
            server.GetLoger(EventArgs.LogType.Info)?.Log(EventArgs.LogType.Info, session, "session data init");
        }
    }
}
