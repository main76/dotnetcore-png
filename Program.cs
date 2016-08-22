using System;
using System.IO;
using System.IO.Compression;
using Masteryu.Png;

namespace ConsoleApplication
{
    public class Program
    {
        private const string DEFAULTPNG = "res4debug/Untitled.png";
        public static int Main(string[] args)
        {
            if (args.Length >= 1)
                throw new NotSupportedException("Only suports 0 or 1 argument");

            var pi = DebugMethod(args.Length == 0 ? DEFAULTPNG : args[0]);

            using (FileStream fs = new FileStream("binary", FileMode.Create))
            {                
                byte[] data = pi.IDATBytes;
                for (int i = 2; i < data.Length - 4; i++)
                {
                    fs.WriteByte(data[i]);
                }                    
            }

            using (MemoryStream output = new MemoryStream())
            using (FileStream input = new FileStream("binary", FileMode.Open))
            {
                using (DeflateStream ds = new DeflateStream(input, CompressionMode.Decompress))
                {
                    ds.CopyTo(output);
                }
                output.Position = 0;
                while (true)
                {
                    int value = output.ReadByte();
                    if (value == -1)
                        break;
                    Console.Write("{0} ", value);
                }
            }

            return 0;
        }

        private static PngImage DebugMethod(string pngpath)
        {
            PngImage pi = new PngImage(pngpath);
            // Console.WriteLine(pi);
            return pi;
        }
    }
}
