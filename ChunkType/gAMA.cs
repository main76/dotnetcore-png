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
        internal const int LENGTH = 4;

        public int Gamma
        {
            get { return _buf.ReadInt(_offset, LENGTH); }
            set
            {
                Debug.Assert(!IsReadOnly);
                byte[] bytes = ((uint)value).ToBytes();
                Array.Copy(bytes, 0, _buf, _offset, LENGTH);
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
                    byte[] bytes = new byte[LENGTH];
                    Array.Copy(_buf, _offset, bytes, 0, LENGTH);
                    
                    return bytes;
                }
                
                return _buf;
            }
        }

        public gAMA(byte[] buf, int offset, int length) : base(buf, offset)
        {
            Debug.Assert(length == LENGTH);
        }

        public gAMA(int gamma) : this()
        {
            Gamma = gamma;
        }

        public gAMA() : base(LENGTH)
        {
            
        }
    }
}