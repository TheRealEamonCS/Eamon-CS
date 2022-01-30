
// QueueExtensions.cs

// Copyright (c) 2014+ by Michael Penner.  All rights reserved.

using System.Collections.Generic;
using System.Diagnostics;

namespace Eamon.Game.Extensions
{
	public static class QueueExtensions
	{
		public static void AddRange<T>(this Queue<T> queue, IEnumerable<T> enu)
		{
			Debug.Assert(queue != null && enu != null);

			foreach (T obj in enu)
			{
				queue.Enqueue(obj);
			}
		}
	}
}
