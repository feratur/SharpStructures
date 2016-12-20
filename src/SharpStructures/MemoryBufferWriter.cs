using System;

namespace SharpStructures
{
    /// <summary>
    /// Provides stream writing capabilities to <see cref="T:SharpStructures.MemoryBuffer" />.
    /// </summary>
    public class MemoryBufferWriter
    {
        /// <summary>
        /// Underlying <see cref="T:SharpStructures.MemoryBuffer" /> to write to.
        /// </summary>
        public MemoryBuffer Buffer { get; }

        /// <summary>
        /// true if writing is performed in little-endian format by default; otherwise, false.
        /// </summary>
        public bool WriteAsLittleEndian { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="T:SharpStructures.MemoryBufferWriter" /> class.
        /// </summary>
        /// <param name="buffer">Underlying <see cref="T:SharpStructures.MemoryBuffer" /> to write to.</param>
        public MemoryBufferWriter(MemoryBuffer buffer) : this(buffer, BitConverter.IsLittleEndian)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="T:SharpStructures.MemoryBufferWriter" /> class.
        /// </summary>
        /// <param name="buffer">Internal <see cref="T:SharpStructures.MemoryBuffer" /> to write to.</param>
        /// <param name="writeAsLittleEndian">true to write in little-endian format by default; otherwise, false.</param>
        public MemoryBufferWriter(MemoryBuffer buffer, bool writeAsLittleEndian)
        {
            if (buffer == null)
                throw new ArgumentNullException(nameof(buffer));

            Buffer = buffer;
            WriteAsLittleEndian = writeAsLittleEndian;
        }

        /// <summary>
        /// Writes a byte array to the underlying buffer.
        /// </summary>
        /// <param name="buffer">A byte array containing the data to write.</param>
        public void Write(byte[] buffer)
        {
            if (buffer == null)
                throw new ArgumentNullException(nameof(buffer));

            Write(buffer, 0, buffer.Length);
        }

        /// <summary>
        /// Writes a byte array to the underlying buffer.
        /// </summary>
        /// <param name="buffer">A byte array containing the data to write.</param>
        /// <param name="offset">The starting point in <paramref name="buffer" /> at which to begin writing.</param>
        /// <param name="count">The number of bytes to write.</param>
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

            Buffer.AllocateSpace(count);

            System.Buffer.BlockCopy(buffer, offset, Buffer.Array, Buffer.Position - count, count);
        }

        /// <summary>
        /// Writes a two-byte unsigned integer to the current buffer and advances the buffer position by two bytes.
        /// </summary>
        /// <param name="value">The two-byte unsigned integer to write.</param>
        public void Write(ushort value) => Write(value, WriteAsLittleEndian);

        /// <summary>
        /// Writes a two-byte signed integer to the current buffer and advances the buffer position by two bytes.
        /// </summary>
        /// <param name="value">The two-byte signed integer to write.</param>
        public void Write(short value) => Write(value, WriteAsLittleEndian);

        /// <summary>
        /// Writes a four-byte unsigned integer to the current buffer and advances the buffer position by four bytes.
        /// </summary>
        /// <param name="value">The four-byte unsigned integer to write.</param>
        public void Write(uint value) => Write(value, WriteAsLittleEndian);

        /// <summary>
        /// Writes a four-byte signed integer to the current buffer and advances the buffer position by four bytes.
        /// </summary>
        /// <param name="value">The four-byte signed integer to write.</param>
        public void Write(int value) => Write(value, WriteAsLittleEndian);

        /// <summary>
        /// Writes an eight-byte unsigned integer to the current buffer and advances the buffer position by eight bytes.
        /// </summary>
        /// <param name="value">The eight-byte unsigned integer to write.</param>
        public void Write(ulong value) => Write(value, WriteAsLittleEndian);

        /// <summary>
        /// Writes an eight-byte signed integer to the current buffer and advances the buffer position by eight bytes.
        /// </summary>
        /// <param name="value">The eight-byte signed integer to write.</param>
        public void Write(long value) => Write(value, WriteAsLittleEndian);

        /// <summary>
        /// Writes a four-byte floating-point value to the current buffer and advances the buffer position by four bytes.
        /// </summary>
        /// <param name="value">The four-byte floating-point value to write.</param>
        public void Write(float value) => Write(value, WriteAsLittleEndian);

        /// <summary>
        /// Writes an eight-byte floating-point value to the current buffer and advances the buffer position by eight bytes.
        /// </summary>
        /// <param name="value">The eight-byte floating-point value to write.</param>
        public void Write(double value) => Write(value, WriteAsLittleEndian);

