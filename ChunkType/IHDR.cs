using System;
using System.Diagnostics;
using Masteryu.Extension;

namespace Masteryu.Png
{
    /// <summary>
    /// The IHDR chunk shall be the first chunk in the PNG datastream.
    /// </summary>
    public class IHDR : ChunkData
    {
        private byte[] bytes;
        internal const int LENGTH = 13;

        public int Width
        {
            get
            {
                if (IsReadOnly)
                {
                    return _buf.ReadInt(_offset, 4);
                }
                else
                {
                    return bytes.ReadInt(0, 4);
                }
            }
            set
            {
                Debug.Assert(!IsReadOnly);
                Debug.Assert(value >= 0, "Width can not below zero!");
                byte[] bs = ((uint)value).ToBytes();
                Array.Copy(bs, 0, bytes, 0, 4);
            }
        }

        public int Height
        {
            get
            {
                const int pos = 4;
                return IsReadOnly 
                     ? _buf.ReadInt(pos + _offset, 4)
                     : bytes.ReadInt(pos, 4);
            }
            set
            {
                Debug.Assert(!IsReadOnly);
                Debug.Assert(value >= 0, "Height can not below zero!");
                byte[] bs = ((uint)value).ToBytes();
                Array.Copy(bs, 0, bytes, 4, 4);
            }
        }

        public byte BitDepth
        {
            get
            {
                const int pos = 8;
                return IsReadOnly ? _buf[pos + _offset] : bytes[pos];
            }
            set
            {
                Debug.Assert(!IsReadOnly);
                bytes[8] = value;
            }
        }

        public byte ColorType
        {
            get
            {
                const int pos = 9;                
                return IsReadOnly ? _buf[pos + _offset] : bytes[pos];
            }
            set
            {
                Debug.Assert(!IsReadOnly);
                bytes[9] = value;
            }
        }

        public byte CompressedMethod
        {
            get
            {
                const int pos = 10;                
                return IsReadOnly ? _buf[pos + _offset] : bytes[pos];
            }
            set
            {
                Debug.Assert(!IsReadOnly);
                bytes[10] = value;
            }
        }

        public byte FilterMethod
        {
            get
            {
                const int pos = 11;
                return IsReadOnly ? _buf[pos + _offset] : bytes[pos];
            }
            set
            {
                Debug.Assert(!IsReadOnly);
                bytes[11] = value;
            }
        }

        public byte InterlaceMethod
        {
            get
            {
                const int pos = 12;
                return IsReadOnly ? _buf[pos + _offset] : bytes[pos];
            }
            set
            {
                Debug.Assert(!IsReadOnly);
                bytes[12] = value;
            }
        }

        public override ChunkType ChunkType
        {
            get { return ChunkType.IHDR; }
        }

        public override byte[] Bytes
        {
            get
            {
                if (IsReadOnly)
                {
                    // only for debug
                    if (bytes == null)
                    {
                        bytes = new byte[LENGTH];
                        Array.Copy(_buf, _offset, bytes, 0, LENGTH);
                    }

                    return bytes;
                }
                
                return bytes;
            }
        }

        public IHDR(byte[] buf, int offset, int length) : base(buf, offset)
        {
            Debug.Assert(length == LENGTH);
        }

        public IHDR() : base()
        {
            bytes = new byte[LENGTH];
        }
    }
}