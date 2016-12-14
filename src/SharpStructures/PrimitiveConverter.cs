using System;
using System.Runtime.InteropServices;

namespace SharpStructures
{
    public static class PrimitiveConverter
    {
        public static float ToSingle(int value) => new FloatUnion { IntValue = value }.SingleValue;

        public static int ToInt32(float value) => new FloatUnion { SingleValue = value }.IntValue;

        public static long ToInt64(double value) => BitConverter.DoubleToInt64Bits(value);

        public static double ToDouble(long value) => BitConverter.Int64BitsToDouble(value);

        public static short ToInt16(ushort value) => unchecked((short)value);

        public static ushort ToUInt16(short value) => unchecked((ushort)value);

        public static int ToInt32(uint value) => unchecked((int)value);

        public static uint ToUInt32(int value) => unchecked((uint)value);

        public static long ToInt64(ulong value) => unchecked((uint)value);

        public static ulong ToUInt64(long value) => unchecked((ulong)value);

        public static sbyte ToSByte(byte value) => unchecked((sbyte)value);

        public static byte ToByte(sbyte value) => unchecked((byte)value);

        [StructLayout(LayoutKind.Explicit)]
        private struct FloatUnion
        {
            [FieldOffset(0)]
            public float SingleValue;

            [FieldOffset(0)]
            public int IntValue;
        }
    }
}