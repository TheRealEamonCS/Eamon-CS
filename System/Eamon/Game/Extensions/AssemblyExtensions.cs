
// AssemblyExtensions.cs

// Copyright (c) 2014+ by Michael Penner.  All rights reserved.

using System.Diagnostics;
using System.Reflection;

namespace Eamon.Game.Extensions
{
	public static class AssemblyExtensions
	{
		public static string GetShortName(this Assembly assembly)
		{
			Debug.Assert(assembly != null);

#if PORTABLE

			var i = assembly.FullName.IndexOf(',');

			return i > 0 ? assembly.FullName.Substring(0, i) : "???";

#else

			return assembly.GetName().Name;

#endif

		}
	}
}
