//Copyright 2011 Trent Tobler. All rights reserved.

//Redistribution and use in source and binary forms, with or without modification, are
//permitted provided that the following conditions are met:

//   1. Redistributions of source code must retain the above copyright notice, this list of
//      conditions and the following disclaimer.

//   2. Redistributions in binary form must reproduce the above copyright notice, this list
//      of conditions and the following disclaimer in the documentation and/or other materials
//      provided with the distribution.

//THIS SOFTWARE IS PROVIDED BY TRENT TOBLER ''AS IS'' AND ANY EXPRESS OR IMPLIED
//WARRANTIES, INCLUDING, BUT NOT LIMITED TO, THE IMPLIED WARRANTIES OF MERCHANTABILITY AND
//FITNESS FOR A PARTICULAR PURPOSE ARE DISCLAIMED. IN NO EVENT SHALL TRENT TOBLER OR
//CONTRIBUTORS BE LIABLE FOR ANY DIRECT, INDIRECT, INCIDENTAL, SPECIAL, EXEMPLARY, OR
//CONSEQUENTIAL DAMAGES (INCLUDING, BUT NOT LIMITED TO, PROCUREMENT OF SUBSTITUTE GOODS OR
//SERVICES; LOSS OF USE, DATA, OR PROFITS; OR BUSINESS INTERRUPTION) HOWEVER CAUSED AND ON
//ANY THEORY OF LIABILITY, WHETHER IN CONTRACT, STRICT LIABILITY, OR TORT (INCLUDING
//NEGLIGENCE OR OTHERWISE) ARISING IN ANY WAY OUT OF THE USE OF THIS SOFTWARE, EVEN IF
//ADVISED OF THE POSSIBILITY OF SUCH DAMAGE.

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.Contracts;

namespace Eamon.ThirdParty
{
	/// <summary>
	/// A sorted collection (set) data structure using b-trees.
	/// </summary>
	/// <typeparam name="T">The type of items in the collection.</typeparam>
	[DebuggerDisplay( "Count = {Count}" )]
    public class BTree<T> : ISortedCollection<T>
    {
        #region Fields

        Node root;
        readonly Node first;
        readonly IComparer<T> comparer;
        bool allowDuplicates = false;

        [ContractInvariantMethod]
        private void ObjectInvariant()
        {
            Contract.Invariant( root != null );
            Contract.Invariant( first != null );
            Contract.Invariant( comparer != null );
        }

		#endregion

		#region Construction

		// +++ MP BEGIN +++

		/// <summary>
		/// Initializes a new BTree instance.
		/// </summary>
		public BTree()
				: this(16)
		{

		}

		// +++ MP END +++

		/// <summary>
		/// Initializes a new BTree instance.
		/// </summary>
		/// <param name="nodeCapacity">The node capacity.</param>
		public BTree( int nodeCapacity = 128 )
            : this( Comparer<T>.Default, nodeCapacity )
        {
            Contract.Requires( nodeCapacity > 2, SR.btreeCapacityError );
        }

