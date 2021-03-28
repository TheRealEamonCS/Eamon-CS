
// GameState.cs

// Copyright (c) 2014+ by Michael Penner.  All rights reserved.

using Eamon.Framework;
using Eamon.Game.Attributes;
using static WrenholdsSecretVigil.Game.Plugin.PluginContext;

namespace WrenholdsSecretVigil.Game
{
	[ClassMappings(typeof(IGameState))]
	public class GameState : Eamon.Game.GameState, Framework.IGameState
	{
		public virtual long MedallionCharges { get; set; }

		public virtual long SlimeBlasts { get; set; }

		public virtual bool PulledRope { get; set; }

		public virtual bool RemovedLifeOrb { get; set; }

		public virtual bool[] MonsterCurses { get; set; }

		public virtual bool GetMonsterCurses(long index)
		{
			return MonsterCurses[index];
		}

		public virtual void SetMonsterCurses(long index, bool value)
		{
			MonsterCurses[index] = value;
		}

		public GameState()
		{
			// Dial back the medallion's power ... 8-15 charges only

			MedallionCharges = gEngine.RollDice(1, 8, 7);

			MonsterCurses = new bool[6];
		}
	}
}
