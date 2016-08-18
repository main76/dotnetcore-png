using System.IO;

namespace Masteryu.Png
{
    public static class PngSign
    {
        private const int LENGTH = 8;
        private static byte[] signature = new byte[LENGTH] 
        {
            137, 
            80, 
            78, 
            71,
            13, 
            10, 
            26, 
            10
        };

        public static int Check(FileStream fs)
        {
            byte[] rs = new byte[LENGTH];
            int count = fs.Read(rs, 0, LENGTH);

            if (count != LENGTH)
                throw new InvalidDataException("Too few bytes content.");

            for (int i = 0; i < LENGTH; i++)
            {
                if (rs[i] != signature[i])
                    throw new InvalidDataException("Not a PNG file");
            }
            
            return count;
        }
    }
}