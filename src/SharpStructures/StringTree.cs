using System;
using System.Collections;
using System.Collections.Generic;

namespace SharpStructures
{
    /// <summary>
    /// A tree structure (single character on each node) for associating strings with payloads.
    /// </summary>
    /// <typeparam name="T">The type of the payload.</typeparam>
    public class StringTree<T> : IEnumerable
    {
        #region Private members

        private readonly List<TreeNode> _nodes = new List<TreeNode>();

        #endregion

        /// <summary>
        /// Adds a string-value pair to the tree.
        /// </summary>
        /// <param name="key">Unique string key.</param>
        /// <param name="value">Payload value.</param>
        public void Add(string key, T value)
        {
            if (string.IsNullOrEmpty(key))
                throw new ArgumentException();

            var nodes = _nodes;

            for (var i = 0; i < key.Length - 1; ++i)
            {
                var character = key[i];

                var index = -1;

                for (var j = 0; j < nodes.Count; ++j)
                {
                    if (nodes[j].Character != character)
                        continue;

                    index = j;

                    break;
                }

                if (index < 0)
                {
                    var newNodes = new List<TreeNode>();

                    nodes.Add(new TreeNode
                    {
                        Character = character,
                        Children = newNodes
                    });

                    nodes = newNodes;
                }
                else if (nodes[index].Children == null)
                    throw new ArgumentException();
                else
                    nodes = nodes[index].Children;
            }

            var lastCharacter = key[key.Length - 1];

            for (var i = 0; i < nodes.Count; ++i)
            {
                if (nodes[i].Character == lastCharacter)
                    throw new ArgumentException();
            }

            nodes.Add(new TreeNode
            {
                Character = lastCharacter,
                Payload = value
            });
        }

        /// <summary>
        /// Gets the value associated with the specified key or default value if the search fails.
        /// </summary>
        /// <param name="characters">Sequence of characters to search for in the tree.</param>
        /// <returns>The value associated with the specified key or default value if the search fails.</returns>
        public T GetValueOrDefault(IEnumerable<char> characters)
        {
            T value;

            TryGetValue(characters, out value);

            return value;
        }

        /// <summary>
        /// Gets the value associated with the specified key.
        /// </summary>
        /// <param name="characters">Sequence of characters to search for in the tree.</param>
        /// <param name="value">When this method returns, contains the value associated with the specified key, if the key is found; otherwise, the default value for the type of the <paramref name="value" /> parameter. This parameter is passed uninitialized.</param>
        /// <returns>true if the tree contains an element with the specified key; otherwise, false.</returns>
        public bool TryGetValue(IEnumerable<char> characters, out T value)
        {
            var nodes = _nodes;

            foreach (var character in characters)
            {
                var index = -1;

                for (var i = 0; i < nodes.Count; ++i)
                {
                    if (nodes[i].Character != character)
                        continue;

                    index = i;

                    break;
                }

                if (index < 0)
                {
                    value = default(T);

                    return false;
                }

                if (nodes[index].Children == null)
                {
                    value = nodes[index].Payload;

                    return true;
                }

                nodes = nodes[index].Children;
            }

            value = default(T);

            return false;
        }

        /// <summary>
        /// Not implemented (needed only for collection initializer).
        /// </summary>
        /// <returns>Throws exception.</returns>
        public IEnumerator GetEnumerator()
        {
            throw new NotImplementedException();
        }

        private struct TreeNode
        {
            public char Character;
            public List<TreeNode> Children;
            public T Payload;
        }
    }
}