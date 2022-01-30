
// EnumUtil.cs

// Copyright (c) 2014+ by Michael Penner.  All rights reserved.

using System;
using System.Collections.Generic;
using System.Linq;

namespace Eamon.Game.Utilities
{
	public static class EnumUtil
	{
		public static IList<T> GetValues<T>()
		{
			return Enum.GetValues(typeof(T)).Cast<T>().OrderBy(x => x).ToList();
		}

		public static IList<T> GetValues<T>(Func<T, bool> cmpFunc)
		{
			return Enum.GetValues(typeof(T)).Cast<T>().Where(x => cmpFunc(x)).OrderBy(x => x).ToList();
		}

		public static T GetFirstValue<T>()
		{
			return GetValues<T>()[0];
		}

		public static T GetFirstValue<T>(Func<T, bool> cmpFunc)
		{
			return GetValues<T>(cmpFunc)[0];
		}

		public static T GetLastValue<T>()
		{
			var values = GetValues<T>();

			return values[values.Count - 1];
		}

		public static T GetLastValue<T>(Func<T, bool> cmpFunc)
		{
			var values = GetValues<T>(cmpFunc);

			return values[values.Count - 1];
		}
	}
}
