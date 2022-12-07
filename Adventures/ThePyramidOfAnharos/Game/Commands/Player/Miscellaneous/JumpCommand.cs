
// JumpCommand.cs

// Copyright (c) 2014+ by Michael Penner.  All rights reserved.

using System;
using System.Diagnostics;
using Eamon.Framework.Primitive.Enums;
using Eamon.Game.Attributes;
using EamonRT.Framework.Components;
using EamonRT.Framework.States;
using static ThePyramidOfAnharos.Game.Plugin.Globals;

namespace ThePyramidOfAnharos.Game.Commands
{
	[ClassMappings]
	public class JumpCommand : EamonRT.Game.Commands.Command, Framework.Commands.IJumpCommand
	{
		public override void Execute()
		{
			var pikeArtifact = gADB[11];

			Debug.Assert(pikeArtifact != null);

			// Krell statue / Pike

			if (ActorRoom.Uid == 22 && pikeArtifact.IsCarriedByCharacter())
			{
				gEngine.PrintEffectDesc(22);

				goto Cleanup;
			}

			// Acid moat

			if (ActorRoom.Uid == 26 || ActorRoom.Uid == 27)
			{
				if (!pikeArtifact.IsCarriedByCharacter())
				{
					gEngine.PrintEffectDesc(20);

					var monsterList = gEngine.GetMonsterList(m => m.IsCharacterMonster(), m => !m.IsCharacterMonster() && m.Reaction == Friendliness.Friend && m.IsInRoom(ActorRoom));

					foreach (var monster in monsterList)
					{
						var dice = (long)Math.Floor(0.1 * (monster.Hardiness - monster.DmgTaken) + 1);

						var combatComponent = gEngine.CreateInstance<ICombatComponent>(x =>
						{
							x.SetNextStateFunc = s => NextState = s;

							x.ActorRoom = ActorRoom;

							x.Dobj = monster;

							x.OmitArmor = true;
						});

						combatComponent.ExecuteCalculateDamage(dice, 1);

						var deadBodyArtifact = monster.DeadBody > 0 ? gADB[monster.DeadBody] : null;

						if (deadBodyArtifact != null && !deadBodyArtifact.IsInLimbo())
						{
							deadBodyArtifact.SetInRoomUid(ActorRoom.Uid == 26 ? 27 : 26);
						}

						if (gGameState.Die > 0)
						{
							goto Cleanup;
						}
					}

					foreach (var monster in monsterList)
					{
						gEngine.DamageWeaponsAndArmor(ActorRoom, monster);
					}
				}
				else
				{
					gEngine.PrintEffectDesc(21);
				}

				gGameState.R2 = ActorRoom.Uid == 26 ? 27 : 26;

				NextState = gEngine.CreateInstance<IAfterPlayerMoveState>();

				goto Cleanup;
			}

			gOut.Print("Try something else, grasshopper.");

		Cleanup:

			if (NextState == null)
			{
				NextState = gEngine.CreateInstance<IMonsterStartState>();
			}
		}

		public JumpCommand()
		{
			SortOrder = 460;

			IsNew = true;

			Name = "JumpCommand";

			Verb = "jump";

			Type = CommandType.Miscellaneous;
		}
	}
}
