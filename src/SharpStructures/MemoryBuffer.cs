using System;
using System.Collections;
using System.Collections.Generic;

namespace SharpStructures
{
    /// <summary>
    /// Byte vector structure with an access to the underlying buffer.
    /// </summary>
    public class MemoryBuffer : IList<byte>
    {
        private const int MinBufferSize = 4;

        private const int MaxBufferSize = 1073741824;

        /// <summary>
        /// Underlying buffer (length is always a power of two).
        /// </summary>
        public byte[] Array { get; private set; }

        /// <summary>
        /// The number of explicitly allocated bytes (less than or equal to real buffer length).
        /// </summary>
        public int Position { get; private set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="T:SharpStructures.MemoryBuffer" /> class.
        /// </summary>
        public MemoryBuffer() : this(0)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="T:SharpStructures.MemoryBuffer" /> class.
        /// </summary>
        /// <param name="size">Minimal size of the initial buffer.</param>
        public MemoryBuffer(int size)
        {
            if (size < 0 || size > MaxBufferSize)
                throw new ArgumentOutOfRangeException(nameof(size));

            Array = new byte[GetPowerOfTwoLength(MinBufferSize, size)];
        }

        /// <summary>
        /// Sets the position.
        /// </summary>
        /// <param name="value">New position value.</param>
        public void SetPosition(int value)
        {
            if (value < 0 || value > Array.Length)
                throw new ArgumentOutOfRangeException(nameof(value));

            Position = value;
        }

        /// <summary>
        /// Ensures that the length of the buffer will be more than or equal to (Position + <paramref name="requiredSpace" />).
        /// </summary>
        /// <param name="requiredSpace">Number of bytes to advance the Position by.</param>
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

        /// <summary>
        /// Returns an enumerator that iterates through the buffer.
        /// </summary>
        /// <returns>A strongly-typed byte enumerator.</returns>
        public IEnumerator<byte> GetEnumerator()
        {
            for (var i = 0; i < Position; ++i)
                yield return Array[i];
        }

        /// <summary>
        /// Returns an enumerator that iterates through the buffer.
        /// </summary>
        /// <returns>A byte enumerator.</returns>
        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

        /// <summary>
        /// Adds an object to the end of the buffer.
        /// </summary>
        /// <param name="item">The byte to be added to the end of the buffer.</param>
        public void Add(byte item)
        {
            AllocateSpace(1);

            Array[Position - 1] = item;
        }

        /// <summary>
        /// Sets the position to zero.
        /// </summary>
        public void Clear() => Position = 0;

        /// <summary>
        /// Determines whether an element is in the buffer.
        /// </summary>
        /// <param name="item">The byte to locate in the buffer.</param>
        /// <returns>true if <paramref name="item" /> is found in the buffer; otherwise, false.</returns>
        public bool Contains(byte item) => IndexOf(item) >= 0;

        /// <summary>
        /// Copies the buffer to a compatible one-dimensional array, starting at the specified index of the target array.
        /// </summary>
        /// <param name="array">The one-dimensional <see cref="T:System.Array" /> that is the destination of the elements copied from the buffer. The <see cref="T:System.Array" /> must have zero-based indexing.</param>
        /// <param name="arrayIndex">The zero-based index in <paramref name="array" /> at which copying begins.</param>
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

        /// <summary>
        /// Removes the first occurrence of a specific byte from the buffer.
        /// </summary>
        /// <param name="item">The byte to remove from the buffer.</param>
        /// <returns>true if <paramref name="item" /> is successfully removed; otherwise, false.  This method also returns false if <paramref name="item" /> was not found in the buffer.</returns>
        public bool Remove(byte item)
        {
            var index = IndexOf(item);

            if (index < 0)
                return false;

            RemoveAt(index);

            return true;
        }

        /// <summary>
        /// Gets the number of explicitly allocated bytes.
        /// </summary>
        public int Count => Position;

        /// <summary>
        /// Gets a value indicating whether the buffer is read-only.
        /// </summary>
        public bool IsReadOnly => false;

        /// <summary>
        /// Searches for the specified byte and returns the zero-based index of the first occurrence within the entire buffer.
        /// </summary>
        /// <param name="item">The byte to locate in the buffer.</param>
        /// <returns>The zero-based index of the first occurrence of <paramref name="item" /> within the buffer, if found; otherwise, –1.</returns>
        public int IndexOf(byte item) => System.Array.IndexOf(Array, item, 0, Position);

        /// <summary>
        /// Inserts an element into the buffer at the specified index.
        /// </summary>
        /// <param name="index">The zero-based index at which <paramref name="item" /> should be inserted.</param>
        /// <param name="item">The byte to insert.</param>
        public void Insert(int index, byte item)
        {
            if (index < 0 || index > Position)
                throw new ArgumentOutOfRangeException(nameof(index));

            AllocateSpace(1);

            if (index < Position - 1)
                Buffer.BlockCopy(Array, index, Array, index + 1, Position - 1 - index);

            Array[index] = item;
        }

        /// <summary>
        /// Removes the element at the specified index of the buffer.
        /// </summary>
        /// <param name="index">The zero-based index of the element to remove.</param>
        public void RemoveAt(int index)
        {
            if (index < 0 || index >= Position)
                throw new ArgumentOutOfRangeException(nameof(index));

            --Position;

            if (index < Position)
                Buffer.BlockCopy(Array, index + 1, Array, index, Position - index);
        }

        /// <summary>
        /// Gets or sets the element at the specified index.
        /// </summary>
        /// <param name="index">The zero-based index of the element to get or set.</param>
        /// <returns>The element at the specified index.</returns>
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

        #region Private methods

        private static int GetPowerOfTwoLength(int currentLength, int requiredLength)
        {
            var length = currentLength;

            while (length < requiredLength)
                length <<= 1;

            return length;
        }

        #endregion
    }
}