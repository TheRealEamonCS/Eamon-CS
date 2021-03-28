
// ArrayExtensions.cs

// Copyright (c) 2014+ by Michael Penner.  All rights reserved.

using System;
using System.Diagnostics;
using System.Linq;

namespace Eamon.Game.Extensions
{
	public static class ArrayExtensions
	{
		/// <summary>
		/// Gets the indexes for elements that match the conditions defined by the specified predicate.
		/// </summary>
		/// <typeparam name="T">The type of the elements of the array.</typeparam>
		/// <param name="array">The one-dimensional, zero-based <see cref="Array"/> to search.</param>
		/// <param name="match">The <see cref="Predicate{T}"/> that defines the conditions of the elements to search for.</param>
		/// <returns>
		/// An <see cref="Array"/> containing all the indexes of elements that match the conditions defined by the
		/// specified predicate, if found; otherwise, an empty Array.
		/// </returns>
		/// <remarks>
		/// Full credit:  https://stackoverflow.com/questions/10443461/c-sharp-array-findallindexof-which-findall-indexof
		/// </remarks>
		public static int[] FindAllIndexOf<T>(this T[] array, Predicate<T> match)
		{
			Debug.Assert(array != null && match != null);

			return array.Select((value, index) => match(value) ? index : -1).Where(index => index != -1).ToArray();
		}
	}
}
