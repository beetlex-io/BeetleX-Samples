using System;
using System.Threading.Tasks;
using BeetleX;
using BeetleX.Buffers;
using BeetleX.Clients;

namespace Client
{
    class Program
    {
        static Task Main(string[] args)
        {
            var client = SocketFactory.CreateClient<AsyncTcpClient, StringPacket>("127.0.0.1", 9090);
            client.PacketReceive = (o, d) => {
                Console.WriteLine($"{DateTime.Now} {d}");
            };
            while (true)
            {
                Console.Write("Enter Name:");
                var line = Console.ReadLine();
                client.Send(line);
                
            }
        }
    }

    public class StringPacket : BeetleX.Packets.FixeHeaderClientPacket
    {
        public override IClientPacket Clone()
        {
            return new StringPacket();
        }

        protected override object OnRead(IClient client, PipeStream stream)
        {
            return stream.ReadString(CurrentSize);
        }

        protected override void OnWrite(object data, IClient client, PipeStream stream)
        {
            stream.Write((string)data);
        }
    }

}