        /// <summary>
        /// Initializes a new BTree instance with the specified comparer.
        /// </summary>
        /// <param name="comparer"></param>
        /// <param name="nodeCapacity"></param>
        public BTree( IComparer<T> comparer, int nodeCapacity = 128 )
        {
            Contract.Requires( comparer != null, SR.nullArgumentError );
            Contract.Requires( nodeCapacity > 2, SR.btreeCapacityError );

            this.comparer = comparer;
            this.first = new Node( nodeCapacity );
            this.root = this.first;
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets the number of items in the collection.
        /// </summary>
        public int Count
        {
            get
            {
                Contract.Ensures( Contract.Result<int>() >= 0 );

                return this.root.TotalCount;
            }
        }

        /// <summary>
        /// Gets the comparer used to order items in the collection.
        /// </summary>
        public IComparer<T> Comparer
        {
            get
            {
                Contract.Ensures( Contract.Result<IComparer<T>>() != null );

                return this.comparer;
            }
        }

        /// <summary>
        /// Gets or sets indication whether this collection is readonly or mutable.
        /// </summary>
        public bool IsReadOnly
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets indication whether this collection allows duplicate values.
        /// </summary>
        public bool AllowDuplicates
        {
            get
            {
                return this.allowDuplicates;
            }
            set
            {
                Contract.Requires( !IsReadOnly, SR.immutableError );
                Contract.Requires( value == true || AllowDuplicates == false || Count == 0, SR.collectionMustBeEmptyToClearAllowDuplicates );

                this.allowDuplicates = value;
            }
        }

        #endregion

        #region Methods

        /// <summary>
        /// Gets the item at the specified index. O(log N)
        /// </summary>
        /// <param name="index">The index for the item to retrieve.</param>
        /// <returns>The value of the item at the specified index.</returns>
        public T At( int index )
        {
            Contract.Requires( index >= 0 && index < this.Count, SR.indexOutOfRangeError );

            var leaf = Node.LeafAt( root, ref index );
            return leaf.GetKey( index );
        }

        /// <summary>
        /// Gets a value indicating whether the specified value is in the collection. O(log N)
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns>True if the collection contains at item with the value; Otherwise, false.</returns>
        public bool Contains( T value )
        {
            Node leaf;
            int pos;
            return Node.Find( root, value, Comparer, 0, out leaf, out pos );
        }

        /// <summary>
        /// Adds the specified value to the collection. O(log N)
        /// </summary>
        /// <param name="value">The value to add.</param>
        public void Add( T value )
        {
            Contract.Requires( !IsReadOnly, SR.immutableError );
            
            Node leaf;
            int pos;
            var found = Node.Find( root, value, Comparer, 0, out leaf, out pos );
            if( found && !AllowDuplicates )
                throw new InvalidOperationException( SR.duplicateNotAllowedError );
            Node.Insert( value, leaf, pos, ref root );
        }

        /// <summary>
        /// Clears the collection of all elements. O(1)
        /// </summary>
        public void Clear()
        {
            Contract.Requires( !IsReadOnly, SR.immutableError );

            Node.Clear( first );
            root = first;
        }

        /// <summary>
        /// Removes the specified key value from the collection. O(log N)
        /// </summary>
        /// <param name="value">The key value to remove.</param>
        /// <returns>True if the value was added; otherwise, false.</returns>
        public bool Remove( T value )
        {
            Contract.Requires( !IsReadOnly, SR.immutableError );

            Node leaf;
            int pos;
            if( !Node.Find( root, value, Comparer, 0, out leaf, out pos ) )
                return false;

            Node.Remove( leaf, pos, ref root );
            return true;
        }

        /// <summary>
        /// Removes the item at the specified index. O(log N)
        /// </summary>
        /// <param name="index">The index from which to remove.</param>
        public void RemoveAt( int index )
        {
            Contract.Requires( index >= 0 && index < this.Count, SR.indexOutOfRangeError );
            Contract.Requires( !IsReadOnly, SR.immutableError );

            var leaf = Node.LeafAt( root, ref index );
            Node.Remove( leaf, index, ref root );
        }

        /// <summary>
        /// Gets an enumerator for the collection. O(1), move next: O(1)
        /// </summary>
        /// <returns>An enumerator for the collection.</returns>
        public IEnumerator<T> GetEnumerator()
        {
            Contract.Ensures( Contract.Result<IEnumerator<T>>() != null );

            return Node.ForwardFromIndex( first, 0 ).GetEnumerator();
        }

        /// <summary>
        /// Copies the collection into the specified array. O(N)
        /// </summary>
        /// <param name="array">The array into which to copy.</param>
        /// <param name="arrayIndex">The index at which to start copying.</param>
        public void CopyTo( T[] array, int arrayIndex )
        {
            Contract.Requires( array != null, SR.nullArgumentError );
            Contract.Requires( arrayIndex + this.Count <= array.Length, SR.indexOutOfRangeError );

            foreach( var item in this )
                array[arrayIndex++] = item;
        }

        /// <summary>
        /// Gets the index of the first item greater than the specified value. O(log N), move next: O(1)
        /// </summary>
        /// <param name="value">The value for which to find the index.</param>
        /// <returns>The index of the first item greater than the specified value, or Count if no such item exists.</returns>
        public int FirstIndexWhereGreaterThan( T value )
        {
            Contract.Ensures( Contract.Result<int>() >= 0 && Contract.Result<int>() <= this.Count );

            Node leaf;
            int pos;
            var found = Node.Find( root, value, Comparer, AllowDuplicates ? 1 : 0, out leaf, out pos );
            var result = Node.GetRootIndex( leaf, pos );
            if( found )
                ++result;
            return result;
        }

        /// <summary>
        /// Gets the index of the last item less than the specified key. O(log N), move next: O(1)
        /// </summary>
        /// <param name="value">The value for which to find the index.</param>
        /// <returns>The index of the last item less than the specified value, or -1 if no such item exists.</returns>
        public int LastIndexWhereLessThan( T value )
        {
            Contract.Ensures( Contract.Result<int>() >= -1 && Contract.Result<int>() < this.Count );

            Node leaf;
            int pos;
            var found = Node.Find( root, value, Comparer, AllowDuplicates ? -1 : 0, out leaf, out pos );
            var result = Node.GetRootIndex( leaf, pos );
            --result;
            return result;
        }

        /// <summary>
        /// Get all items equal to or greater than the specified value, starting with the lowest index and moving forwards. O(log N), move next: O(1)
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns>All items having values equal to or greater than the specified value.</returns>
        public IEnumerable<T> WhereGreaterOrEqual( T value )
        {
            Contract.Ensures( Contract.Result<IEnumerable<T>>() != null );

            Node leaf;
            int pos;
            Node.Find( root, value, Comparer, AllowDuplicates ? -1 : 0, out leaf, out pos );
            return Node.ForwardFromIndex( leaf, pos );
        }

        /// <summary>
        /// Get all items less than or equal to the specified value, starting with the highest index and moving backwards. O(log N), move next: O(1)
        /// </summary>
        /// <param name="value">The key value.</param>
        /// <returns>All items having values equal to or greater than the specified value.</returns>
        public IEnumerable<T> WhereLessOrEqualBackwards( T value )
        {
            Contract.Ensures( Contract.Result<IEnumerable<T>>() != null );

            Node leaf;
            int pos;
            var found = Node.Find( root, value, Comparer, AllowDuplicates ? 1 : 0, out leaf, out pos );
            if( !found )
                --pos;
            return Node.BackwardFromIndex( leaf, pos );
        }

        /// <summary>
        /// Get all items starting at the index, and moving forward. O(log N), move next: O(1)
        /// </summary>
        public IEnumerable<T> ForwardFromIndex( int index )
        {
            Contract.Requires( index >= 0 && index <= this.Count, SR.indexOutOfRangeError );
            Contract.Ensures( Contract.Result<IEnumerable<T>>() != null );

            var node = Node.LeafAt( root, ref index );
            return Node.ForwardFromIndex( node, index );
        }

        /// <summary>
        /// Get all items starting at the index, and moving backward. O(log N), move next: O(1)
        /// </summary>
        public IEnumerable<T> BackwardFromIndex( int index )
        {
            Contract.Requires( index >= 0 && index <= this.Count, SR.indexOutOfRangeError );
            Contract.Ensures( Contract.Result<IEnumerable<T>>() != null );

            var node = Node.LeafAt( root, ref index );
            return Node.BackwardFromIndex( node, index );
        }

        #endregion

        #region Implementation - Nested Types

        [DebuggerDisplay( "Count={nodeCount}/{totalCount}, First={keys[0]}" )]
        sealed class Node
        {
            #region Fields

            readonly T[] keys;
            readonly Node[] nodes;

            int nodeCount;
            int totalCount;

            Node parent;
            Node next;
            Node prev;

            [ContractInvariantMethod]
            private void ObjectInvariant()
            {
                // Simple BTree invariants
                Contract.Invariant( keys != null );
                Contract.Invariant( nodeCount >= 0 && nodeCount <= keys.Length );
                Contract.Invariant( nodes == null || keys.Length == nodes.Length );

                // Indexable BTree invariants
                Contract.Invariant( totalCount >= 0 );
            }

            #endregion

            #region Construction

            /// <summary>
            /// Initialize the first node in the BTree structure.
            /// </summary>
            public Node( int nodeCapacity )
                : this( nodeCapacity, true )
            {
            }

            #endregion

            #region Properties

            public int TotalCount
            {
                get
                {
                    return this.totalCount;
                }
            }

            public bool IsRoot
            {
                get
                {
                    return this.parent == null;
                }
            }

            public bool IsLeaf
            {
                get
                {
                    return nodes == null;
                }
            }

            public int NodeCount
            {
                get
                {
                    return this.nodeCount;
                }
            }

            #endregion

            #region Methods

            /// <summary>
            /// Gets the key at the specified position.
            /// </summary>
            public T GetKey( int pos )
            {
                Contract.Requires( pos >= 0 && pos < this.NodeCount );
                return this.keys[pos];
            }

            /// <summary>
            /// Get the leaf node at the specified index in the tree defined by the specified root.
            /// </summary>
            public static Node LeafAt( Node root, ref int pos )
            {
                Contract.Requires( root != null );
                Contract.Requires( root.IsRoot );
                Contract.Requires( 0 <= pos && pos < root.TotalCount );
                Contract.Ensures( Contract.Result<Node>() != null );
                Contract.Ensures( Contract.Result<Node>().IsLeaf );
                Contract.Ensures( 0 <= pos && pos < Contract.Result<Node>().NodeCount );

                int nodeIndex = 0;
                while( true )
                {
                    // If root is a leaf, then it is the result.
                    if( root.nodes == null )
                        return root;

                    // Scan thru the nodes in the root, until the total count exceeds the index.
                    var node = root.nodes[nodeIndex];
                    if( pos < node.totalCount )
                    {
                        // Found the node.  Move down one level.
                        root = node;
                        nodeIndex = 0;
                    }
                    else
                    {
                        // Move to the next node in the root, and adjust index to be
                        // relative to the first element in that node.
                        pos -= node.totalCount;
                        ++nodeIndex;
                    }
                }
            }

            /// <summary>
            /// Find the node and index in the tree defined by the specified root.
            /// </summary>
            public static bool Find( Node root, T key, IComparer<T> keyComparer, int duplicatesBias, out Node leaf, out int pos )
            {
                Contract.Requires( root != null );
                Contract.Requires( root.IsRoot );
                Contract.Ensures( Contract.ValueAtReturn<Node>( out leaf ) != null );
                Contract.Ensures( 0 <= Contract.ValueAtReturn<int>( out pos ) && Contract.ValueAtReturn<int>( out pos ) <= leaf.NodeCount );

                pos = Array.BinarySearch( root.keys, 0, root.nodeCount, key, keyComparer );
                while( root.nodes != null )
                {
                    if( pos >= 0 )
                    {
                        if( duplicatesBias != 0 )
                            MoveToDuplicatesBoundary( key, keyComparer, duplicatesBias, ref root, ref pos );

                        // Found an exact match.  Move down one level.
                        root = root.nodes[pos];
                    }
                    else
                    {
                        // No exact match.  Find greatest lower bound.
                        pos = ~pos;
                        if( pos > 0 )
                            --pos;
                        root = root.nodes[pos];
                    }
                    Contract.Assume( root != null );
                    pos = Array.BinarySearch( root.keys, 0, root.nodeCount, key, keyComparer );
                }

                leaf = root;
                if( pos < 0 )
                {
                    pos = ~pos;
                    return false;
                }

                if( duplicatesBias != 0 )
                    MoveToDuplicatesBoundary( key, keyComparer, duplicatesBias, ref leaf, ref pos );

                return true;
            }

            /// <summary>
            /// Insert a new key into the leaf node at the specified position.
            /// </summary>
            public static void Insert( T key, Node leaf, int pos, ref Node root )
            {
                // Make sure there is space for the new key.
                if( EnsureSpace( leaf, ref root ) && pos > leaf.nodeCount )
                {
                    pos -= leaf.nodeCount;
                    leaf = leaf.next;
                }

                // Insert the key.
                Array.Copy( leaf.keys, pos, leaf.keys, pos + 1, leaf.nodeCount - pos );
                leaf.keys[pos] = key;
                ++leaf.nodeCount;

                // Make sure parent keys index into this node correctly.
                EnsureParentKey( leaf, pos );

                // Update total counts.
                for( var node = leaf; node != null; node = node.parent )
                    ++node.totalCount;
            }

            /// <summary>
            /// Remove the item from the node at the specified position.
            /// </summary>
            public static bool Remove( Node leaf, int pos, ref Node root )
            {
                Contract.Requires( leaf != null );
                Contract.Requires( 0 <= pos && pos < leaf.NodeCount );
                Contract.Requires( leaf.IsLeaf );

                // Update total counts.
                for( var node = leaf; node != null; node = node.parent )
                    --node.totalCount;

                // Remove the key from the node.
                --leaf.nodeCount;
                Array.Copy( leaf.keys, pos + 1, leaf.keys, pos, leaf.nodeCount - pos );
                leaf.keys[leaf.nodeCount] = default( T );

                // Make sure parent keys index correctly into this node.
                if( leaf.nodeCount > 0 )
                    EnsureParentKey( leaf, pos );

                // Merge this node with others if it is below the node capacity threshold.
                Merge( leaf, ref root );
                return true;
            }

            /// <summary>
            /// Get all items starting at the index, and moving forward.
            /// </summary>
            public static IEnumerable<T> ForwardFromIndex( Node leaf, int pos )
            {
                Contract.Requires( leaf != null );
                Contract.Requires( leaf.IsLeaf );
                Contract.Requires( 0 <= pos && pos <= leaf.NodeCount );

                while( leaf != null )
                {
                    while( pos < leaf.nodeCount )
                    {
                        yield return leaf.GetKey( pos );
                        ++pos;
                    }
                    pos -= leaf.nodeCount;
                    leaf = leaf.next;
                }
            }

            /// <summary>
            /// Get all items starting at the index, and moving backward.
            /// </summary>
            public static IEnumerable<T> BackwardFromIndex( Node leaf, int pos )
            {
                Contract.Requires( leaf != null );
                Contract.Requires( leaf.IsLeaf );
                Contract.Requires( -1 <= pos && pos <= leaf.NodeCount );

                if( pos == -1 )
                {
                    // Handle special case to start moving in the previous node.
                    leaf = leaf.prev;
                    if( leaf != null )
                        pos = leaf.nodeCount - 1;
                    else
                        pos = 0;
                }
                else if( pos == leaf.NodeCount )
                {
                    // Handle special case to start moving in the next node.
                    if( leaf.next == null )
                        --pos;
                    else
                    {
                        leaf = leaf.next;
                        pos = 0;
                    }
                }

                // Loop thru collection, yielding each value in sequence.
                while( leaf != null )
                {
                    while( pos >= 0 )
                    {
                        yield return leaf.GetKey( pos );
                        --pos;
                    }
                    leaf = leaf.prev;
                    if( leaf != null )
                        pos += leaf.nodeCount;
                }
            }

            /// <summary>
            /// Clear all values from the specified node.
            /// </summary>
            public static void Clear( Node firstNode )
            {
                Contract.Requires( firstNode != null );

                Array.Clear( firstNode.keys, 0, firstNode.nodeCount );
                firstNode.nodeCount = 0;
                firstNode.totalCount = 0;

                firstNode.parent = null;
                firstNode.next = null;
            }

            /// <summary>
            /// Get the index relative to the root node, for the position in the specified leaf.
            /// </summary>
            public static int GetRootIndex( Node leaf, int pos )
            {
                var node = leaf;
                var rootIndex = pos;
                while( node.parent != null )
                {
                    int nodePos = Array.IndexOf( node.parent.nodes, node, 0, node.parent.nodeCount );
                    for( int i = 0; i < nodePos; ++i )
                        rootIndex += node.parent.nodes[i].totalCount;
                    node = node.parent;
                }
                return rootIndex;
            }

            #endregion

            #region Implementation

            Node( int nodeCapacity, bool leaf )
            {
                this.keys = new T[nodeCapacity];

                if( leaf )
                {
                    this.nodes = null;
                }
                else
                {
                    this.nodes = new Node[nodeCapacity];
                }

                this.nodeCount = 0;
                this.totalCount = 0;
                this.parent = null;
                this.next = null;
                this.prev = null;
            }

            /// <summary>
            /// (Assumes: key is a duplicate in node at pos) Move to the side on the range of duplicates,
            /// as indicated by the sign of duplicatesBias.
            /// </summary>
            /// <param name="key"></param>
            /// <param name="keyComparer"></param>
            /// <param name="duplicatesBias"></param>
            /// <param name="node"></param>
            /// <param name="pos"></param>
            static void MoveToDuplicatesBoundary( T key, IComparer<T> keyComparer, int duplicatesBias, ref Node node, ref int pos )
            {
                // Technically, we could adjust the binary search to perform most of this step, but duplicates
                // are usually unexpected.. algorithm is still O(log N), because scan include at most a scan thru two nodes
                // worth of keys, for each level.
                // Also, the binary search option would still need the ugliness of the special case for moving into the 
                // previous node; it would only be a little faster, on average, assuming large numbers of duplicates were common.

                if( duplicatesBias < 0 )
                {
                    // Move backward over duplicates.
                    while( pos > 0 && 0 == keyComparer.Compare( node.keys[pos - 1], key ) )
                        --pos;

                    // Special case: duplicates can span backwards into the previous node because the parent
                    // key pivot might be in the center for the duplicates.
                    if( pos == 0 && node.prev != null )
                    {
                        var prev = node.prev;
                        var prevPos = prev.NodeCount;
                        while( prevPos > 0 && 0 == keyComparer.Compare( prev.keys[prevPos - 1], key ) )
                        {
                            --prevPos;
                        }
                        if( prevPos < prev.NodeCount )
                        {
                            node = prev;
                            pos = prevPos;
                        }
                    }
                }
                else
                {
                    // Move forward over duplicates.
                    while( pos < node.NodeCount - 1 && 0 == keyComparer.Compare( node.keys[pos + 1], key ) )
                        ++pos;
                }
            }

            static bool EnsureSpace( Node node, ref Node root )
            {
                if( node.nodeCount < node.keys.Length )
                    return false;

                EnsureParent( node, ref root );
                EnsureSpace( node.parent, ref root );

                var sibling = new Node( node.keys.Length, node.nodes == null );
                sibling.next = node.next;
                sibling.prev = node;
                sibling.parent = node.parent;

                if( node.next != null )
                    node.next.prev = sibling;
                node.next = sibling;

                int pos = Array.IndexOf( node.parent.nodes, node, 0, node.parent.nodeCount );
                int siblingPos = pos + 1;

                Array.Copy( node.parent.keys, siblingPos, node.parent.keys, siblingPos + 1, node.parent.nodeCount - siblingPos );
                Array.Copy( node.parent.nodes, siblingPos, node.parent.nodes, siblingPos + 1, node.parent.nodeCount - siblingPos );
                ++node.parent.nodeCount;
                node.parent.nodes[siblingPos] = sibling;

                int half = node.nodeCount / 2;
                int halfCount = node.nodeCount - half;
                Move( node, half, sibling, 0, halfCount );
                return true;
            }

            static void Move( Node source, int sourceIndex, Node target, int targetIndex, int moveCount )
            {
                Move( source.keys, sourceIndex, source.nodeCount, target.keys, targetIndex, target.nodeCount, moveCount );

                int totalMoveCount;
                if( source.nodes == null )
                {
                    totalMoveCount = moveCount;
                }
                else
                {
                    Move( source.nodes, sourceIndex, source.nodeCount, target.nodes, targetIndex, target.nodeCount, moveCount );
                    totalMoveCount = 0;
                    for( int i = 0; i < moveCount; ++i )
                    {
                        var child = target.nodes[targetIndex + i];
                        child.parent = target;
                        totalMoveCount += child.totalCount;
                    }
                }

                source.nodeCount -= moveCount;
                target.nodeCount += moveCount;

                var sn = source;
                var tn = target;
                while( sn != null && sn != tn )
                {
                    sn.totalCount -= totalMoveCount;
                    tn.totalCount += totalMoveCount;
                    sn = sn.parent;
                    tn = tn.parent;
                }

                EnsureParentKey( source, sourceIndex );
                EnsureParentKey( target, targetIndex );
            }

            static void Move<TItem>( TItem[] source, int sourceIndex, int sourceTotal, TItem[] target, int targetIndex, int targetTotal, int count )
            {
                Array.Copy( target, targetIndex, target, targetIndex + count, targetTotal - targetIndex );
                Array.Copy( source, sourceIndex, target, targetIndex, count );
                Array.Copy( source, sourceIndex + count, source, sourceIndex, sourceTotal - sourceIndex - count );
                Array.Clear( source, sourceTotal - count, count );
            }

            static void EnsureParent( Node node, ref Node root )
            {
                if( node.parent != null )
                    return;

                var parent = new Node( node.keys.Length, false );
                parent.totalCount = node.totalCount;
                parent.nodeCount = 1;
                parent.keys[0] = node.keys[0];
                parent.nodes[0] = node;

                node.parent = parent;
                root = parent;
            }

            static void EnsureParentKey( Node node, int pos )
            {
                while( pos == 0 && node.parent != null )
                {
                    pos = Array.IndexOf( node.parent.nodes, node, 0, node.parent.nodeCount );
                    node.parent.keys[pos] = node.keys[0];
                    node = node.parent;
                }
            }

            static void Merge( Node node, ref Node root )
            {
                if( node.nodeCount == 0 )
                {
                    // Handle special case: Empty node.
                    if( node.parent == null )
                        return;

                    // Remove the node from the parent nodes.
                    int pos = Array.IndexOf( node.parent.nodes, node, 0, node.parent.nodeCount );
                    --node.parent.nodeCount;
                    Array.Copy( node.parent.keys, pos + 1, node.parent.keys, pos, node.parent.nodeCount - pos );
                    Array.Copy( node.parent.nodes, pos + 1, node.parent.nodes, pos, node.parent.nodeCount - pos );
                    node.parent.keys[node.parent.nodeCount] = default( T );
                    node.parent.nodes[node.parent.nodeCount] = null;

                    // Make sure parent (of the parent) keys link down correctly.
                    if( node.parent.nodeCount > 0 )
                        EnsureParentKey( node.parent, pos );

                    // Delete the node from the next/prev linked list.
                    if( node.prev != null )
                        node.prev.next = node.next;
                    if( node.next != null )
                        node.next.prev = node.prev;

                    // Merge the parent node.
                    Merge( node.parent, ref root );
                    return;
                }

                if( node.next == null )
                {
                    if( node.parent == null && node.nodeCount == 1 && node.nodes != null )
                    {
                        root = node.nodes[0];
                        root.parent = null;
                    }

                    return;
                }

                if( node.nodeCount >= node.keys.Length / 2 )
                    return;

                int count = node.next.nodeCount;
                if( node.nodeCount + count > node.keys.Length )
                    count -= ( node.nodeCount + count ) / 2;

                Move( node.next, 0, node, node.nodeCount, count );
                Merge( node.next, ref root );
            }

            #endregion
        }

        #endregion

        #region IEnumerable members

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return this.GetEnumerator();
        }

        #endregion
    }
}
