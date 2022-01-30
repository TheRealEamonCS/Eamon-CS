
// FieldNameAttribute.cs

// Copyright (c) 2014+ by Michael Penner.  All rights reserved.

using System;

namespace Eamon.Game.Attributes
{
	/// <summary>
	/// </summary>
	/// <remarks>
	/// </remarks>
	[AttributeUsage(AttributeTargets.Property, AllowMultiple = false)]
	public class FieldNameAttribute : Attribute
	{
		public long SortOrder { get; set; }

		/// <summary>
		/// </summary>
		public FieldNameAttribute(long sortOrder)
		{
			SortOrder = sortOrder;
		}
	}
}
