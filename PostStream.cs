using ImageFilterASP.Controllers;
using Microsoft.Extensions.Logging;
using System;
using System.IO;
using SliceClass;

namespace ImageFilterASP
{
    class PostStream : Stream
    {
        Stream inner;
        FileStream fs;
        long iter = 0;
        byte[] backup = { };
        private readonly ILogger<HomeController> _logger;
        public PostStream(Stream inner, FileStream fs, ILogger<HomeController> logger)
        {
            _logger = logger;
            this.fs = fs;
            this.inner = inner;
        }

        public override bool CanRead => inner.CanRead;

        public override bool CanSeek => inner.CanSeek;

        public override bool CanWrite => inner.CanWrite;

        public override long Length => inner.Length;

        public override long Position { get => inner.Position; set => inner.Position = value; }

        public override void Flush()
        {
            inner.Flush();
        }

        public override int Read(byte[] buffer, int offset, int count)
        {
            return inner.Read(buffer, offset, count);
        }

        public override long Seek(long offset, SeekOrigin origin)
        {
            return inner.Seek(offset, origin);
        }

        public override void SetLength(long value)
        {
            inner.SetLength(value);
        }
        byte[] startBytes = new byte[] {0x0d, 0x0a, 0x0d, 0x0a};
        byte[] endBytes = new byte[] {0x2d, 0x2d, 0x2d, 0x2d, 0x2d, 0x2d };
        public override void Write(byte[] buffer, int offset, int count)
        {
            //_logger.LogInformation($"----writing-----");
            /*if(iter == 0)
            {*/
            int startPos = 0;
                int endPos = 0;
                int foundBoundaries = 0;
                iter++;
            
            byte[] compBytes = new byte[4];
            if (iter ==1)
            {
               
                for (int i = offset; i < count - 4; i++)
                {
                    Array.Copy(buffer, i, compBytes, 0, 4);
                    if (CompareByteArray(compBytes, startBytes, 4))
                    {
                        startPos = i + 4;
                        _logger.LogInformation($"starting pos: {startPos}");
                        break;
                    }
                }
            }
            else
            {
                //fs.Write(backup, 0, backup.Length);
               // inner.Write(backup, 0, backup.Length);
            }
                
                compBytes = new byte[6];

                for (int i = offset + count; i > offset + 6; i--)
                {
                    //_logger.LogInformation($"byte at {i} is {buffer[i]}");
                    Array.Copy(buffer, i, compBytes, 0, 6);
                    if (CompareByteArray(compBytes, endBytes, 6))
                    {
                        endPos = i-startPos;
                        
                        _logger.LogInformation($"end pos: {endPos}");
                    if (foundBoundaries != 4)
                    {
                        foundBoundaries++;
                    }
                    else
                    {
                        break;
                    }
                       
                    }
                }
            if (endPos == 0) endPos = count;
            //backup = new byte[(count-endPos)];
          //  backup = buffer.Slice(endPos, count+1);
                fs.Write(buffer, offset+startPos, endPos);
                inner.Write(buffer, offset + startPos, endPos);
            /*}
            else
            {
                inner.Write(buffer, offset, count);
            }*/
        }

            ///
            private static bool CompareByteArray(byte[] a1, byte[] a2, int len)
        {
            for (int i = 0; i < len; i++)
            {
                if (a1[i] != a2[i])
                {
                    return false;
                }
            }

            return true;
        }
    }
    


    /*public override void Write(byte[] buffer, int offset, int count)
    {
        //168626701
        //checking for start of image
        //8349220207


        //3255307777713441293
    }
}*/
}
