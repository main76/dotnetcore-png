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
        private byte data;

        public enum RenderingIntent
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
                int b = IsReadOnly ? _buf[_offset] : data;
                return (RenderingIntent)b;
            }
            set
            {
                Debug.Assert(!IsReadOnly);
                data = (byte)(int)value;
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
                // only for debug
                byte b = IsReadOnly ? _buf[_offset] : data;
                return new byte[] { b };
            }
        }

        public sRGB(byte[] buf, int offset, int length) : base(buf, offset)
        {
            Debug.Assert(length == 1);
        }

        public sRGB() : base()
        {
            
        }
    }
}