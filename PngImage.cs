using System.IO;
using System.Collections.Generic;
using Masteryu.Image;
using Masteryu.Extension;

namespace Masteryu.Png
{
    /// <summary>
    /// class to describe a PNG file
    /// </sumary>
    public sealed class PngImage
    {
        private readonly byte[] chunkBytes;
        private readonly List<PngChunk> chunks = new List<PngChunk>();

        public PngImage(string path)
        {
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

        public PngImage(Bitmap bm)
        {
            byte[] compressed;
            using (Stream s = bm.GetStream())
                compressed = s.Compress();

            PngChunk ihdr = new PngChunk(new IHDR 
            { 
                Width = 2, 
                Height = 3,
                BitDepth = 8,
                ColorType = 6
            });
            PngChunk srgb = new PngChunk(new sRGB());
            PngChunk gama = new PngChunk(new gAMA(45455));
            PngChunk phys = new PngChunk(new pHYs
            {
                PxPerUnitX = 3780,
                PxPerUnitY = 3780
            });
            PngChunk idat = new PngChunk(new IDAT(compressed));
            PngChunk iend = new PngChunk(new IEND());

            chunks.Add(ihdr);
            chunks.Add(srgb);
            chunks.Add(gama);
            chunks.Add(phys);
            chunks.Add(idat);
            chunks.Add(iend);
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

        // Only use for debug
        public byte[] IDATBytes
        {
            get 
            {
                foreach (var chunk in chunks)
                {
                    if (chunk.Type == ChunkType.IDAT)
                        return chunk.Data.Bytes;
                }
                throw new InvalidDataException();
            }
        }
        public void ExportPng(string path)
        {
            using (FileStream fs = File.Create(path))
            {
                PngSign.Sign(fs);
                
                foreach (var chunk in chunks)
                    chunk.WriteToStream(fs);
            }
        }
    }
}
