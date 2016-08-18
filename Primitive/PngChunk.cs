using System;
using System.Diagnostics;
using Masteryu.Extension;

namespace Masteryu.Png
{
    /// <summary>
    /// Each chunk consists of three or four fields: 
    /// Length, Type, Data(does not exist when Length is 0), CRC
    /// </summary>
    public sealed class PngChunk
    {
        internal const int LENGTHBYTESCOUNT = 4;
        internal const int TYPEBYTESCOUNT = 4;
        internal const int HEADBYTESCOUNT = LENGTHBYTESCOUNT + TYPEBYTESCOUNT;
        internal const int CRCBYTESCOUNT = 4;
        
        private readonly byte[] _buf;
        private readonly int _offset;
        private ChunkData _data;
        public readonly bool IsReadOnly;

        public int Length
        {
            get
            {
                if (IsReadOnly)
                {
                    return _buf.ReadInt(_offset, LENGTHBYTESCOUNT);
                }
                else
                {
                    throw new NotImplementedException();
                }
            }
        }

        public ChunkType Type
        {
            get
            {
                if (IsReadOnly)
                {
                    int offset = _offset + LENGTHBYTESCOUNT;
                    int value = _buf.ReadInt(offset, TYPEBYTESCOUNT);

                    return (ChunkType)value;
                }
                else
                {
                    throw new NotImplementedException();
                }
            }
        }

        public ChunkData Data
        {
            get
            {
                if (Length == 0)
                    return null;

                if (IsReadOnly)
                {
                    if (_data == null)
                        _data = ChunkData.CreatInstance(Type, _buf, _offset + HEADBYTESCOUNT, Length);

                    return _data;
                }
                else
                {
                    throw new NotImplementedException();
                }
            }
            set
            {
                Debug.Assert(!IsReadOnly);
                throw new NotImplementedException();
            }
        }

        public byte[] CRC
        {
            get
            {
                int offset = _offset + LENGTHBYTESCOUNT;
                int length = Length + TYPEBYTESCOUNT;
                uint crcValue = Crc32Bit.Value(_buf, offset, length);

                return crcValue.ToBytes();
            }
        }

        /// <summary>
        /// Use this for creating a png file
        /// </summary>
        public PngChunk()
        {
            IsReadOnly = false;
        }

        /// <summary>
        /// Use this for reading from an existing png file
        /// </summary>
        public PngChunk(byte[] buf, int offset)
        {
            _buf = buf;
            _offset = offset;
            IsReadOnly = true;
        }

        public override string ToString()
        {
            string rtn = string.Empty;
            string dataview = Data?.DebuggerView;
            string crc = " ";

            foreach (var b in CRC)
            {
                crc += $"{b} ";
            }

            rtn += $"{this.GetType().Name} = {{\r\n";
            rtn += $"    {nameof(Length)}: {Length},\r\n";
            rtn += $"    {nameof(Type)}: {Type.ToString()},\r\n";
            if (dataview != null)
                rtn += $"    {nameof(Data)}: {dataview},\r\n";
            rtn += $"    {nameof(CRC)}: {{{crc}}}\r\n";
            rtn += $"}}";

            return rtn;
        }

        /// <summary>
        /// Recode from Annex E's CRC-32bit algorithm using c
        /// </summary>
        private static class Crc32Bit
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
    }

}