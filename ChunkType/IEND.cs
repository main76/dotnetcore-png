namespace Masteryu.Png
{
    /// <summary>
    /// IEND Must be the Last Chunk of a PNG File.
    /// This class should always be 0 reference.
    /// </summary>
    public class IEND : ChunkData
    {
        public override ChunkType ChunkType
        {
            get { return ChunkType.IEND; }
        }

        public override byte[] Bytes
        {
            get { return null; }
        }
    }
}