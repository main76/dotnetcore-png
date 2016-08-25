using System;
using System.Diagnostics;
using System.IO;
using Masteryu.Extension;

namespace Masteryu.Png
{
    /// <summary>
    /// The pHYs chunk specifies the intended pixel size or aspect ratio for display of the image.
    /// Before IDAT
    /// </summary>
    public class IDAT : ChunkData
    {
        private readonly int LENGTH;

        public override ChunkType ChunkType
        {
            get { return ChunkType.IDAT; }
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

        public IDAT(byte[] buf, int offset, int length) : base(buf, offset)
        {
            LENGTH = length;
        }

        public IDAT(byte[] compressed) : base(compressed.Length + 6)
        {
            uint cs = Adler32.Value(compressed, 0, compressed.Length);
            LENGTH = compressed.Length + 6;

            // zlib header
            _buf[0] = 24;
            _buf[1] = 87;
            // compressed data
            Array.Copy(compressed, 0, _buf, _offset + 2, compressed.Length);
            // check sum
            Array.Copy(cs.ToBytes(), 0, _buf, _offset + LENGTH - 4, 4);
        }
    }
}