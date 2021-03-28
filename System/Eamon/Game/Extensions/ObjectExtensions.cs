
// ObjectExtensions.cs

// Copyright (c) 2014+ by Michael Penner.  All rights reserved.

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
	}
}
