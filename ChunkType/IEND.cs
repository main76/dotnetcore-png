namespace Masteryu.Png
{
    /// <summary>
    /// IEND Must be the Last Chunk of a PNG File.
    /// </summary>
    public class IEND : ChunkData
    {
        public override ChunkType ChunkType
        {
            get { return ChunkType.IEND; }
        }

        public override byte[] Bytes
        {
            get { return new byte[0]; }
        }

        public IEND() : base(0)
        {
            
        }
    }
}