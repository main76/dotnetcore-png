using System;
using System.Reflection;

namespace Masteryu.Png
{
    public abstract class ChunkData
    {
        protected readonly byte[] _buf;
        protected readonly int _offset;
        public readonly bool IsReadOnly;

        public abstract ChunkType ChunkType { get; }

        public abstract byte[] Bytes { get; }

        public virtual string DebuggerView
        {
            get { return ToString(1); }
        }

        /// <summary>
        /// Use this for creating a png file
        /// </summary>
        public ChunkData()
        {
            IsReadOnly = false;
        }

        /// <summary>
        /// Use this for reading from an existing png file
        /// </summary>
        protected ChunkData(byte[] buf, int offset)
        {
            _buf = buf;
            _offset = offset;
            IsReadOnly = true;
        }

        private string ToString(int stackdepth)
        {
            const string _4spc = "    "; 
            string space = _4spc;
            Type dtype = GetType();
            var pInfos = dtype.GetProperties();
            int pl = pInfos.Length;
            string rtn = string.Empty;

            for (int i = 0; i < stackdepth - 1; i++)
            {
                space += _4spc;
            }

            rtn += "{\r\n";

            for (int i = 0; i < pl; i++)
            {
                var pInfo = pInfos[i];
                if (pInfo.Name == nameof(DebuggerView))
                    continue;
                
                object obj = pInfo.GetValue(this);
                string valueString;

                if (pInfo.PropertyType.IsArray && obj != null)
                {
                    valueString = "{ ";
                    Array arr = (Array)obj;
                    foreach (var elem in arr)
                    {
                        valueString += $"{elem.ToString()} ";
                    }
                    valueString += "}";
                }
                else valueString = obj?.ToString() ?? "null";

                rtn += $"{space}{_4spc}{pInfo.Name}: {valueString}";
                rtn += (i < pl - 1) ? ",\r\n" : "\r\n";
            }
            
            rtn += $"{_4spc}}}";

            return rtn;
        }

        public override string ToString()
        {
            return ToString(0);
        }

        public static ChunkData CreatInstance(ChunkType type, byte[] buf, int offset, int length)
        {
            switch (type)
            {
                case ChunkType.IHDR:
                    return new IHDR(buf, offset);
            }

            return new UnknownData(buf, offset, length);
        }

        /// <summary>
        /// Use this temporarily
        /// </summary>
        private class UnknownData : ChunkData
        {
            private readonly int length;
            public UnknownData(byte[] buf, int offset, int length) : base(buf, offset)
            {
                this.length = length;
            }

            public string Data
            {
                get { return "Nothing"; }
            }

            public override ChunkType ChunkType
            {
                get { return ChunkType.NULL; }
            }

            public override byte[] Bytes
            {
                get
                {
                    // only for debug
                    if (length == 0)
                        return null;
                    
                    byte[] bytes = new byte[length];
                    Array.Copy(_buf, _offset, bytes, 0, length);

                    return bytes;
                }
            }
        }
    }
}