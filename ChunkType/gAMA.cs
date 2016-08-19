using System;
using System.Diagnostics;
using Masteryu.Extension;

namespace Masteryu.Png
{
    /// <summary>
    /// The gAMA chunk specifies the relationship between the image samples and the desired display output intensity.
    /// Before PLTE and IDAT. 
    /// </summary>
    public class gAMA : ChunkData
    {
        private byte[] bytes;
        internal const int LENGTH = 4;

        public int Gamma
        {
            get 
            {
                int rtn = IsReadOnly 
                         ? _buf.ReadInt(_offset, LENGTH)
                         : bytes.ReadInt(0, LENGTH); 
                return rtn;
            }
            set
            {
                Debug.Assert(!IsReadOnly);
                bytes = ((uint)value).ToBytes();
            }
        }

        public override ChunkType ChunkType
        {
            get { return ChunkType.gAMA; }
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

        public gAMA(byte[] buf, int offset, int length) : base(buf, offset)
        {
            Debug.Assert(length == LENGTH);
        }

        public gAMA() : base()
        {
            
        }
    }
}