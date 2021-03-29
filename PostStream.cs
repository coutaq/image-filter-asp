using System;
using System.IO;

namespace ImageFilterASP
{
    public class PostStream: System.IO.Stream
    {
        public PostStream()
        {
        }
        
        public override bool CanRead => throw new NotImplementedException();

        public override bool CanSeek => throw new NotImplementedException();

        public override bool CanWrite => true;

        public override long Length => throw new NotImplementedException();

        public override long Position { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

        public override void Flush()
        {
            throw new NotImplementedException();
        }

        public override int Read(byte[] buffer, int offset, int count)
        {
            throw new NotImplementedException();
        }

        public override long Seek(long offset, SeekOrigin origin)
        {
            throw new NotImplementedException();
        }

        public override void SetLength(long value)
        {
            throw new NotImplementedException();
        }

        public override void Write(byte[] buffer, int offset, int count)
        {
            //168626701
            //checking for start of image
            //8349220207
            uint startPos = 0;
            for (int i = offset; i<count-4; i++)
            {
                if (buffer[i] == 83 && buffer[i+1] == 49 && buffer[i+2]==220 && buffer[i + 3] == 207)
                {
                    startPos = (uint)(i + 3);
                    Console.WriteLine(startPos);
                    break;
                }
            }
            for (int i = offset+count; i > offset + 6; i--)
            {
                Console.WriteLine($"byte at {i} is {buffer[i]}");
                if (buffer[i] == 83 && buffer[i + 1] == 49 && buffer[i + 2] == 220 && buffer[i + 3] == 207)
                {
                    startPos = (uint)(i + 3);
                }
            }
            //3255307777713441293
        }
    }
}
