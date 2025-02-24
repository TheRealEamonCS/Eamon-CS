
// EndOfRoundState.cs

// Copyright (c) 2014+ by Michael Penner.  All rights reserved.

using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Eamon.Framework;
using Eamon.Framework.Primitive.Enums;
using Eamon.Game.Attributes;
using EamonRT.Framework.Primitive.Enums;
using EamonRT.Framework.States;
using static EamonRT.Game.Plugin.Globals;

namespace EamonRT.Game.States
{
	[ClassMappings]
	public class EndOfRoundState : State, IEndOfRoundState
	{
		/// <summary></summary>
		public virtual IList<IMonster> ResetMonsterList { get; set; }

		public override void Execute()
		{
			Debug.Assert(gCharRoom != null);

			ResetMonsterList = gDatabase.MonsterTable.Records.ToList();

			foreach (var monster in ResetMonsterList)
			{
				monster.InitGroupCount = monster.CurrGroupCount;

				if (gGameState.EnhancedCombat && monster.ParryCode != ParryCode.NeverVaries && gGameState.CurrTurn % monster.ParryTurns == 0 && !monster.IsInLimbo() && !monster.IsInRoom(gCharRoom))
				{
					var odds = monster.GetInitParryResetOdds();

					var rl = gEngine.RollDice(1, 100, 0);

					if (rl <= odds)
					{
						monster.Parry = monster.InitParry;
					}
				}
			}

			ProcessEvents(EventType.AfterEndRound);

			if (GotoCleanup)
			{
				goto Cleanup;
			}

		Cleanup:

			if (NextState == null)
			{
				NextState = gEngine.InitialState != null ? gEngine.InitialState : gEngine.CreateInstance<IStartState>();

				gEngine.InitialState = null;
			}

			gEngine.NextState = NextState;
		}

		public EndOfRoundState()
		{
			Name = "EndOfRoundState";
		}
	}
}
