using System;
using System.Collections.Generic;
using System.Text;

namespace BeetleX.Samples.Tcp.PushFile.Messages
{
    public class FileReader : IDisposable
    {
        public FileReader(string file)
        {
            mInfo = new System.IO.FileInfo(file);
            mPages = (int)(mInfo.Length / mSize);
            if (mInfo.Length % mSize > 0)
                mPages++;
            FileSize = mInfo.Length;
            mBuffer = new byte[mSize];
            mReader = mInfo.OpenRead();
        }

        private System.IO.Stream mReader;

        private byte[] mBuffer;

        private System.IO.FileInfo mInfo;

        private int mPages;

        private int mSize = 1024 * 16;

        private int mIndex;

        public int Index => mIndex;

        public int Size => mSize;

        public int Pages => mPages;

        public long FileSize { get; private set; }

        public long CompletedSize { get; private set; }

        public bool Completed => mIndex == mPages;

        public FileContentBlock Next()
        {
            FileContentBlock result = new FileContentBlock();
            result.FileName = mInfo.Name;
            byte[] data;
            if (mIndex == mPages - 1)
            {
                data = new byte[mInfo.Length - mIndex * mSize];
                result.Eof = true;
            }
            else
            {
                data = mBuffer;
            }
            CompletedSize += data.Length;
            mReader.Read(data, 0, data.Length);
            result.Index = mIndex;
            result.Data = data;
            mIndex++;


            return result;
        }

        public void Dispose()
        {
            throw new NotImplementedException();
        }
    }
}
