
// IEnumerableExtensions.cs

// Copyright (c) 2014+ by Michael Penner.  All rights reserved.

using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace Eamon.Game.Extensions
{
	public static class IEnumerableExtensions
	{
		public static void ForEach<T>(this IEnumerable<T> source, Action<T> action, bool throwOnError = true)
		{
			Debug.Assert(source != null);

			Debug.Assert(action != null);

			if (source is IList<T> list)
			{
				for (int i = 0; i < list.Count; i++)
				{
					try
					{
						action(list[i]);
					}
					catch (Exception)
					{
						if (throwOnError)
						{
							throw;
						}
					}
				}
			}
			else
			{
				foreach (var item in source)
				{
					try
					{
						action(item);
					}
					catch (Exception)
					{
						if (throwOnError)
						{
							throw;
						}
					}
				}
			}
		}

		public static void ForEach<T>(this IEnumerable<T> source, Action<T, int> action, bool throwOnError = true)
		{
			Debug.Assert(source != null);

			Debug.Assert(action != null);

			if (source is IList<T> list)
			{
				for (int i = 0; i < list.Count; i++)
				{
					try
					{
						action(list[i], i);
					}
					catch (Exception)
					{
						if (throwOnError)
						{
							throw;
						}
					}
				}
			}
			else
			{
				int index = 0;

				foreach (var item in source)
				{
					try
					{
						action(item, index++);
					}
					catch (Exception)
					{
						if (throwOnError)
						{
							throw;
						}
					}
				}
			}
		}
	}
}
