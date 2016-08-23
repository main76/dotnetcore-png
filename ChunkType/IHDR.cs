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
        internal const int LENGTH = 13;

        public int Width
        {
            get { return _buf.ReadInt(_offset, 4); }
            set
            {
                Debug.Assert(!IsReadOnly);
                Debug.Assert(value >= 0, "Width can not below zero!");
                byte[] bs = ((uint)value).ToBytes();
                Array.Copy(bs, 0, _buf, 0, 4);
            }
        }

        public int Height
        {
            get
            {
                const int pos = 4;
                return _buf.ReadInt(pos + _offset, 4);
            }
            set
            {
                Debug.Assert(!IsReadOnly);
                Debug.Assert(value >= 0, "Height can not below zero!");
                byte[] bs = ((uint)value).ToBytes();
                Array.Copy(bs, 0, _buf, 4, 4);
            }
        }

        public byte BitDepth
        {
            get
            {
                const int pos = 8;
                return _buf[pos + _offset];
            }
            set
            {
                Debug.Assert(!IsReadOnly);
                _buf[8] = value;
            }
        }

        public byte ColorType
        {
            get
            {
                const int pos = 9;                
                return _buf[pos + _offset];
            }
            set
            {
                Debug.Assert(!IsReadOnly);
                _buf[9] = value;
            }
        }

        // The byte value of CompressedMethod must be 0
        public byte CompressedMethod
        {
            get
            {
                const int pos = 10; 
                byte value = _buf[pos + _offset];
                Debug.Assert(value == 0);
                return value;
            }
            set
            {
                Debug.Assert(!IsReadOnly);
                Debug.Assert(value == 0);
                _buf[10] = value;
            }
        }

        // The byte value of FilterMethod must be 0
        public byte FilterMethod
        {
            get
            {
                const int pos = 11;
                byte value = _buf[pos + _offset];
                Debug.Assert(value == 0);
                return value;
            }
            set
            {
                Debug.Assert(!IsReadOnly);
                Debug.Assert(value == 0);
                _buf[11] = value;
            }
        }

        public Interlace InterlaceMethod
        {
            get
            {
                const int pos = 12;
                byte value = _buf[pos + _offset];
                return (Interlace)value;
            }
            set
            {
                Debug.Assert(!IsReadOnly);
                _buf[12] = (byte)value;
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
                    byte[] bytes = new byte[LENGTH];
                    Array.Copy(_buf, _offset, bytes, 0, LENGTH);

                    return bytes;
                }
                
                return _buf;
            }
        }

        public IHDR(byte[] buf, int offset, int length) : base(buf, offset)
        {
            Debug.Assert(length == LENGTH);
        }

        public IHDR() : base(LENGTH)
        {
            
        }

        /// <summary>Two interlace methods are defined in this International Standard</summary>
        public enum Interlace : byte
        {
            Null = 0,
            Adam7 = 1
        }
    }
}