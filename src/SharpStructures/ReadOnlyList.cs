using System;
using System.Collections;
using System.Collections.Generic;

namespace SharpStructures
{
    /// <summary>
    /// Am implementation of the <see cref="T:System.Collections.Generic.IReadOnlyList`1" /> interface.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class ReadOnlyList<T> : IReadOnlyList<T>
    {
        #region Private members

        private readonly IReadOnlyList<T> _list;
        private readonly int _count = -1;

        #endregion

        /// <summary>
        /// Initializes a new instance of the <see cref="T:SharpStructures.ReadOnlyList`1" /> class.
        /// </summary>
        /// <param name="list">An underlying read-only collection.</param>
        public ReadOnlyList(IReadOnlyList<T> list)
        {
            if (list == null)
                throw new ArgumentNullException(nameof(list));

            _list = list;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="T:SharpStructures.ReadOnlyList`1" /> class.
        /// </summary>
        /// <param name="list">An underlying read-only collection.</param>
        /// <param name="count">The number of accessible elements.</param>
        public ReadOnlyList(IReadOnlyList<T> list, int count)
        {
            if (list == null)
                throw new ArgumentNullException(nameof(list));

            if (count < 0 || count > list.Count)
                throw new ArgumentOutOfRangeException(nameof(count));

            _list = list;
            _count = count;
        }

        /// <summary>
        /// Returns an enumerator that iterates through the collection.
        /// </summary>
        /// <returns>An enumerator that can be used to iterate through the collection.</returns>
        public IEnumerator<T> GetEnumerator() => _list.GetEnumerator();

        /// <summary>
        /// Returns an enumerator that iterates through the collection.
        /// </summary>
        /// <returns>An enumerator that can be used to iterate through the collection.</returns>
        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

        /// <summary>
        /// Gets the number of elements in the collection.
        /// </summary>
        public int Count => _count < 0 ? _list.Count : _count;

        /// <summary>
        /// Gets the element at the specified index in the read-only list.
        /// </summary>
        /// <param name="index">The zero-based index of the element to get.</param>
        /// <returns>The element at the specified index in the read-only list.</returns>
        public T this[int index]
        {
            get
            {
                if (index < 0 || index >= Count)
                    throw new ArgumentOutOfRangeException(nameof(index));

                return _list[index];
            }
        }
    }
}