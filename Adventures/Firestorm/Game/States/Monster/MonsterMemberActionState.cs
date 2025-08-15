
// MonsterMemberActionState.cs

// Copyright (c) 2014+ by Michael Penner.  All rights reserved.

using System.Diagnostics;
using Eamon.Framework.Primitive.Enums;
using Eamon.Game.Attributes;
using EamonRT.Framework.Commands;
using EamonRT.Framework.States;
using static Firestorm.Game.Plugin.Globals;

namespace Firestorm.Game.States
{
	[ClassMappings]
	public class MonsterMemberActionState : EamonRT.Game.States.MonsterMemberActionState, IMonsterMemberActionState
	{
		public override void MonsterMemberMiscActionCheck01()
		{
			Debug.Assert(gCharMonster != null);

			Debug.Assert(gCharRoom != null);

			base.MonsterMemberMiscActionCheck01();

			var weaponArtifact = gCharMonster.Weapon > 0 ? gADB[gCharMonster.Weapon] : null;

			var rl = gEngine.RollDice(1, 100, 0);

			// Steal player weapon

			if (LoopMonster.Reaction == Friendliness.Enemy && LoopMonster.Weapon >= -1 && LoopMonster.Weapon <= 0 && LoopMonster.Field1 == 1 && gGameState.ST <= 0 && weaponArtifact != null && rl >= 98)
			{
				rl = gEngine.RollDice(1, 3, 0);

				gEngine.PrintEffectDesc(61 + rl);

				if (rl == 3)
				{
					LoopMonster.DmgTaken = LoopMonster.Hardiness - 1;

					gEngine.PrintHealthStatus(LoopMonster, true);
				}

				gOut.EnableOutput = false;

				var dropCommand = gEngine.CreateInstance<IDropCommand>(x =>
				{
					x.ActorMonster = gCharMonster;

					x.ActorRoom = gCharRoom;

					x.Dobj = weaponArtifact;
				});

				dropCommand.Execute();

				gOut.EnableOutput = true;

				weaponArtifact.SetCarriedByMonster(LoopMonster);

				LoopMonster.Weapon = -1;

				// NextState = gEngine.CreateInstance<IMonsterLoopIncrementState>();

				GotoCleanup = true;
			}
		}
	}
}
