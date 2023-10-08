
// IRoom.cs

// Copyright (c) 2014+ by Michael Penner.  All rights reserved.

using static TheWayfarersInn.Game.Plugin.Globals;

namespace TheWayfarersInn.Framework
{
	public interface IRoom : Eamon.Framework.IRoom
	{
		/// <summary></summary>
		long AboveRoomUid { get; set; }

		/// <summary></summary>
		long BelowRoomUid { get; set; }

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
