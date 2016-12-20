using System;

namespace SharpStructures
{
    /// <summary>
    /// Array that is not moved by the GC and has a static pointer.
    /// </summary>
    /// <typeparam name="T">Structure type.</typeparam>
    public interface IPinnedArray<T> : IDisposable where T : struct
    {
        /// <summary>
        /// true if the array has been disposed; otherwise, false.
        /// </summary>
        bool IsDisposed { get; }

        /// <summary>
        /// Total number of elements in the array.
        /// </summary>
        int Length { get; }

        /// <summary>
        /// Pointer to the beginning of the array.
        /// </summary>
        IntPtr ArrayPointer { get; }

        /// <summary>
        /// Gets or sets the element at the specified index.
        /// </summary>
        /// <param name="index">The zero-based index of the element to get or set.</param>
        /// <returns>The element at the specified index.</returns>
        T this[int index] { get; set; }
    }
}