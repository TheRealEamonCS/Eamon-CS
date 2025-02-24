
// ObjectExtensions.cs

// Copyright (c) 2014+ by Michael Penner.  All rights reserved.

using System;
using System.Diagnostics;

namespace Eamon.Game.Extensions
{
	public static class ObjectExtensions
	{
		public static T Cast<T>(this object obj) where T : class
		{
			Debug.Assert(obj != null);

			return obj as T;
		}

		/// <remarks>
		/// Full credit:  https://stackoverflow.com/questions/2683442/where-can-i-find-the-clamp-function-in-net
		/// </remarks>
		public static T Clamp<T>(this T val, T min, T max) where T : IComparable<T>
		{
			if (val.CompareTo(min) < 0) return min;
			else if (val.CompareTo(max) > 0) return max;
			else return val;
		}
	}
}
