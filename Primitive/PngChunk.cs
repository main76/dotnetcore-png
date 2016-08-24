using System;
using System.Diagnostics;
using System.IO;
using Masteryu.Extension;

namespace Masteryu.Png
{
    /// <summary>
    /// Each chunk consists of three or four fields: 
    /// Length, Type, Data(does not exist when Length is 0), CRC
    /// </summary>
    public sealed class PngChunk
    {
        internal const int LENGTHBYTESCOUNT = 4;
        internal const int TYPEBYTESCOUNT = 4;
        internal const int HEADBYTESCOUNT = LENGTHBYTESCOUNT + TYPEBYTESCOUNT;
        internal const int CRCBYTESCOUNT = 4;
        
        private readonly byte[] _buf;
        private readonly int _offset;
        private ChunkData _data;
        public readonly bool IsReadOnly;

        public int Length
        {
            get { return _buf.ReadInt(_offset, LENGTHBYTESCOUNT); }
            private set 
            {
                byte[] bytes = ((uint)value).ToBytes();
                Array.Copy(bytes, 0, _buf, _offset, LENGTHBYTESCOUNT);
            }
        }

        public ChunkType Type
        {
            get
            {
                int offset = _offset + LENGTHBYTESCOUNT;
                int value = _buf.ReadInt(offset, TYPEBYTESCOUNT);

                return (ChunkType)value;
            }
            private set 
            {
                byte[] bytes = ((uint)value).ToBytes();
                Array.Copy(bytes, 0, _buf, _offset + LENGTHBYTESCOUNT, TYPEBYTESCOUNT);
            }
        }

        public ChunkData Data
        {
            get
            {
                if (Length == 0)
                    return null;

                if (IsReadOnly)
                {
                    if (_data == null)
                        _data = ChunkData.CreatInstance(Type, _buf, _offset + HEADBYTESCOUNT, Length);

                }

                return _data;
            }
            private set
            {
                Debug.Assert(!IsReadOnly);
                Debug.Assert(Type == value.ChunkType);
                if (value.ChunkType == ChunkType.IEND)
                    return;
                
                byte[] bytes = value.Bytes;
                _data = value;
                Array.Copy(bytes, 0, _buf, HEADBYTESCOUNT, bytes.Length);
            }
        }

        public byte[] CRC
        {
            get
            {
                int offset = _offset + LENGTHBYTESCOUNT;
                int length = Length + TYPEBYTESCOUNT;
                uint crcValue = Crc32Bit.Value(_buf, offset, length);
                byte[] rtn = crcValue.ToBytes();

                if (IsReadOnly)
                {
                    offset = offset + length;
                    for (int i = 0; i < rtn.Length; i++)
                    {
                        if (_buf[i + offset] != rtn[i])
                            throw new InvalidDataException();
                    }
                }

                return rtn;
            }
        }

        /// <summary>
        /// Use this for creating a png file
        /// </summary>
        public PngChunk(ChunkData data)
        {
            byte[] bytes = data?.Bytes;
            if (bytes == null)
                throw new ArgumentNullException();

            _offset = 0;
            IsReadOnly = false;
            
            // Initialize the data keeping field _buf
            int fullLength = HEADBYTESCOUNT + bytes.Length + CRCBYTESCOUNT;
            _buf = new byte[fullLength];
            // Set the Length
            Length = bytes.Length;
            // Set the Type
            Type = data.ChunkType;
            // Set the Data 
            Data = data;
        }

        /// <summary>
        /// Use this for reading from an existing png file
        /// </summary>
        public PngChunk(byte[] buf, int offset)
        {
            _buf = buf;
            _offset = offset;
            IsReadOnly = true;
        }

        public override string ToString()
        {
            string rtn = string.Empty;
            string dataview = Data?.DebuggerView;
            string crc = " ";

            foreach (var b in CRC)
            {
                crc += $"{b} ";
            }

            rtn += $"{this.GetType().Name} = {{\r\n";
            rtn += $"    {nameof(Length)}: {Length},\r\n";
            rtn += $"    {nameof(Type)}: {Type.ToString()},\r\n";
            if (dataview != null)
                rtn += $"    {nameof(Data)}: {dataview},\r\n";
            rtn += $"    {nameof(CRC)}: {{{crc}}}\r\n";
            rtn += $"}}";

            return rtn;
        }

        public void WriteToStream(Stream stream)
        {
            if (stream == null)
                throw new ArgumentNullException(nameof(stream));

            int fullLength = HEADBYTESCOUNT + CRCBYTESCOUNT + Length;
            
            // Set the CRC value into the buffer field
            Array.Copy(CRC, 0, _buf, _offset + fullLength - CRCBYTESCOUNT, 4);

            stream.Write(_buf, _offset, fullLength);
        }
    }
}