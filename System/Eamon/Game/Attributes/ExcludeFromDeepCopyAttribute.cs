
// ExcludeFromDeepCopyAttribute.cs

// Copyright (c) 2014+ by Michael Penner.  All rights reserved.

using System;

namespace Eamon.Game.Attributes
{
	/// <summary>
	/// </summary>
	/// <remarks>
	/// </remarks>
	[AttributeUsage(AttributeTargets.Property, AllowMultiple = false, Inherited = true)]
	public class ExcludeFromDeepCopyAttribute : Attribute
	{
	}
}
