using System;
using System.Runtime.InteropServices;

namespace SharpStructures
{
    /// <summary>
    /// Contains methods for unchecked conversion between common system primitives.
    /// </summary>
    public static class PrimitiveConverter
    {
        /// <summary>
        /// Convert from <see cref="T:System.Int32" /> to <see cref="T:System.Single" />.
        /// </summary>
        /// <param name="value"><see cref="T:System.Int32" /> value.</param>
        /// <returns><see cref="T:System.Single" /> value.</returns>
        public static float ToSingle(int value) => new FloatUnion {IntValue = value}.SingleValue;

        /// <summary>
        /// Convert from <see cref="T:System.Single" /> to <see cref="T:System.Int32" />.
        /// </summary>
        /// <param name="value"><see cref="T:System.Single" /> value.</param>
        /// <returns><see cref="T:System.Int32" /> value.</returns>
        public static int ToInt32(float value) => new FloatUnion {SingleValue = value}.IntValue;

        /// <summary>
        /// Convert from <see cref="T:System.Double" /> to <see cref="T:System.Int64" />.
        /// </summary>
        /// <param name="value"><see cref="T:System.Double" /> value.</param>
        /// <returns><see cref="T:System.Int64" /> value.</returns>
        public static long ToInt64(double value) => BitConverter.DoubleToInt64Bits(value);

        /// <summary>
        /// Convert from <see cref="T:System.Int64" /> to <see cref="T:System.Double" />.
        /// </summary>
        /// <param name="value"><see cref="T:System.Int64" /> value.</param>
        /// <returns><see cref="T:System.Double" /> value.</returns>
        public static double ToDouble(long value) => BitConverter.Int64BitsToDouble(value);

        /// <summary>
        /// Convert from <see cref="T:System.UInt16" /> to <see cref="T:System.Int16" />.
        /// </summary>
        /// <param name="value"><see cref="T:System.UInt16" /> value.</param>
        /// <returns><see cref="T:System.Int16" /> value.</returns>
        public static short ToInt16(ushort value) => unchecked((short) value);

        /// <summary>
        /// Convert from <see cref="T:System.Int16" /> to <see cref="T:System.UInt16" />.
        /// </summary>
        /// <param name="value"><see cref="T:System.Int16" /> value.</param>
        /// <returns><see cref="T:System.UInt16" /> value.</returns>
        public static ushort ToUInt16(short value) => unchecked((ushort) value);

        /// <summary>
        /// Convert from <see cref="T:System.UInt32" /> to <see cref="T:System.Int32" />.
        /// </summary>
        /// <param name="value"><see cref="T:System.UInt32" /> value.</param>
        /// <returns><see cref="T:System.Int32" /> value.</returns>
        public static int ToInt32(uint value) => unchecked((int) value);

        /// <summary>
        /// Convert from <see cref="T:System.Int32" /> to <see cref="T:System.UInt32" />.
        /// </summary>
        /// <param name="value"><see cref="T:System.Int32" /> value.</param>
        /// <returns><see cref="T:System.UInt32" /> value.</returns>
        public static uint ToUInt32(int value) => unchecked((uint) value);

        /// <summary>
        /// Convert from <see cref="T:System.UInt64" /> to <see cref="T:System.Int64" />.
        /// </summary>
        /// <param name="value"><see cref="T:System.UInt64" /> value.</param>
        /// <returns><see cref="T:System.Int64" /> value.</returns>
        public static long ToInt64(ulong value) => unchecked((uint) value);

        /// <summary>
        /// Convert from <see cref="T:System.Int64" /> to <see cref="T:System.UInt64" />.
        /// </summary>
        /// <param name="value"><see cref="T:System.Int64" /> value.</param>
        /// <returns><see cref="T:System.UInt64" /> value.</returns>
        public static ulong ToUInt64(long value) => unchecked((ulong) value);

        /// <summary>
        /// Convert from <see cref="T:System.Byte" /> to <see cref="T:System.SByte" />.
        /// </summary>
        /// <param name="value"><see cref="T:System.Byte" /> value.</param>
        /// <returns><see cref="T:System.SByte" /> value.</returns>
        public static sbyte ToSByte(byte value) => unchecked((sbyte) value);

        /// <summary>
        /// Convert from <see cref="T:System.SByte" /> to <see cref="T:System.Byte" />.
        /// </summary>
        /// <param name="value"><see cref="T:System.SByte" /> value.</param>
        /// <returns><see cref="T:System.Byte" /> value.</returns>
        public static byte ToByte(sbyte value) => unchecked((byte) value);

        [StructLayout(LayoutKind.Explicit)]
        private struct FloatUnion
        {
            [FieldOffset(0)] public float SingleValue;

            [FieldOffset(0)] public int IntValue;
        }
    }
}