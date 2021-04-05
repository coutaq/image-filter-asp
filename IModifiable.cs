using System;
using System.IO;

namespace ImageFilterASP
{
    public abstract class IModifiable:Stream
    {
        protected Stream stream;
        public IModifiable(Stream stream)
        {
            this.stream = stream;
        }
        public virtual void Apply(IFilter filter)
        {
            byte[] bytes = new byte[stream.Length];
            stream.Read(bytes);
            stream.Dispose();
            for (int i =0; i<stream.Length; i++)
            {
                bytes[i] = filter.ModifyByte(bytes[i], i);
            }
            stream.Write(bytes);
        }
    }
}
