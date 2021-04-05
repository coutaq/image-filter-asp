using System;
namespace ImageFilterASP
{
    public class DummyFilter: IFilter
    {
        public byte ModifyByte(byte currentByte, int position)
        {
            if (position % 3 == 0)
            {
                currentByte = (byte)(currentByte + currentByte);
            }
            return currentByte;
        }
    }
}
