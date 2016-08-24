using System;
using System.IO;

namespace Masteryu.Image
{
    public struct Color
    {
        public byte Alpha
        {
            get { return _buf[3]; }
            set { _buf[3] = value; }
        }

        public byte Red
        {
            get { return _buf[0]; }
            set { _buf[0] = value; }
        }

        public byte Green
        {
            get { return _buf[1]; }
            set { _buf[1] = value; }
        }

        public byte Blue
        {
            get { return _buf[2]; }
            set { _buf[2] = value; }
        }

        public byte[] Buffer
        {
            get { return _buf; }
        }

        private readonly byte[] _buf;

        public Color(byte g, byte a = 255) : this(g, g, g, a)
        {

        }

        public Color(byte r, byte g, byte b, byte a = 255)
        {
            _buf = new byte[4];
            _buf[0] = r;
            _buf[1] = g;
            _buf[2] = b;
            _buf[3] = a;
        }
    }
    public class Bitmap
    {
        private readonly Color[,] colorField;
        private readonly int width;
        private readonly int height;

        public Bitmap(bool test)
        {
            if (test != true)
                return;

            width = 2;
            height = 3;

            colorField = new Color[width, height];
            colorField[0, 0] = new Color(0);
            colorField[0, 1] = new Color(144);
            colorField[1, 0] = new Color(66, 166, 23);
            colorField[1, 1] = new Color(30, 130, 30);
        }

        public Stream GetStream()
        {
            MemoryStream ms = new MemoryStream();
            for (int i = 0; i < width; i++)
            {
                // filter type 0
                ms.WriteByte(0);
                for (int j = 0; j < height; j++)
                {
                    foreach (var b in colorField[i, j].Buffer)
                        ms.WriteByte(b);
                }
            }
            ms.Position = 0;
            return ms;
        }
    }
}