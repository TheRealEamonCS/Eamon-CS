
// ThrowCommand.cs

// Copyright (c) 2014+ by Michael Penner.  All rights reserved.

using System.Diagnostics;
using Eamon.Framework.Primitive.Enums;
using Eamon.Game.Attributes;
using EamonRT.Framework.States;
using static ThePyramidOfAnharos.Game.Plugin.Globals;

namespace ThePyramidOfAnharos.Game.Commands
{
	[ClassMappings]
	public class ThrowCommand : EamonRT.Game.Commands.Command, Framework.Commands.IThrowCommand
	{
		public override void Execute()
		{
			Debug.Assert(DobjArtifact != null);

			// Rope

			if ((ActorRoom.Uid == 22 || ActorRoom.Uid == 25) && DobjArtifact.Uid == 13 && DobjArtifact.IsCarriedByCharacter())
			{
				gEngine.PrintEffectDesc(42);

				DobjArtifact.SetInRoom(ActorRoom);

				gGameState.KG = 1;

				goto Cleanup;
			}

			if (DobjArtifact.IsCarriedByCharacter())
			{
				gOut.Print("That would accomplish nothing.");

				gOut.Print("Try something else.");

				goto Cleanup;
			}

			gOut.Print("I don't understand.");

		Cleanup:

			if (NextState == null)
			{
				NextState = gEngine.CreateInstance<IMonsterStartState>();
			}
		}

		public ThrowCommand()
		{
			SortOrder = 440;

			IsNew = true;

			Name = "ThrowCommand";

			Verb = "throw";

			Type = CommandType.Manipulation;
		}
	}
}
