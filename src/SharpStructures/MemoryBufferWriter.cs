using System;

namespace SharpStructures
{
    public class MemoryBufferWriter
    {
        public MemoryBuffer Buffer { get; }

        public bool WriteAsLittleEndian { get; }

        public MemoryBufferWriter(MemoryBuffer buffer) : this(buffer, BitConverter.IsLittleEndian)
        {
        }

        public MemoryBufferWriter(MemoryBuffer buffer, bool writeAsLittleEndian)
        {
            if (buffer == null)
                throw new ArgumentNullException(nameof(buffer));

            Buffer = buffer;
            WriteAsLittleEndian = writeAsLittleEndian;
        }

        public void Write(byte[] buffer)
        {
            if (buffer == null)
                throw new ArgumentNullException(nameof(buffer));

            Write(buffer, 0, buffer.Length);
        }

        public void Write(byte[] buffer, int offset, int count)
        {
            if (buffer == null)
                throw new ArgumentNullException(nameof(buffer));

            if (offset < 0)
                throw new ArgumentOutOfRangeException(nameof(offset));

            if (count < 0)
                throw new ArgumentOutOfRangeException(nameof(count));

            if (buffer.Length - offset < count)
                throw new ArgumentException();

            Buffer.PreallocateSpace(count);

            System.Buffer.BlockCopy(buffer, offset, Buffer.Array, Buffer.Position - count, count);
        }

        public void Write(ushort value) => Write(value, WriteAsLittleEndian);

        public void Write(short value) => Write(value, WriteAsLittleEndian);

        public void Write(uint value) => Write(value, WriteAsLittleEndian);

        public void Write(int value) => Write(value, WriteAsLittleEndian);

        public void Write(ulong value) => Write(value, WriteAsLittleEndian);

        public void Write(long value) => Write(value, WriteAsLittleEndian);

        public void Write(float value) => Write(value, WriteAsLittleEndian);

        public void Write(double value) => Write(value, WriteAsLittleEndian);

        public void Write(float value, bool asLittleEndian) => Write(PrimitiveConverter.ToInt32(value), asLittleEndian);

        public void Write(double value, bool asLittleEndian) => Write(PrimitiveConverter.ToInt64(value), asLittleEndian);

        public void Write(ushort value, bool asLittleEndian) => Write(PrimitiveConverter.ToInt16(value), asLittleEndian);

        public void Write(uint value, bool asLittleEndian) => Write(PrimitiveConverter.ToInt32(value), asLittleEndian);

        public void Write(ulong value, bool asLittleEndian) => Write(PrimitiveConverter.ToInt64(value), asLittleEndian);

        public void Write(sbyte value) => Write(PrimitiveConverter.ToByte(value));

        public void Write(byte value)
        {
            Buffer.PreallocateSpace(1);

            Buffer.Array[Buffer.Position - 1] = value;
        }

        public void Write(short value, bool asLittleEndian)
        {
            const int byteCount = sizeof(short);

            Buffer.PreallocateSpace(byteCount);

            for (var i = 0; i < byteCount; ++i)
                Buffer.Array[Buffer.Position - (asLittleEndian ? byteCount - i : i + 1)] =
                    unchecked((byte)(value >> (8 * i)));
        }

        public void Write(int value, bool asLittleEndian)
        {
            const int byteCount = sizeof(int);

            Buffer.PreallocateSpace(byteCount);

            for (var i = 0; i < byteCount; ++i)
                Buffer.Array[Buffer.Position - (asLittleEndian ? byteCount - i : i + 1)] =
                    unchecked((byte)(value >> (8 * i)));
        }

        public void Write(long value, bool asLittleEndian)
        {
            const int byteCount = sizeof(long);

            Buffer.PreallocateSpace(byteCount);

            for (var i = 0; i < byteCount; ++i)
                Buffer.Array[Buffer.Position - (asLittleEndian ? byteCount - i : i + 1)] =
                    unchecked((byte)(value >> (8 * i)));
        }
    }
}