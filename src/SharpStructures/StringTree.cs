using System;
using System.Collections;
using System.Collections.Generic;

namespace SharpStructures
{
    public class StringTree<T> : IEnumerable
    {
        private readonly List<TreeNode> _nodes = new List<TreeNode>();

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

        public T GetValueOrDefault(IEnumerable<char> characters)
        {
            T value;

            TryGetValue(characters, out value);

            return value;
        }

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