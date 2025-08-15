
// TrainCommand.cs

// Copyright (c) 2014+ by Michael Penner.  All rights reserved.

using System.Diagnostics;
using Eamon.Framework.Primitive.Enums;
using Eamon.Game.Attributes;
using EamonRT.Framework.States;
using static Firestorm.Game.Plugin.Globals;

namespace Firestorm.Game.Commands
{
	[ClassMappings]
	public class TrainCommand : EamonRT.Game.Commands.Command, Framework.Commands.ITrainCommand
	{
		public override void ExecuteForPlayer()
		{
			var translatorEarplugArtifact = gADB[16];

			Debug.Assert(translatorEarplugArtifact != null);

			var redswordArtifact = gADB[17];

			Debug.Assert(redswordArtifact != null);

			if (ActorRoom.Uid != 29)
			{
				gOut.Print("You can't do that here.");

				NextState = gEngine.CreateInstance<IStartState>();

				goto Cleanup;
			}

			if (!redswordArtifact.IsCarriedByMonster(ActorMonster))
			{
				gOut.Print("You need the redsword to do your training.");

				NextState = gEngine.CreateInstance<IStartState>();

				goto Cleanup;
			}

			if (!translatorEarplugArtifact.IsWornByMonster(ActorMonster))
			{
				gOut.Print("You try, but can't understand a word of it, so you learn nothing.");

				goto Cleanup;
			}

			gEngine.PrintEffectDesc(27);

			gGameState.GH = 1;

		Cleanup:

			if (NextState == null)
			{
				NextState = gEngine.CreateInstance<IMonsterStartState>();
			}
		}

		public TrainCommand()
		{
			SortOrder = 480;

			IsNew = true;

			Name = "TrainCommand";

			Verb = "train";

			Type = CommandType.Miscellaneous;
		}
	}
}
