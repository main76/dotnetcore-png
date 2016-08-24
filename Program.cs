using System;
using System.IO;
using System.IO.Compression;
using Masteryu.Extension;
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

            using (Stream output = pi.IDATBytes.Decompress(2, pi.IDATBytes.Length - 6))
            {
                while (true)
                {
                    int value = output.ReadByte();
                    if (value == -1)
                        break;
                    Console.Write("{0} ", value);
                }
                Console.WriteLine();
            }

            return 0;
        }

        private static PngImage DebugMethod(string pngpath)
        {
            Color[,] colorField =  new Color[2, 3];
            colorField[0, 0] = new Color(0);
            colorField[0, 1] = new Color(144);
            colorField[0, 2] = new Color(255, 55, 155);
            colorField[1, 0] = new Color(66, 166, 23);
            colorField[1, 1] = new Color(30, 130, 30);
            colorField[1, 2] = new Color(255);

            PngImage pi = new PngImage(new Bitmap(colorField, 2, 3));
            pi.ExportPng("res4debug/export.png");

            pi = new PngImage("res4debug/export.png");

            // pi =  new PngImage(pngpath);
            Console.WriteLine(pi);

            return pi;
        }
    }
}
