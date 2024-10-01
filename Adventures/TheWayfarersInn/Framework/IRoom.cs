
// IRoom.cs

// Copyright (c) 2014+ by Michael Penner.  All rights reserved.

using static TheWayfarersInn.Game.Plugin.Globals;

namespace TheWayfarersInn.Framework
{
	/// <inheritdoc />
	public interface IRoom : Eamon.Framework.IRoom
	{
		/// <summary></summary>
		bool IsForestRoom();

		/// <summary></summary>
		bool IsRiverRoom();

		/// <summary></summary>
		bool IsWayfarersInnClearingRoom();

		/// <summary></summary>
		bool IsWayfarersInnRoom();
	}
}
