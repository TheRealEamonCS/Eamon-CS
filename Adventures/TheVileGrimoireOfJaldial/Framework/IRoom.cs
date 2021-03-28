
// IRoom.cs

// Copyright (c) 2014+ by Michael Penner.  All rights reserved.

using static TheVileGrimoireOfJaldial.Game.Plugin.PluginContext;

namespace TheVileGrimoireOfJaldial.Framework
{
	public interface IRoom : Eamon.Framework.IRoom
	{
		/// <summary></summary>
		bool Seen02 { get; }

		/// <summary></summary>
		bool IsGroundsRoom();

		/// <summary></summary>
		bool IsCryptRoom();

		/// <summary></summary>
		bool IsFenceRoom();

		/// <summary></summary>
		bool IsSwampRoom();

		/// <summary></summary>
		bool IsBodyChamberRoom();

		/// <summary></summary>
		bool IsRainyRoom();

		/// <summary></summary>
		bool IsFoggyRoom();

		/// <summary></summary>
		bool IsDimLightRoom();

		/// <summary></summary>
		bool IsDimLightRoomWithoutGlowingMonsters();

		/// <summary></summary>
		long GetWeatherIntensity();
	}
}