        /// <summary>
        /// Writes a four-byte floating-point value to the current buffer and advances the buffer position by four bytes.
        /// </summary>
        /// <param name="value">The four-byte floating-point value to write.</param>
        /// <param name="asLittleEndian">true to write in little-endian format; otherwise, false.</param>
        public void Write(float value, bool asLittleEndian) => Write(PrimitiveConverter.ToInt32(value), asLittleEndian);

        /// <summary>
        /// Writes an eight-byte floating-point value to the current buffer and advances the buffer position by eight bytes.
        /// </summary>
        /// <param name="value">The eight-byte floating-point value to write.</param>
        /// <param name="asLittleEndian">true to write in little-endian format; otherwise, false.</param>
        public void Write(double value, bool asLittleEndian) => Write(PrimitiveConverter.ToInt64(value), asLittleEndian);

        /// <summary>
        /// Writes a two-byte unsigned integer to the current buffer and advances the buffer position by two bytes.
        /// </summary>
        /// <param name="value">The two-byte unsigned integer to write.</param>
        /// <param name="asLittleEndian">true to write in little-endian format; otherwise, false.</param>
        public void Write(ushort value, bool asLittleEndian) => Write(PrimitiveConverter.ToInt16(value), asLittleEndian);

        /// <summary>
        /// Writes a four-byte unsigned integer to the current buffer and advances the buffer position by four bytes.
        /// </summary>
        /// <param name="value">The four-byte unsigned integer to write.</param>
        /// <param name="asLittleEndian">true to write in little-endian format; otherwise, false.</param>
        public void Write(uint value, bool asLittleEndian) => Write(PrimitiveConverter.ToInt32(value), asLittleEndian);

        /// <summary>
        /// Writes an eight-byte unsigned integer to the current buffer and advances the buffer position by eight bytes.
        /// </summary>
        /// <param name="value">The eight-byte unsigned integer to write.</param>
        /// <param name="asLittleEndian">true to write in little-endian format; otherwise, false.</param>
        public void Write(ulong value, bool asLittleEndian) => Write(PrimitiveConverter.ToInt64(value), asLittleEndian);

        /// <summary>
        /// Writes an signed byte to the current buffer and advances the buffer position by one byte.
        /// </summary>
        /// <param name="value">The signed byte to write.</param>
        public void Write(sbyte value) => Write(PrimitiveConverter.ToByte(value));

        /// <summary>
        /// Writes an unsigned byte to the current buffer and advances the buffer position by one byte.
        /// </summary>
        /// <param name="value">The unsigned byte to write.</param>
        public void Write(byte value)
        {
            Buffer.AllocateSpace(1);

            Buffer.Array[Buffer.Position - 1] = value;
        }

        /// <summary>
        /// Writes a two-byte signed integer to the current buffer and advances the buffer position by two bytes.
        /// </summary>
        /// <param name="value">The two-byte signed integer to write.</param>
        /// <param name="asLittleEndian">true to write in little-endian format; otherwise, false.</param>
        public void Write(short value, bool asLittleEndian)
        {
            const int byteCount = sizeof(short);

            Buffer.AllocateSpace(byteCount);

            for (var i = 0; i < byteCount; ++i)
                Buffer.Array[Buffer.Position - (asLittleEndian ? byteCount - i : i + 1)] =
                    unchecked((byte) (value >> (i << 3)));
        }

        /// <summary>
        /// Writes a four-byte signed integer to the current buffer and advances the buffer position by four bytes.
        /// </summary>
        /// <param name="value">The four-byte signed integer to write.</param>
        /// <param name="asLittleEndian">true to write in little-endian format; otherwise, false.</param>
        public void Write(int value, bool asLittleEndian)
        {
            const int byteCount = sizeof(int);

            Buffer.AllocateSpace(byteCount);

            for (var i = 0; i < byteCount; ++i)
                Buffer.Array[Buffer.Position - (asLittleEndian ? byteCount - i : i + 1)] =
                    unchecked((byte) (value >> (i << 3)));
        }

        /// <summary>
        /// Writes an eight-byte signed integer to the current buffer and advances the buffer position by eight bytes.
        /// </summary>
        /// <param name="value">The eight-byte signed integer to write.</param>
        /// <param name="asLittleEndian">true to write in little-endian format; otherwise, false.</param>
        public void Write(long value, bool asLittleEndian)
        {
            const int byteCount = sizeof(long);

            Buffer.AllocateSpace(byteCount);

            for (var i = 0; i < byteCount; ++i)
                Buffer.Array[Buffer.Position - (asLittleEndian ? byteCount - i : i + 1)] =
                    unchecked((byte) (value >> (i << 3)));
        }
    }
}