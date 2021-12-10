using BeetleX.FastHttpApi;
using BeetleX.FastHttpApi.WebSockets;
using Newtonsoft.Json.Linq;
using System;
using System.Linq;

namespace BeetleX.WebChat.Sample
{
    class Program
    {
        static SampleServer mChatServer;
        static void Main(string[] args)
        {
            mChatServer = new SampleServer(80);
            mChatServer.Options(options =>
            {
                options.LogToConsole = true;
                options.LogLevel = BeetleX.EventArgs.LogType.Debug;

            });
            mChatServer.Started(http => {
                http.ResourceCenter.LoadManifestResource(typeof(Program).Assembly);
            });
            mChatServer.Run();
        }
    }

    public class SampleServer : ChatServer<SampleUser, SampleRegion>
    {
        public SampleServer(int port, string host = null) : base(port, host)
        {

        }
        protected override object GetFrameData(WebSocketReceiveArgs e, DataBuffer<byte> frame)
        {
            var token = (JToken)base.GetFrameData(e, frame);
            SampleMessage msg = new SampleMessage();
            msg.Command = token["command"]?.Value<string>();
            msg.Data = token["data"];
            return msg;

        }
        public override ArraySegment<byte> FrameSerialize(DataFrame packet, object body)
        {
            return base.FrameSerialize(packet, body);
        }
    }

    public class SampleMessage : IChatMessage
    {
        public string Command { get; set; }

        public object Data { get; set; }
    }

    public class SampleUser : User
    {
        public override void Receive(object msg)
        {
            base.Receive(msg);
        }

        [Command("signin")]
        public void Signin(JToken data)
        {
            Name = data.Value<string>();
            RegionList(data);
        }
        [Command("send-message")]
        public void Message(JToken data)
        {
            Region?.Send(new
            {
                command = "send-message",
                data = new { Region.ID, this.UserID, this.Name, Message = data.Value<string>() }
            });
        }
        [Command("region-users")]
        public void RegionUsers(JToken data)
        {
            var area = ServerContext.GetRegion(data.Value<string>());
            Send(new
            {
                command = "region-users",
                data = from a in area.Users select new { a.Value.UserID, a.Value.Name }
            });
        }
        [Command("region-list")]
        public void RegionList(JToken data)
        {

            Send(new
            {
                command = "region-list",
                data = ServerContext.ListRegions(0, 1000, false).Item1
            });
        }
        [Command("region-signin")]
        public void RegionSignin(JToken data)
        {
            var room = ServerContext.GetRegion(data.Value<string>());
            room?.Signin(this);

        }
        [Command("region-signout")]
        public void RegionSignout(JToken data)
        {
            var room = ServerContext.GetRegion(data.Value<string>());
            room?.Signout(this);
        }

    }

    public class SampleRegion : Region
    {

        public override void Receive(object message, User user)
        {
            base.Receive(message, user);
        }

        public override void Signin(User user)
        {
            base.Signin(user);
            Send(new
            {
                command = "region-signin",
                data = new { user.UserID, user.Name, AreaRoom = this.ID }
            });
            RefreshUsers();
        }

        public void RefreshUsers()
        {
            Send(new
            {
                command = "region-users",
                data = from a in Users select new { a.Value.UserID, a.Value.Name }
            });
        }

        public override void Signout(User user)
        {
            base.Signout(user);
            Send(new
            {
                command = "region-signout",
                data = new { user.UserID, user.Name, AreaRoom = this.ID }
            });
            RefreshUsers();

        }
    }
}
