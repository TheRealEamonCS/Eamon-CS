﻿
// BurnDownSpeedSpellState.cs

// Copyright (c) 2014+ by Michael Penner.  All rights reserved.

using System.Diagnostics;
using Eamon.Framework.Primitive.Classes;
using Eamon.Game.Attributes;
using EamonRT.Framework.States;
using static EamonRT.Game.Plugin.Globals;

namespace EamonRT.Game.States
{
	[ClassMappings]
	public class BurnDownSpeedSpellState : State, IBurnDownSpeedSpellState
	{
		/// <summary></summary>
		public virtual IStat AgilityStat { get; set; }

		/// <summary></summary>
		public virtual long PrintSpellExpiredRoll { get; set; }

		public override void Execute()
		{
			if (gGameState.Speed <= 0 || !gEngine.ShouldPreTurnProcess)
			{
				goto Cleanup;
			}

			gGameState.Speed--;

			if (gGameState.Speed > 0)
			{
				goto Cleanup;
			}

			AgilityStat = gEngine.GetStat(Eamon.Framework.Primitive.Enums.Stat.Agility);

			Debug.Assert(AgilityStat != null);

			Debug.Assert(gCharMonster != null);

			gCharMonster.Agility /= 2;

			if (gCharMonster.Agility < AgilityStat.MinValue)
			{
				gCharMonster.Agility = AgilityStat.MinValue;
			}

			PrintSpellExpiredRoll = gEngine.RollDice(1, 100, 0);

			if (PrintSpellExpiredRoll > 80 || !gEngine.IsRulesetVersion(5))
			{
				PrintSpeedSpellExpired();
			}
			
		Cleanup:

			if (NextState == null)
			{
				NextState = gEngine.CreateInstance<IRegenerateSpellAbilitiesState>();
			}

			gEngine.NextState = NextState;
		}

		public BurnDownSpeedSpellState()
		{
			Name = "BurnDownSpeedSpellState";
		}
	}
}
