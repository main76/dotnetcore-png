using System.Collections.Generic;
using System.IO;

namespace Masteryu.Png
{
    /// <summary>
    /// class to describe a PNG file
    /// </sumary>
    public sealed class PngImage
    {
        private readonly byte[] chunkBytes;
        private readonly List<PngChunk> chunks;

        public PngImage(string path)
        {
            chunks = new List<PngChunk>();

            // Read from file
            using (FileStream fs = File.OpenRead(path))
            {
                int offset = PngSign.Check(fs);
                int count = (int)fs.Length - offset;

                chunkBytes = new byte[count];
                fs.Read(chunkBytes, 0, count);
            }
            
            // Start decoding
            ParseChunk(0);
        }

        public PngImage()
        {

        }

        private void ParseChunk(int offset)
        {
            if (offset >= chunkBytes.Length)
                return;

            PngChunk pc = new PngChunk(chunkBytes, offset);
            chunks.Add(pc);

            int newOffset = offset 
                          + pc.Length                // Data
                          + PngChunk.HEADBYTESCOUNT  // Length & Type
                          + PngChunk.CRCBYTESCOUNT;  // CRC
                          
            ParseChunk(newOffset);
        }

        public override string ToString()
        {
            string rtn = string.Empty;

            for (int i = 0; i < chunks.Count; i++)
            {
                rtn += $"chunk {(i + 1)}:\r\n";
                rtn += chunks[i].ToString();
                rtn += "\r\n____________________\r\n";
            }

            return rtn;
        }
    }
}
