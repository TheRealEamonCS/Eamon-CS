
// TypeExtensions.cs

// Copyright (c) 2014+ by Michael Penner.  All rights reserved.

using System;
using System.Collections.Generic;
using System.Linq;

namespace Eamon.ThirdParty
{
	public static class TypeExtensions
	{
		public static IEnumerable<Type> GetInterfaces(this Type type, bool includeInherited)		// note: this needs thorough testing
		{
			if (includeInherited || type.BaseType == null)
			{
				return type.GetInterfaces();
			}
			else
			{
				var interfaces = type.GetInterfaces();

				return interfaces.Except(interfaces.SelectMany(t => t.GetInterfaces()));

				// return type.GetInterfaces().Except(type.BaseType.GetInterfaces());
			}
		}

		public static bool IsSameOrSubclassOf(this Type potentialDescendant, Type potentialBase)
		{
			return potentialDescendant.IsSubclassOf(potentialBase) || potentialDescendant == potentialBase;
		}
	}
}
