// Copyright 2011-2020 Trent Tobler.All rights reserved.
// 
// Permission is hereby granted, free of charge, to any person obtaining a copy
// of this software and associated documentation files (the "Software"), to deal
// in the Software without restriction, including without limitation the rights
// to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
// copies of the Software, and to permit persons to whom the Software is
// furnished to do so, subject to the following conditions:
// 
// The above copyright notice and this permission notice shall be included in all
// copies or substantial portions of the Software.
// 
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
// IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT.IN NO EVENT SHALL THE
// AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
// LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
// OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
// SOFTWARE.

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

		// +++ MP BEGIN +++

		/// <summary>
		/// Searches the collection for a given value and returns the equal value it finds, if any.
		/// </summary>
		bool TryGetValue( T equalValue, out T actualValue );

		// +++ MP END +++

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
}
