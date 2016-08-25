using System;
using System.IO;
using System.IO.Compression;

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
            return (T)Enum.Parse(typeof(T), value, ignoreCase);
        }

        public static byte[] Compress(this Stream input)
        {
            MemoryStream compressStream = new MemoryStream();            
            using(DeflateStream compressor = new DeflateStream(compressStream, CompressionLevel.Optimal))
            {
                input.CopyTo(compressor);
            }
            return compressStream.ToArray();
        }

        public static Stream Decompress(this byte[] data, int index, int count)
        {
            MemoryStream output = new MemoryStream();
            using(MemoryStream compressedStream = new MemoryStream(data, index, count))
            using(DeflateStream decompressor = new DeflateStream(compressedStream, CompressionMode.Decompress))
            {
                decompressor.CopyTo(output);
            }
            output.Position = 0;
            return output;
        }
    }
    
    /// <summary>
    /// Recode from Annex E's CRC-32bit algorithm using c
    /// </summary>
    public static class Crc32Bit
    {
        [ThreadStatic]
        private static uint[] _crcTbl = null;

        private const int LENGTH = 256;

        private static void Init()
        {
            lock (typeof(Crc32Bit))
            {
                if (_crcTbl != null)
                    return;

                _crcTbl = new uint[LENGTH];
                uint c;
                for (int i = 0; i < LENGTH; i++)
                {
                    c = (uint)i;
                    for (int j = 0; j < 8; j++)
                    {
                        if ((c & 1) == 1)
                        {
                            c = 0xEDB88320 ^ (c >> 1);
                        }
                        else
                        {
                            c >>= 1;
                        }
                    }
                    _crcTbl[i] = c;
                }
            }
        }

        public static uint Value(byte[] buf, int offset, int length)
        {
            uint c = 0xffffffff;

            if (_crcTbl == null)
            {
                Init();
            }

            for (int i = 0; i < length; i++)
            {
                uint index = (c ^ buf[i + offset]) & 0xff;
                c = _crcTbl[index] ^ (c >> 8);
            }

            return ~c;
        }
    }

    public static class Adler32
    {
        private const ushort ADLER = 65521;
        public static uint Value(byte[] buf, int offset, int length)
        {
            uint a = 1;
            uint b = 0;

            for (int i = 0; i < length; i++)
            {
                a = (a + buf[i + offset]) % ADLER;
                b = (a + b) % ADLER;
            }

            return (b << 16) | a;
        }
    }
}