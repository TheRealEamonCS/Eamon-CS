
// NecromancerAttackActionState.cs

// Copyright (c) 2014+ by Michael Penner.  All rights reserved.

using System.Diagnostics;
using Eamon.Framework.Primitive.Enums;
using Eamon.Game.Attributes;
using Eamon.Game.Utilities;
using EamonRT.Framework.Combat;
using EamonRT.Framework.States;
using static StrongholdOfKahrDur.Game.Plugin.PluginContext;

namespace StrongholdOfKahrDur.Game.States
{
	[ClassMappings]
	public class NecromancerAttackActionState : EamonRT.Game.States.State, Framework.States.INecromancerAttackActionState
	{
		public override void Execute()
		{
			var necromancerMonster = gMDB[Globals.LoopMonsterUid];

			Debug.Assert(necromancerMonster != null);

			Debug.Assert(gCharMonster != null);

			var room = gCharMonster.GetInRoom();

			Debug.Assert(room != null);

			if (!necromancerMonster.IsInRoom(room))
			{
				goto Cleanup;
			}

			var combatSystem = Globals.CreateInstance<ICombatSystem>(x =>
			{
				x.SetNextStateFunc = s => NextState = s;

				x.DfMonster = gCharMonster;

				x.OmitArmor = true;
			});

			var rl = gEngine.RollDice(1, 7, 0);

			gEngine.PrintEffectDesc(69 + rl);

			switch(rl)
			{
				// Magical energy drain

				case 1:

					var spellValues = EnumUtil.GetValues<Spell>();

					foreach (var sv in spellValues)
					{
						if (gGameState.GetSa(sv) > 5)
						{
							gGameState.SetSa(sv, (long)(gGameState.GetSa(sv) * 0.8));

							if (gGameState.GetSa(sv) < 5)
							{
								gGameState.SetSa(sv, 5);
							}
						}
					}

					break;

				// Lightning Spell

				case 2:

					combatSystem.ExecuteCalculateDamage(1, 8);

					break;

				// Fireball Spell

				case 3:

					combatSystem.ExecuteCalculateDamage(1, 6);

					break;

				// Necrotic Spell

				case 4:

					combatSystem.ExecuteCalculateDamage(1, 10);

					break;

				// Summon fire demon (25)

				case 5:

					gEngine.SummonMonster(room, 25);

					break;

				// Summon hell hound (24)

				case 6:

					gEngine.SummonMonster(room, 24);

					break;

				// Summon demonic serpent (23)

				case 7:

					gEngine.SummonMonster(room, 23);

					break;
			}

		Cleanup:

			if (NextState == null)
			{
				NextState = Globals.CreateInstance<IMonsterAttackLoopIncrementState>();
			}

			Globals.NextState = NextState;
		}

		public NecromancerAttackActionState()
		{
			Uid = 82;

			Name = "NecromancerAttackActionState";
		}
	}
}
