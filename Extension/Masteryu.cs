using System;

namespace Masteryu.Extension
{
    public static class Assistent
    {
        public static int ReadInt(this byte[] buf, int offset, int length)
        {
            int rtn = 0;

            for (int i = 0; i < length - 1; i++)
            {
                int moveleft = 8 * (length - i - 1);
                int temp = ((int)buf[i + offset]) << moveleft;
                rtn += temp;
            }

            rtn += ((int)buf[length + offset - 1]);

            return rtn;
        }

        public static string ReadString(this byte[] buf, int offset, int length, Action<char> charCheck = null)
        {
            string rtn = string.Empty;

            for (int i = 0; i < length; i++)
            {
                char c = (char)buf[i + offset];
                charCheck?.Invoke(c);

                rtn += c;
            }

            return rtn;
        }

        public static byte[] ToBytes(this uint value)
        {
            return new byte[]
            {
                (byte)((value & 0xff000000) >> 24),
                (byte)((value & 0x00ff0000) >> 16),
                (byte)((value & 0x0000ff00) >> 8),
                (byte)(value & 0x000000ff)
            };
        }

        public static T ToEnum<T>(this string value, bool ignoreCase = false)
        {
            return (T) Enum.Parse(typeof(T), value, ignoreCase);
        }
    }
}