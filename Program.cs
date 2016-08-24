using System;
using System.IO;
using System.IO.Compression;
using Masteryu.Image;
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

            using (MemoryStream output = new MemoryStream())
            using (MemoryStream input = new MemoryStream(pi.IDATBytes, 2, pi.IDATBytes.Length - 4))
            {
                using (DeflateStream ds = new DeflateStream(input, CompressionMode.Decompress))
                    ds.CopyTo(output);

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
            PngImage pi = new PngImage(new Bitmap(true));
            Console.WriteLine(pi);

            pi =  new PngImage(pngpath);
            Console.WriteLine(pi);

            return pi;
        }
    }
}
