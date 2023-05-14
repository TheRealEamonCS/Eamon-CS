
// DigCommand.cs

// Copyright (c) 2014+ by Michael Penner.  All rights reserved.

using System.Diagnostics;
using System.Linq;
using Eamon.Framework.Primitive.Enums;
using Eamon.Game.Attributes;
using EamonRT.Framework.States;
using static TheDeepCanyon.Game.Plugin.Globals;

namespace TheDeepCanyon.Game.Commands
{
	[ClassMappings]
	public class DigCommand : EamonRT.Game.Commands.Command, Framework.Commands.IDigCommand
	{
		public override void Execute()
		{
			var shovelArtifact = gADB[18];

			Debug.Assert(shovelArtifact != null);

			if (!shovelArtifact.IsInRoom(ActorRoom) && !shovelArtifact.IsCarriedByMonster(ActorMonster))
			{
				gOut.Print("You don't have anything to dig with.");

				goto Cleanup;
			}

			PrintOkay();

			// Note: the peanuts existed in the Apple II game but weren't used, for whatever reason; it seems appropriate to put them back in!

			var peanutsArtifact = gADB[20];

			Debug.Assert(peanutsArtifact != null);

			if (ActorRoom.Uid == 31 && !gGameState.Peanuts)
			{
				gOut.Print("You dug up some peanuts.");

				peanutsArtifact.SetInRoom(ActorRoom);

				gGameState.Peanuts = true;

				goto Cleanup;
			}

			var elephantStatueArtifact = gADB[13];

			Debug.Assert(elephantStatueArtifact != null);

			if (ActorRoom.Uid == 38 && !gGameState.ElephantStatue)
			{
				gEngine.PrintEffectDesc(4);

				elephantStatueArtifact.SetInRoom(ActorRoom);

				gGameState.ElephantStatue = true;

				goto Cleanup;
			}

			// Note: the baby sand worm existed in the Apple II game but wasn't used, for whatever reason; it seems appropriate to put it back in!

			var sandyRoomUids = new long[] { 1, 2, 5, 6, 7, 8, 9, 10, 45, 46, 47, 48, 49, 50, 51, 52, 54, 57, 58 };

			var babySandWormMonster = gMDB[1];

			Debug.Assert(babySandWormMonster != null);

			if (sandyRoomUids.Contains(ActorRoom.Uid) && babySandWormMonster.Location == -1)
			{
				gEngine.PrintEffectDesc(8);

				babySandWormMonster.SetInRoom(ActorRoom);

				goto Cleanup;
			}

			var groundhogMonster = gMDB[17];

			Debug.Assert(groundhogMonster != null);

			if (ActorRoom.Uid == 34 && groundhogMonster.Location == -1)
			{
				gEngine.PrintEffectDesc(5);

				groundhogMonster.SetInRoom(ActorRoom);

				goto Cleanup;
			}

			var diamondsArtifact = gADB[10];

			Debug.Assert(diamondsArtifact != null);

			if (ActorRoom.Uid == 24 && !gGameState.Diamonds)
			{
				gEngine.PrintEffectDesc(7);

				diamondsArtifact.SetInRoom(ActorRoom);

				gGameState.Diamonds = true;

				goto Cleanup;
			}

			gOut.Print("You found nothing.");

		Cleanup:

			if (NextState == null)
			{
				NextState = gEngine.CreateInstance<IMonsterStartState>();
			}
		}

		public DigCommand()
		{
			SortOrder = 450;

			IsNew = true;

			Name = "DigCommand";

			Verb = "dig";

			Type = CommandType.Miscellaneous;
		}
	}
}
