
// MonsterAttackActionState.cs

// Copyright (c) 2014+ by Michael Penner.  All rights reserved.

using System.Diagnostics;
using Eamon.Game.Attributes;
using EamonRT.Framework.States;
using static StrongholdOfKahrDur.Game.Plugin.PluginContext;

namespace StrongholdOfKahrDur.Game.States
{
	[ClassMappings]
	public class MonsterAttackActionState : EamonRT.Game.States.MonsterAttackActionState, IMonsterAttackActionState
	{
		public override void Execute()
		{
			var monster = gMDB[Globals.LoopMonsterUid];

			Debug.Assert(monster != null);

			// Necromancer has special attack routine. Casts spells, never misses, always attacks player.

			if (monster.Uid == 22)
			{
				NextState = Globals.CreateInstance<Framework.States.INecromancerAttackActionState>();

				Globals.NextState = NextState;
			}
			else
			{
				base.Execute();
			}
		}
	}
}
