using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ProtoBuf;

namespace Websocket.ProtobufPacket
{


    [AttributeUsage(AttributeTargets.Class)]
    public class BinaryTypeAttribute : Attribute
    {
        public BinaryTypeAttribute(int id)
        {

            this.ID = id;
        }

        public int ID { get; set; }
    }

    public class BinaryDataFactory
    {

        private Dictionary<int, Type> mMessageTypes = new Dictionary<int, Type>();

        private Dictionary<Type, int> mMessageIDs = new Dictionary<Type, int>();

        public void RegisterComponent<T>()
        {
            RegisterComponent(typeof(T));
        }

        public void RegisterComponent(Type type)
        {
            foreach (var item in type.Assembly.GetTypes())
            {
                BinaryTypeAttribute[] bta = (BinaryTypeAttribute[])item.GetCustomAttributes(typeof(BinaryTypeAttribute), false);
                if (bta != null && bta.Length > 0)
                {
                    mMessageTypes[bta[0].ID] = item;
                    mMessageIDs[item] = bta[0].ID;
                }
            }
        }

        public ArraySegment<byte> Serializable(object data)
        {
            MemoryStream memory = new MemoryStream();
            var type = GetTypeID(data.GetType(), true);
            memory.Write(type);
            Serializer.Serialize(memory, data);
            return new ArraySegment<byte>(memory.GetBuffer(), 0, (int)memory.Length);
        }

        public object Deserialize(byte[] data)
        {
            return Deserialize(new ArraySegment<byte>(data, 0, data.Length));
        }

        public object Deserialize(ArraySegment<byte> data)
        {
            MemoryStream memory = new MemoryStream(data.Array, data.Offset, data.Count);
            byte[] id = new byte[4];
            memory.Read(id, 0, 4);
            Type type = GetMessageType(id, true);
            return Serializer.Deserialize(type, memory);
        }

        public byte[] GetTypeID(Type type, bool littleEndian)
        {
            if (mMessageIDs.TryGetValue(type, out int value))
            {

                if (!littleEndian)
                {
                    value = BeetleX.Buffers.BitHelper.SwapInt32(value);
                }
                return BitConverter.GetBytes(value);

            }
            throw new Exception($"binary websocket {type} id not found!");
        }

        public Type GetMessageType(int id)
        {
            mMessageTypes.TryGetValue(id, out Type result);
            return result;
        }

        public Type GetMessageType(byte[] data, bool littleEndian)
        {
            int value = BitConverter.ToInt32(data, 0);
            if (!littleEndian)
                value = BeetleX.Buffers.BitHelper.SwapInt32(value);
            return GetMessageType(value);
        }
    }
}
