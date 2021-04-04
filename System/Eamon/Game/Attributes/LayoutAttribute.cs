
// LayoutAttribute.cs

// Copyright (c) 2014+ by Michael Penner.  All rights reserved.

using System;

namespace Eamon.Game.Attributes
{
	/// <summary>
	/// </summary>
	/// <remarks>
	/// Full credit:  https://stackoverflow.com/questions/26060441/reading-data-from-fixed-length-file-into-class-objects
	/// </remarks>
	[AttributeUsage(AttributeTargets.Field)]
	public class LayoutAttribute : Attribute
	{
		protected int _length;

		public int length
		{
			get 
			{ 
				return _length; 
			}
		}

		public LayoutAttribute(int length)
		{
			this._length = length;
		}
	}
}
