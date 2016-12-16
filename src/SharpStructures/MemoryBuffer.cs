using System;
using System.Collections;
using System.Collections.Generic;

namespace SharpStructures
{
    public class MemoryBuffer : IList<byte>
    {
        private const int MinBufferSize = 4;

        private const int MaxBufferSize = 1073741824;

        public byte[] Array { get; private set; }

        public int Position { get; private set; }

        public MemoryBuffer() : this(MinBufferSize)
        {
        }

        public MemoryBuffer(int size)
        {
            if (size < 0 || size > MaxBufferSize)
                throw new ArgumentOutOfRangeException(nameof(size));

            Array = new byte[GetPowerOfTwoLength(MinBufferSize, size)];
        }

        public void SetPosition(int value)
        {
            if (value < 0 || value > Array.Length)
                throw new ArgumentOutOfRangeException(nameof(value));

            Position = value;
        }

        public void AllocateSpace(int requiredSpace)
        {
            if (requiredSpace < 0 || Position + requiredSpace > MaxBufferSize)
                throw new ArgumentOutOfRangeException(nameof(requiredSpace));

            if (Array.Length < Position + requiredSpace)
            {
                var newArray = new byte[GetPowerOfTwoLength(Array.Length, Position + requiredSpace)];

                Buffer.BlockCopy(Array, 0, newArray, 0, Position);

                Array = newArray;
            }

            Position += requiredSpace;
        }

        #region IList<byte> implementation

        public IEnumerator<byte> GetEnumerator()
        {
            for (var i = 0; i < Position; ++i)
                yield return Array[i];
        }

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

        public void Add(byte item)
        {
            AllocateSpace(1);

            Array[Position - 1] = item;
        }

        public void Clear() => Position = 0;

        public bool Contains(byte item) => IndexOf(item) >= 0;

        public void CopyTo(byte[] array, int arrayIndex)
        {
            if (array == null)
                throw new ArgumentNullException(nameof(array));

            if (arrayIndex < 0 || arrayIndex > array.Length)
                throw new ArgumentOutOfRangeException(nameof(arrayIndex));

            if (Position > array.Length - arrayIndex)
                throw new ArgumentException();

            Buffer.BlockCopy(Array, 0, array, arrayIndex, Position);
        }

        public bool Remove(byte item)
        {
            var index = IndexOf(item);

            if (index < 0)
                return false;

            RemoveAt(index);

            return true;
        }

        public int Count => Position;

        public bool IsReadOnly => false;

        public int IndexOf(byte item) => System.Array.IndexOf(Array, item, 0, Position);

        public void Insert(int index, byte item)
        {
            if (index < 0 || index > Position)
                throw new ArgumentOutOfRangeException(nameof(index));

            AllocateSpace(1);

            if (index < Position - 1)
                Buffer.BlockCopy(Array, index, Array, index + 1, Position - 1 - index);

            Array[index] = item;
        }

        public void RemoveAt(int index)
        {
            if (index < 0 || index >= Position)
                throw new ArgumentOutOfRangeException(nameof(index));

            --Position;

            if (index < Position)
                Buffer.BlockCopy(Array, index + 1, Array, index, Position - index);
        }

        public byte this[int index]
        {
            get
            {
                if (index < 0 || index >= Position)
                    throw new ArgumentOutOfRangeException(nameof(index));

                return Array[index];
            }
            set
            {
                if (index < 0 || index >= Position)
                    throw new ArgumentOutOfRangeException(nameof(index));

                Array[index] = value;
            }
        }

        #endregion

        private static int GetPowerOfTwoLength(int currentLength, int requiredLength)
        {
            var length = currentLength;

            while (length < requiredLength)
                length <<= 1;

            return length;
        }
    }
}