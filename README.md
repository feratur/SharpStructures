# SharpStructures

A collection of very simple data structures that are lacking in .NET Framework 4.5.2 for some reason.

* **BinaryListReader** - for reading lists of bytes as a stream but with an ability to switch between Big and Little Endian.
* **MemoryBuffer** - byte vector structure (like a List<byte>) but with an access to the underlying buffer.
* **MemoryBufferWriter** - provides stream writing capabilities to MemoryBuffer.
* **PrimitiveConverter** - provides unchecked conversion methods between signed and unsigned primitive strucures.
* **ReadOnlyList<T>** - an implementation of the IReadOnlyList<T> interface.
* **StringTree<T>** - a tree structure (single character on each node) for associating strings with payloads.
