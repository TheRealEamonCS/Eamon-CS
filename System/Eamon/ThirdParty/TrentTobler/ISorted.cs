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

using System.Collections.Generic;

namespace Eamon.ThirdParty
{
	/// <summary>
	/// Represents a generic interface of an ordered collection.
	/// </summary>
	/// <typeparam name="T">The type of elements in the collection.</typeparam>
	public interface ISortedCollection<T> : ICollection<T>
    {
        /// <summary>
        /// Gets the comparer used to order items in the collection.
        /// </summary>
        IComparer<T> Comparer
        {
            get;
        }

        /// <summary>
        /// Gets indication of whether the collection allows duplicate values.
        /// </summary>
        bool AllowDuplicates
        {
            get;
        }

        /// <summary>
        /// Get all items equal to or greater than the specified value, starting with the lowest index and moving forwards.
        /// </summary>
        IEnumerable<T> WhereGreaterOrEqual( T value );

        /// <summary>
        /// Get all items less than or equal to the specified value, starting with the highest index and moving backwards.
        /// </summary>
        IEnumerable<T> WhereLessOrEqualBackwards( T value );

        /// <summary>
        /// Gets the index of the first item greater than the specified value.
        /// /// </summary>
        int FirstIndexWhereGreaterThan( T value );

        /// <summary>
        /// Gets the index of the last item less than the specified key.
        /// </summary>
        int LastIndexWhereLessThan( T value );

        /// <summary>
        /// Gets the item at the specified index.
        /// </summary>
        T At( int index );

        /// <summary>
        /// Removes the item at the specified index.
        /// </summary>
        void RemoveAt( int index );

        /// <summary>
        /// Get all items starting at the index, and moving forward.
        /// </summary>
        IEnumerable<T> ForwardFromIndex( int index );

        /// <summary>
        /// Get all items starting at the index, and moving backward.
        /// </summary>
        IEnumerable<T> BackwardFromIndex( int index );
    }

    /// <summary>
    /// Represents a generic interface of ordered key/value pairs.
    /// </summary>
    /// <typeparam name="TKey">The type of the key.</typeparam>
    /// <typeparam name="TValue">The type of the value.</typeparam>
    public interface ISortedDictionary<TKey, TValue> : IDictionary<TKey, TValue>
    {
        /// <summary>
        /// Get all items having a key equal to or greater than the specified key, starting with the lowest index and moving forwards.
        /// </summary>
        IEnumerable<KeyValuePair<TKey, TValue>> WhereGreaterOrEqual( TKey key );

        /// <summary>
        /// Get all items less than or equal to the specified value, starting with the highest index and moving backwards.
        /// </summary>
        IEnumerable<KeyValuePair<TKey, TValue>> WhereLessOrEqualBackwards( TKey keyUpperBound );

        /// <summary>
        /// Gets the sorted collection of keys.
        /// </summary>
        new ISortedCollection<TKey> Keys
        {
            get;
        }

        /// <summary>
        /// Gets the item at the specified index.
        /// </summary>
        KeyValuePair<TKey, TValue> At( int index );

        /// <summary>
        /// Removes the item at the specified index.
        /// </summary>
        void RemoveAt( int index );

        /// <summary>
        /// Sets the value at the specified index.
        /// </summary>
        /// <param name="index"></param>
        /// <param name="value"></param>
        void SetValueAt( int index, TValue value );

        /// <summary>
        /// Get all items starting at the index, and moving forward.
        /// </summary>
        IEnumerable<KeyValuePair<TKey, TValue>> ForwardFromIndex( int index );

        /// <summary>
        /// Get all items starting at the index, and moving backward.
        /// </summary>
        IEnumerable<KeyValuePair<TKey, TValue>> BackwardFromIndex( int index );
    }
}
