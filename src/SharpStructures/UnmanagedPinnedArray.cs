using System;
using System.Runtime.InteropServices;

namespace SharpStructures
{
    /// <summary>
    /// Array in unmanaged memory.
    /// </summary>
    /// <typeparam name="T">Structure type.</typeparam>
    public class UnmanagedPinnedArray<T> : IPinnedArray<T> where T : struct
    {
        private static readonly int TypeSize = Marshal.SizeOf(typeof(T));

        /// <summary>
        /// true if the array has been disposed; otherwise, false.
        /// </summary>
        public bool IsDisposed { get; private set; }

        /// <summary>
        /// Total number of elements in the array.
        /// </summary>
        public int Length { get; }

        /// <summary>
        /// Pointer to the beginning of the array.
        /// </summary>
        public IntPtr ArrayPointer { get; }

        /// <summary>
        /// true if the instance frees the memory; otherwise, false.
        /// </summary>
        public bool OwnsHandle { get; }

        /// <summary>
        /// Gets or sets the element at the specified index.
        /// </summary>
        /// <param name="index">The zero-based index of the element to get or set.</param>
        /// <returns>The element at the specified index.</returns>
        public T this[int index]
        {
            get
            {
                if (IsDisposed)
                    throw new ObjectDisposedException(nameof(UnmanagedPinnedArray<T>));

                return (T) Marshal.PtrToStructure(IntPtr.Add(ArrayPointer, index*TypeSize), typeof(T));
            }
            set
            {
                if (IsDisposed)
                    throw new ObjectDisposedException(nameof(UnmanagedPinnedArray<T>));

                Marshal.StructureToPtr(value, IntPtr.Add(ArrayPointer, index*TypeSize), false);
            }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="T:SharpStructures.UnmanagedPinnedArray`1" /> class.
        /// </summary>
        /// <param name="length">Total number of elements in the array.</param>
        public UnmanagedPinnedArray(int length) : this(Marshal.AllocHGlobal(TypeSize*length), length, true)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="T:SharpStructures.UnmanagedPinnedArray`1" /> class.
        /// </summary>
        /// <param name="pointer">Pointer to the beginning of the array.</param>
        /// <param name="length">Total number of elements in the array.</param>
        /// <param name="ownsHandle">true if the instance frees the memory; otherwise, false.</param>
        public UnmanagedPinnedArray(IntPtr pointer, int length, bool ownsHandle)
        {
            if (length < 0)
                throw new ArgumentOutOfRangeException(nameof(length));

            if (pointer == IntPtr.Zero)
                throw new ArgumentOutOfRangeException(nameof(pointer));

            ArrayPointer = pointer;
            Length = length;
            OwnsHandle = ownsHandle;
        }

        /// <summary>
        /// Releases all resources used by the current instance of the <see cref="T:SharpStructures.UnmanagedPinnedArray`1" /> class.
        /// </summary>
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        ~UnmanagedPinnedArray()
        {
            Dispose(false);
        }

        /// <summary>
        /// Releases the unmanaged resources used by the <see cref="T:SharpStructures.UnmanagedPinnedArray`1" /> class and optionally releases the managed resources.
        /// </summary>
        /// <param name="disposing">true to release both managed and unmanaged resources; false to release only unmanaged resources.</param>
        protected void Dispose(bool disposing)
        {
            if (IsDisposed)
                return;

            if (OwnsHandle)
                Marshal.FreeHGlobal(ArrayPointer);

            IsDisposed = true;
        }
    }
}