
// IGameState.cs

// Copyright (c) 2014+ by Michael Penner.  All rights reserved.

using static Firestorm.Game.Plugin.Globals;

namespace Firestorm.Framework
{
	/// <inheritdoc />
	public interface IGameState : Eamon.Framework.IGameState
	{
		long[] OS { get; set; }			// Original stats

		long[] PG { get; set; }			// Program flags

		long MP { get; set; }           // Magic Points

		long ST { get; set; }           // Stunned enemy counter

		long GH { get; set; }			// Ghost hunter

		long PZ { get; set; }			// Poisoned

		long RZ { get; set; }			// Declined fight

		long MY { get; set; }			// Mystique

		long NF { get; set; }			// Guard check

		bool MPEnabled { get; set; }

		bool DeathStory { get; set; }

		long GetOS(long index);

		void SetOS(long index, long value);

		long GetPG(long index);

		void SetPG(long index, long value);
	}
}
