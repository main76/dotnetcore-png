using System;
using System.Diagnostics;
using Masteryu.Extension;

namespace Masteryu.Png
{
    /// <summary>
    /// The pHYs chunk specifies the intended pixel size or aspect ratio for display of the image.
    /// Before IDAT
    /// </summary>
    public class pHYs : ChunkData
    {
        private byte[] bytes;
        internal const int LENGTH = 9;

        public enum UnitSpecifier : byte
        {
            Unknown = 0,
            Metre = 1
        }

        public int PxPerUnitX
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
                Debug.Assert(value >= 0, "PxPerUnitX can not below zero!");
                byte[] bs = ((uint)value).ToBytes();
                Array.Copy(bs, 0, bytes, 0, 4);
            }
        }

        public int PxPerUnitY
        {
            get
            {
                const int pos = 4;
                if (IsReadOnly)
                {
                    return _buf.ReadInt(pos + _offset, 4);
                }
                else
                {
                    return bytes.ReadInt(pos, 4);
                }
            }
            set
            {
                Debug.Assert(!IsReadOnly);
                Debug.Assert(value >= 0, "PxPerUnitY can not below zero!");
                byte[] bs = ((uint)value).ToBytes();
                Array.Copy(bs, 0, bytes, 4, 4);
            }
        }

        public UnitSpecifier Specifier
        {
            get
            {
                const int pos = 8;
                byte b = IsReadOnly ? _buf[pos + _offset] : bytes[pos];
                return (UnitSpecifier)b;
            }
            set
            {
                Debug.Assert(!IsReadOnly);
                bytes[8] = (byte)value;
            }
        }

        public override ChunkType ChunkType
        {
            get { return ChunkType.pHYs; }
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

        public pHYs(byte[] buf, int offset, int length) : base(buf, offset)
        {
            Debug.Assert(length == LENGTH);
        }

        public pHYs() : base()
        {
            bytes = new byte[LENGTH];
        }
    }
}