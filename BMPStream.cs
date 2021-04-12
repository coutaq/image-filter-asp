//using ImageFilterASP.Controllers;
//using Microsoft.Extensions.Logging;
//using System;
//using System.IO;
//using SliceClass;

//namespace ImageFilterASP
//{
//    class BMPStream : Stream, IModifiable
//    {
//        Stream inner;
//        FileStream fs;
//        MemoryStream image;
//        long iter = 0;
//        int position = 0;
//        string text = "";
//        byte[] backup = { };

//        const int OFFSET = 10;
//        const int OFFSET_LENGTH = 4;
//        int imageStart;

//        private readonly ILogger<HomeController> _logger;


//        public BMPStream(Stream inner, FileStream fs, ILogger<HomeController> logger)ф
//        {ф
//            _logger = logger;
//            this.fs = fs;
//            this.inner = inner;
//            image = new MemoryStream();
//        }

//        public override bool CanRead => inner.CanRead;

//        public override bool CanSeek => inner.CanSeek;

//        public override bool CanWrite => inner.CanWrite;

//        public override long Length => inner.Length;

//        public override long Position { get => inner.Position; set => inner.Position = value; }

//        public override void Flush()
//        {
//            inner.Flush();
//        }

//        public override int Read(byte[] buffer, int offset, int count)
//        {
//            return inner.Read(buffer, offset, count);
//        }

//        public override long Seek(long offset, SeekOrigin origin)
//        {
//            return inner.Seek(offset, origin);
//        }

//        public override void SetLength(long value)
//        {
//            inner.SetLength(value);
//        }

//        byte[] startBytes = new byte[] { 0x0d, 0x0a, 0x0d, 0x0a };
//        byte[] endBytes = new byte[] { 0x2d, 0x2d, 0x2d, 0x2d, 0x2d, 0x2d };
//        byte[] imgBytes = new byte[] { 0x49, 0x44, 0x41, 0x54 };
//        byte[] imgEnd = new byte[] { 0x49, 0x45, 0x4E, 0x44 };
//        public override void Write(byte[] buffer, int offset, int count)
//        {
//            int startPos = 0;
//            int endPos = 0;
//            int foundBoundaries = 0;
//            int foundStarts = 0;
//            int imgData = 0;
//            iter++;

//            byte[] compBytes = new byte[4];
//            if (iter == 1)
//            {
//                for (int i = offset; i < count - 4; i++)
//                {
//                    Array.Copy(buffer, i, compBytes, 0, 4);
//                    if (CompareByteArray(compBytes, startBytes, 4))
//                    {
//                        startPos = i + 4;
//                        foundStarts++;
//                        if (foundStarts > 1)
//                        {
//                            byte[] textBytes = new byte[startPos];
//                            textBytes = buffer.Slice(0, startPos);
//                            text = System.Text.Encoding.Default.GetString(textBytes);
//                            var filterStart = text.IndexOf("filterName") + 1 + "filterName".Length;
//                            var filterEnd = text.LastIndexOf("------WebKitFormBoundary");

//                            text = text.Substring(filterStart, filterEnd - filterStart).Trim();
//                            _logger.LogInformation($"current filter: {text}");
//                            break;
//                        }
//                    }
//                }
//            }
//            compBytes = new byte[6];

//            for (int i = offset + count; i > offset + 6; i--)
//            {
//                Array.Copy(buffer, i, compBytes, 0, 6);
//                if (CompareByteArray(compBytes, endBytes, 6))
//                {
//                    endPos = count - i;
//                    if (foundBoundaries != 4)
//                    {
//                        foundBoundaries++;
//                    }
//                    else
//                    {
//                        break;
//                    }

//                }
//            }
//            if (endPos == 0) endPos = count;
//            byte[] data = buffer.Slice(offset + startPos, count);
//            for (int i = 0; i < data.Length; i++)
//            {
//                //data[i] = (byte)(data[i]+data[i]);
//                _logger.LogInformation($"byte at {i} is {data[i]}");
//            }
//            fs.Write(data);
//            inner.Write(data);

//        }

//        private static bool CompareByteArray(byte[] a1, byte[] a2, int len)
//        {
//            for (int i = 0; i < len; i++)
//            {
//                if (a1[i] != a2[i])
//                {
//                    return false;
//                }
//            }

//            return true;
//        }
//    }

//}