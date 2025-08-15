
// ReassembleCommand.cs

// Copyright (c) 2014+ by Michael Penner.  All rights reserved.

using System.Diagnostics;
using Eamon.Framework.Primitive.Enums;
using Eamon.Game.Attributes;
using EamonRT.Framework.States;
using static Firestorm.Game.Plugin.Globals;

namespace Firestorm.Game.Commands
{
	[ClassMappings]
	public class ReassembleCommand : EamonRT.Game.Commands.Command, Framework.Commands.IReassembleCommand
	{
		public override void ExecuteForPlayer()
		{
			Debug.Assert(DobjArtifact != null);

			var sweatBoxArtifact = gADB[25];

			Debug.Assert(sweatBoxArtifact != null);

			var rockPileArtifact = gADB[34];

			Debug.Assert(rockPileArtifact != null);

			// Scrap metal

			if (DobjArtifact.Uid == 26)
			{
				gOut.Print("You put it back together.");

				DobjArtifact.SetInLimbo();

				sweatBoxArtifact.SetInRoom(ActorRoom);

				goto Cleanup;
			}

			// Scattered rock pile

			if (DobjArtifact.Uid == 35)
			{
				gOut.Print("You put it back together.");

				DobjArtifact.SetInLimbo();

				rockPileArtifact.SetInRoom(ActorRoom);

				goto Cleanup;
			}

			gOut.Print("{0} in pieces.", DobjArtifact.EvalPlural("It isn't", "They aren't"));

			NextState = gEngine.CreateInstance<IStartState>();

		Cleanup:

			if (NextState == null)
			{
				NextState = gEngine.CreateInstance<IMonsterStartState>();
			}
		}

		public ReassembleCommand()
		{
			SortOrder = 460;

			IsNew = true;

			Name = "ReassembleCommand";

			Verb = "reassemble";

			Type = CommandType.Manipulation;
		}
	}
}
