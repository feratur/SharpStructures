using System;
using System.Runtime.InteropServices;

namespace SharpStructures
{
    /// <summary>
    /// Managed array with GCHandleType.Pinned.
    /// </summary>
    /// <typeparam name="T">Structure type.</typeparam>
    public class ManagedPinnedArray<T> : IPinnedArray<T> where T : struct
    {
        #region Private members

        private readonly T[] _array;

        private GCHandle _handle;

        #endregion

        /// <summary>
        /// true if the array has been disposed; otherwise, false.
        /// </summary>
        public bool IsDisposed { get; private set; }

        /// <summary>
        /// Total number of elements in the array.
        /// </summary>
        public int Length => _array.Length;

        /// <summary>
        /// Pointer to the beginning of the array.
        /// </summary>
        public IntPtr ArrayPointer
        {
            get
            {
                if (IsDisposed)
                    throw new ObjectDisposedException(nameof(ManagedPinnedArray<T>));

                return _handle.AddrOfPinnedObject();
            }
        }

        /// <summary>
        /// Gets or sets the element at the specified index.
        /// </summary>
        /// <param name="index">The zero-based index of the element to get or set.</param>
        /// <returns>The element at the specified index.</returns>
        public T this[int index]
        {
            get { return _array[index]; }
            set { _array[index] = value; }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="T:SharpStructures.ManagedPinnedArray`1" /> class.
        /// </summary>
        /// <param name="array">Underlying array.</param>
        public ManagedPinnedArray(T[] array)
        {
            if (array == null)
                throw new ArgumentNullException(nameof(array));

            _array = array;
            _handle = GCHandle.Alloc(_array, GCHandleType.Pinned);
        }

        /// <summary>
        /// Releases all resources used by the current instance of the <see cref="T:SharpStructures.ManagedPinnedArray`1" /> class.
        /// </summary>
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        ~ManagedPinnedArray()
        {
            Dispose(false);
        }

        /// <summary>
        /// Releases the unmanaged resources used by the <see cref="T:SharpStructures.ManagedPinnedArray`1" /> class and optionally releases the managed resources.
        /// </summary>
        /// <param name="disposing">true to release both managed and unmanaged resources; false to release only unmanaged resources.</param>
        protected void Dispose(bool disposing)
        {
            if (IsDisposed)
                return;

            _handle.Free();

            IsDisposed = true;
        }
    }
}