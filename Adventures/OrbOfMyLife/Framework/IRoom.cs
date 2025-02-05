
// IRoom.cs

// Copyright (c) 2014+ by Michael Penner.  All rights reserved.

using static OrbOfMyLife.Game.Plugin.Globals;

namespace OrbOfMyLife.Framework
{
	/// <inheritdoc />
	public interface IRoom : Eamon.Framework.IRoom
	{
		/// <summary></summary>
		bool IsDreamDimensionRoom();
	}
}
