
// IEngine.cs

// Copyright (c) 2014+ by Kenneth Pedersen.  All rights reserved.

using System.Text;

namespace LandOfTheMountainKing.Framework.Plugin
{
	/// <inheritdoc />
	public interface IEngine : EamonRT.Framework.Plugin.IEngine
	{
		/// <summary></summary>
		new StringBuilder Buf { get; set; }

		/// <summary></summary>
		new StringBuilder Buf01 { get; set; }
	}
}
