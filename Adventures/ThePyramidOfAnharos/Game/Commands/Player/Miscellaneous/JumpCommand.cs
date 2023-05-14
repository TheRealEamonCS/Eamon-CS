
// JumpCommand.cs

// Copyright (c) 2014+ by Michael Penner.  All rights reserved.

using System.Diagnostics;
using Eamon.Framework.Primitive.Enums;
using Eamon.Game.Attributes;
using EamonRT.Framework.Args;
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

			var gotoCleanup = false;

			// Krell statue / Pike

			if (ActorRoom.Uid == 22 && pikeArtifact.IsCarriedByMonster(ActorMonster))
			{
				gEngine.PrintEffectDesc(22);

				goto Cleanup;
			}

			// Acid moat

			if (ActorRoom.Uid == 26 || ActorRoom.Uid == 27)
			{
				if (!pikeArtifact.IsCarriedByMonster(ActorMonster))
				{
					var injureAndDamageArgs = gEngine.CreateInstance<IInjureAndDamageArgs>(x =>
					{
						x.Room = ActorRoom;

						x.EffectUid = 20;

						x.DeadBodyRoomUid = ActorRoom.Uid == 26 ? 27 : 26;

						x.EquipmentDamageAmount = 1;

						x.InjuryMultiplier = 0.1;

						x.SetNextStateFunc = s => NextState = s;
					});
				
					gEngine.InjurePartyAndDamageEquipment(injureAndDamageArgs, ref gotoCleanup);

					if (gotoCleanup)
					{
						goto Cleanup;
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
