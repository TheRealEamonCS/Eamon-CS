
// SwimCommand.cs

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
	public class SwimCommand : EamonRT.Game.Commands.Command, Framework.Commands.ISwimCommand
	{
		public override void Execute()
		{
			var columnOfWaterArtifact = gADB[48];

			Debug.Assert(columnOfWaterArtifact != null);

			var gotoCleanup = false;

			// Acid moat

			if (ActorRoom.Uid == 26 || ActorRoom.Uid == 27)
			{
				gEngine.InjurePartyAndDamageEquipment(ActorRoom, 23, ActorRoom.Uid == 26 ? 27 : 26, 2, 0.2, s => NextState = s, ref gotoCleanup);

				if (gotoCleanup)
				{
					goto Cleanup;
				}

				gGameState.R2 = ActorRoom.Uid == 26 ? 27 : 26;

				NextState = gEngine.CreateInstance<IAfterPlayerMoveState>();

				goto Cleanup;
			}

			// White room / Column of water

			if (ActorRoom.Uid == 38 && gGameState.KT == 1)
			{
				gEngine.PrintEffectDesc(43);

				gGameState.R2 = 40;

				NextState = gEngine.CreateInstance<IAfterPlayerMoveState>();

				goto Cleanup;
			}

			// Outer chamber / Column of water

			if (ActorRoom.Uid == 40 && gGameState.KT == 1)
			{
				gEngine.PrintEffectDesc(44);

				gGameState.R2 = 38;

				gGameState.KT = 0;

				columnOfWaterArtifact.SetInLimbo();

				NextState = gEngine.CreateInstance<IAfterPlayerMoveState>();

				goto Cleanup;
			}

			// White room / Desert spring

			if (ActorRoom.Uid == 38 || ActorRoom.Uid == 55)
			{
				gOut.Print("You are wet.");

				goto Cleanup;
			}

			gOut.Print("I don't understand.");

		Cleanup:

			if (NextState == null)
			{
				NextState = gEngine.CreateInstance<IMonsterStartState>();
			}
		}

		public SwimCommand()
		{
			SortOrder = 450;

			IsNew = true;

			Name = "SwimCommand";

			Verb = "swim";

			Type = CommandType.Miscellaneous;
		}
	}
}
