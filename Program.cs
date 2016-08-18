using System;
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
            
            DebugMethod(args.Length == 0 ? DEFAULTPNG : args[0]);

            return 0;
        }

        private static void DebugMethod(string pngpath)
        {
            PngImage pi = new PngImage(pngpath);
            Console.WriteLine(pi);
        }
    }
}
