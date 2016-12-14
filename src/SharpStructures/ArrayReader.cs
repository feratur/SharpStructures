using System;
using System.IO;

namespace SharpStructures
{
    public class ArrayReader
    {
        public byte[] Array { get; }

        public int Length { get; }

        public bool ReadAsLittleEndian { get; }

        public int Offset { get; private set; }

        public ArrayReader(byte[] array) : this(array, BitConverter.IsLittleEndian)
        {
        }

        public ArrayReader(byte[] array, int length) : this(array, length, BitConverter.IsLittleEndian)
        {
        }

        public ArrayReader(byte[] array, bool readAsLittleEndian)
        {
            if (array == null)
                throw new ArgumentNullException(nameof(array));

            Array = array;

            Length = array.Length;

            ReadAsLittleEndian = readAsLittleEndian;
        }

        public ArrayReader(byte[] array, int length, bool readAsLittleEndian)
        {
            if (array == null)
                throw new ArgumentNullException(nameof(array));

            if (length < 0 || length > array.Length)
                throw new ArgumentOutOfRangeException(nameof(length));

            Array = array;

            Length = length;

            ReadAsLittleEndian = readAsLittleEndian;
        }

        public void SetPosition(int value)
        {
            if (value < 0 || value > Length)
                throw new ArgumentOutOfRangeException(nameof(value));

            Offset = value;
        }

        public double ReadDouble() => ReadDouble(ReadAsLittleEndian);

        public float ReadSingle() => ReadSingle(ReadAsLittleEndian);

        public ulong ReadUInt64() => ReadUInt64(ReadAsLittleEndian);

        public long ReadInt64() => ReadInt64(ReadAsLittleEndian);

        public uint ReadUInt32() => ReadUInt32(ReadAsLittleEndian);

        public int ReadInt32() => ReadInt32(ReadAsLittleEndian);

        public ushort ReadUInt16() => ReadUInt16(ReadAsLittleEndian);

        public short ReadInt16() => ReadInt16(ReadAsLittleEndian);

        public double ReadDouble(bool asLittleEndian) => PrimitiveConverter.ToDouble(ReadInt64(asLittleEndian));

        public float ReadSingle(bool asLittleEndian) => PrimitiveConverter.ToSingle(ReadInt32(asLittleEndian));

        public ulong ReadUInt64(bool asLittleEndian) => PrimitiveConverter.ToUInt64(ReadInt64(asLittleEndian));

        public uint ReadUInt32(bool asLittleEndian) => PrimitiveConverter.ToUInt32(ReadInt32(asLittleEndian));

        public ushort ReadUInt16(bool asLittleEndian) => PrimitiveConverter.ToUInt16(ReadInt16(asLittleEndian));

        public sbyte ReadSByte() => PrimitiveConverter.ToSByte(ReadByte());

        public byte ReadByte()
        {
            if (Offset + 1 > Length)
                throw new EndOfStreamException();

            var result = Array[Offset];

            ++Offset;

            return result;
        }

        public byte[] ReadBytes(int count)
        {
            if (count < 0)
                throw new ArgumentOutOfRangeException(nameof(count));

            if (Offset + count > Length)
                throw new EndOfStreamException();

            var result = new byte[count];

            Buffer.BlockCopy(Array, Offset, result, 0, count);

            Offset += count;

            return result;
        }

        public short ReadInt16(bool asLittleEndian)
        {
            const int byteCount = sizeof(short);

            if (Offset + byteCount > Length)
                throw new EndOfStreamException();

            var result = 0;

            for (var i = 0; i < byteCount; ++i)
                result |= Array[Offset + (asLittleEndian ? i : byteCount - 1 - i)] << (i << 3);

            Offset += byteCount;

            return unchecked((short) result);
        }

        public int ReadInt32(bool asLittleEndian)
        {
            const int byteCount = sizeof(int);

            if (Offset + byteCount > Length)
                throw new EndOfStreamException();

            var result = 0;

            for (var i = 0; i < byteCount; ++i)
                result |= Array[Offset + (asLittleEndian ? i : byteCount - 1 - i)] << (i << 3);

            Offset += byteCount;

            return result;
        }

        public long ReadInt64(bool asLittleEndian)
        {
            const int byteCount = sizeof(long);

            if (Offset + byteCount > Length)
                throw new EndOfStreamException();

            var result = 0L;

            for (var i = 0; i < byteCount; ++i)
                result |= (long) Array[Offset + (asLittleEndian ? i : byteCount - 1 - i)] << (i << 3);

            Offset += byteCount;

            return result;
        }
    }
}