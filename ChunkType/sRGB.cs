using System;
using System.Diagnostics;

namespace Masteryu.Png
{
    /// <summary>
    /// Standard RGB colour space.
    /// Before PLTE and IDAT. 
    /// If the sRGB chunk is present, the iCCP chunk should not be present.
    /// </summary>
    public class sRGB : ChunkData
    {
        private const int LENGTH = 1;

        public enum RenderingIntent : byte
        {
            /// <summary>For images preferring good adaptation to the output device gamut at the expense of colorimetric accuracy, such as photographs.</summary>
            Perceptual = 0,

            /// <summary>For images requiring colour appearance matching (relative to the output device white point), such as logos. </summary>
            RelativeColorimetric = 1,

            /// <summary>For images preferring preservation of saturation at the expense of hue and lightness, such as charts and graphs.</summary>
            Saturation = 2,

            /// <summary>For images requiring preservation of absolute colorimetry, such as previews of images destined for a different output device (proofs).</summary>
            AbsoluteColorimetric = 3
        }

        public RenderingIntent Intent
        {
            get 
            {
                byte b = _buf[_offset];
                return (RenderingIntent)b;
            }
            set
            {
                Debug.Assert(!IsReadOnly);
                _buf[_offset] = (byte)value;
            }
        }

        public override ChunkType ChunkType
        {
            get { return ChunkType.sRGB; }
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

        public sRGB(byte[] buf, int offset, int length) : base(buf, offset)
        {
            Debug.Assert(length == LENGTH);
        }

        public sRGB() : base(LENGTH)
        {
            
        }
    }
}