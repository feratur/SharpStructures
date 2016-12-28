using System;
using System.Collections.Generic;
using System.IO;

namespace SharpStructures
{
    /// <summary>
    /// Reads a list of bytes in a binary stream manner.
    /// </summary>
    public class BinaryListReader
    {
        /// <summary>
        /// Underlying byte collection to read from.
        /// </summary>
        public IReadOnlyList<byte> ByteList { get; }

        /// <summary>
        /// true if reading is performed in little-endian format by default; otherwise, false.
        /// </summary>
        public bool ReadAsLittleEndian { get; }

        /// <summary>
        /// Current stream position.
        /// </summary>
        public int Offset { get; private set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="T:SharpStructures.ArrayReader" /> class.
        /// </summary>
        /// <param name="list">Underlying byte collection to read from.</param>
        public BinaryListReader(IReadOnlyList<byte> list) : this(list, BitConverter.IsLittleEndian)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="T:SharpStructures.ArrayReader" /> class.
        /// </summary>
        /// <param name="list">Underlying byte collection to read from.</param>
        /// <param name="readAsLittleEndian">true to read in little-endian format by default; otherwise, false.</param>
        public BinaryListReader(IReadOnlyList<byte> list, bool readAsLittleEndian)
        {
            if (list == null)
                throw new ArgumentNullException(nameof(list));

            ByteList = list;

            ReadAsLittleEndian = readAsLittleEndian;
        }

        /// <summary>
        /// Sets stream position.
        /// </summary>
        /// <param name="value">New stream position.</param>
        public void SetPosition(int value)
        {
            if (value < 0 || value > ByteList.Count)
                throw new ArgumentOutOfRangeException(nameof(value));

            Offset = value;
        }

        /// <summary>
        /// Reads an 8-byte floating point value from the current stream and advances the current position of the stream by eight bytes.
        /// </summary>
        /// <returns>An 8-byte floating point value read from the current stream.</returns>
        public double ReadDouble() => ReadDouble(ReadAsLittleEndian);

        /// <summary>
        /// Reads an 4-byte floating point value from the current stream and advances the current position of the stream by four bytes.
        /// </summary>
        /// <returns>An 4-byte floating point value read from the current stream.</returns>
        public float ReadSingle() => ReadSingle(ReadAsLittleEndian);

        /// <summary>
        /// Reads an 8-byte unsigned integer from the current stream and advances the position of the stream by eight bytes.
        /// </summary>
        /// <returns>An 8-byte unsigned integer read from this stream.</returns>
        public ulong ReadUInt64() => ReadUInt64(ReadAsLittleEndian);

        /// <summary>
        /// Reads an 8-byte signed integer from the current stream and advances the position of the stream by eight bytes.
        /// </summary>
        /// <returns>An 8-byte signed integer read from this stream.</returns>
        public long ReadInt64() => ReadInt64(ReadAsLittleEndian);

        /// <summary>
        /// Reads a 4-byte unsigned integer from the current stream and advances the position of the stream by four bytes.
        /// </summary>
        /// <returns>A 4-byte unsigned integer read from this stream.</returns>
        public uint ReadUInt32() => ReadUInt32(ReadAsLittleEndian);

        /// <summary>
        /// Reads a 4-byte signed integer from the current stream and advances the position of the stream by four bytes.
        /// </summary>
        /// <returns>A 4-byte signed integer read from this stream.</returns>
        public int ReadInt32() => ReadInt32(ReadAsLittleEndian);

        /// <summary>
        /// Reads a 2-byte unsigned integer from the current stream and advances the position of the stream by two bytes.
        /// </summary>
        /// <returns>A 2-byte unsigned integer read from this stream.</returns>
        public ushort ReadUInt16() => ReadUInt16(ReadAsLittleEndian);

        /// <summary>
        /// Reads a 2-byte signed integer from the current stream and advances the position of the stream by two bytes.
        /// </summary>
        /// <returns>A 2-byte signed integer read from this stream.</returns>
        public short ReadInt16() => ReadInt16(ReadAsLittleEndian);

        /// <summary>
        /// Reads an 8-byte floating point value from the current stream and advances the current position of the stream by eight bytes.
        /// </summary>
        /// <param name="asLittleEndian">true to read in little-endian format; otherwise, false.</param>
        /// <returns>An 8-byte floating point value read from the current stream.</returns>
        public double ReadDouble(bool asLittleEndian) => PrimitiveConverter.ToDouble(ReadInt64(asLittleEndian));

        /// <summary>
        /// Reads an 4-byte floating point value from the current stream and advances the current position of the stream by four bytes.
        /// </summary>
        /// <param name="asLittleEndian">true to read in little-endian format; otherwise, false.</param>
        /// <returns>An 4-byte floating point value read from the current stream.</returns>
        public float ReadSingle(bool asLittleEndian) => PrimitiveConverter.ToSingle(ReadInt32(asLittleEndian));

        /// <summary>
        /// Reads an 8-byte unsigned integer from the current stream and advances the position of the stream by eight bytes.
        /// </summary>
        /// <param name="asLittleEndian">true to read in little-endian format; otherwise, false.</param>
        /// <returns>An 8-byte unsigned integer read from this stream.</returns>
        public ulong ReadUInt64(bool asLittleEndian) => PrimitiveConverter.ToUInt64(ReadInt64(asLittleEndian));

        /// <summary>
        /// Reads a 4-byte unsigned integer from the current stream and advances the position of the stream by four bytes.
        /// </summary>
        /// <param name="asLittleEndian">true to read in little-endian format; otherwise, false.</param>
        /// <returns>A 4-byte unsigned integer read from this stream.</returns>
        public uint ReadUInt32(bool asLittleEndian) => PrimitiveConverter.ToUInt32(ReadInt32(asLittleEndian));

        /// <summary>
        /// Reads a 2-byte unsigned integer from the current stream and advances the position of the stream by two bytes.
        /// </summary>
        /// <param name="asLittleEndian">true to read in little-endian format; otherwise, false.</param>
        /// <returns>A 2-byte unsigned integer read from this stream.</returns>
        public ushort ReadUInt16(bool asLittleEndian) => PrimitiveConverter.ToUInt16(ReadInt16(asLittleEndian));

        /// <summary>
        /// Reads a signed byte from this stream and advances the current position of the stream by one byte.
        /// </summary>
        /// <returns>A signed byte read from the current stream.</returns>
        public sbyte ReadSByte() => PrimitiveConverter.ToSByte(ReadByte());

        /// <summary>
        /// Reads the next byte from the current stream and advances the current position of the stream by one byte.
        /// </summary>
        /// <returns>The next byte read from the current stream.</returns>
        public byte ReadByte()
        {
            if (Offset + 1 > ByteList.Count)
                throw new EndOfStreamException();

            var result = ByteList[Offset];

            ++Offset;

            return result;
        }

        /// <summary>
        /// Reads the specified number of bytes from the current stream into a byte array and advances the current position by that number of bytes.
        /// </summary>
        /// <param name="count">The number of bytes to read. This value must be 0 or a non-negative number or an exception will occur.</param>
        /// <returns>A byte array containing data read from the underlying stream.</returns>
        public byte[] ReadBytes(int count)
        {
            if (count < 0)
                throw new ArgumentOutOfRangeException(nameof(count));

            if (Offset + count > ByteList.Count)
                throw new EndOfStreamException();

            var result = new byte[count];

            for (var i = 0; i < count; ++i)
                result[i] = ByteList[Offset + i];

            Offset += count;

            return result;
        }

        /// <summary>
        /// Reads a 2-byte signed integer from the current stream and advances the position of the stream by two bytes.
        /// </summary>
        /// <param name="asLittleEndian">true to read in little-endian format; otherwise, false.</param>
        /// <returns>A 2-byte signed integer read from this stream.</returns>
        public short ReadInt16(bool asLittleEndian)
        {
            const int byteCount = sizeof(short);

            if (Offset + byteCount > ByteList.Count)
                throw new EndOfStreamException();

            var result = 0;

            for (var i = 0; i < byteCount; ++i)
                result |= ByteList[Offset + (asLittleEndian ? i : byteCount - 1 - i)] << (i << 3);

            Offset += byteCount;

            return unchecked((short) result);
        }

        /// <summary>
        /// Reads a 4-byte signed integer from the current stream and advances the position of the stream by four bytes.
        /// </summary>
        /// <param name="asLittleEndian">true to read in little-endian format; otherwise, false.</param>
        /// <returns>A 4-byte signed integer read from this stream.</returns>
        public int ReadInt32(bool asLittleEndian)
        {
            const int byteCount = sizeof(int);

            if (Offset + byteCount > ByteList.Count)
                throw new EndOfStreamException();

            var result = 0;

            for (var i = 0; i < byteCount; ++i)
                result |= ByteList[Offset + (asLittleEndian ? i : byteCount - 1 - i)] << (i << 3);

            Offset += byteCount;

            return result;
        }

        /// <summary>
        /// Reads an 8-byte signed integer from the current stream and advances the position of the stream by eight bytes.
        /// </summary>
        /// <param name="asLittleEndian">true to read in little-endian format; otherwise, false.</param>
        /// <returns>An 8-byte signed integer read from this stream.</returns>
        public long ReadInt64(bool asLittleEndian)
        {
            const int byteCount = sizeof(long);

            if (Offset + byteCount > ByteList.Count)
                throw new EndOfStreamException();

            var result = 0L;

            for (var i = 0; i < byteCount; ++i)
                result |= (long) ByteList[Offset + (asLittleEndian ? i : byteCount - 1 - i)] << (i << 3);

            Offset += byteCount;

            return result;
        }
    }
}