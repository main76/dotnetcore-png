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
        internal const int LENGTH = 9;

        public enum UnitSpecifier : byte
        {
            Unknown = 0,
            Metre = 1
        }

        public int PxPerUnitX
        {
            get { return _buf.ReadInt(_offset, 4); }
            set
            {
                Debug.Assert(!IsReadOnly);
                Debug.Assert(value >= 0, "PxPerUnitX can not below zero!");
                byte[] bs = ((uint)value).ToBytes();
                Array.Copy(bs, 0, _buf, _offset, 4);
            }
        }

        public int PxPerUnitY
        {
            get
            {
                const int pos = 4;
                return _buf.ReadInt(pos + _offset, 4);
            }
            set
            {
                Debug.Assert(!IsReadOnly);
                Debug.Assert(value >= 0, "PxPerUnitY can not below zero!");
                byte[] bs = ((uint)value).ToBytes();
                Array.Copy(bs, 0, _buf, _offset + 4, 4);
            }
        }

        public UnitSpecifier Specifier
        {
            get
            {
                const int pos = 8;
                byte b = _buf[pos + _offset];
                return (UnitSpecifier)b;
            }
            set
            {
                Debug.Assert(!IsReadOnly);
                _buf[8] = (byte)value;
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
                    byte[] bytes = new byte[LENGTH];
                    Array.Copy(_buf, _offset, bytes, 0, LENGTH);

                    return bytes;
                }
                
                return _buf;
            }
        }

        public pHYs(byte[] buf, int offset, int length) : base(buf, offset)
        {
            Debug.Assert(length == LENGTH);
        }

        public pHYs() : base(LENGTH)
        {
            
        }
    }
}