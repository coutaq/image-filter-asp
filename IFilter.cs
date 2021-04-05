using System;
namespace ImageFilterASP
{
    public interface IFilter
    {
        public byte ModifyByte(byte currentByte, int position);
    }
}
