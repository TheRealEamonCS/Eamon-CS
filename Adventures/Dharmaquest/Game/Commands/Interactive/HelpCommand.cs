
// HelpCommand.cs

// Copyright (c) 2014+ by Michael Penner.  All rights reserved.

using System.Diagnostics;
using Eamon.Framework.Primitive.Enums;
using Eamon.Game.Attributes;
using EamonRT.Framework.States;
using static Dharmaquest.Game.Plugin.Globals;

namespace Dharmaquest.Game.Commands
{
	[ClassMappings]
	public class HelpCommand : EamonRT.Game.Commands.Command, Framework.Commands.IHelpCommand
	{
		public override void ExecuteForPlayer()
		{
			Debug.Assert(DobjArtifact != null || DobjMonster != null);

			var bronzeRingArtifact = gADB[4];

			Debug.Assert(bronzeRingArtifact != null);

			var goldRingArtifact = gADB[5];

			Debug.Assert(goldRingArtifact != null);

			// Hermit

			if (DobjArtifact?.Uid == 8)
			{
				DobjArtifact.SetInLimbo();

				gEngine.PrintEffectDesc(4);

				bronzeRingArtifact.SetInRoom(ActorRoom);

				gGameState.Karma += 25;

				goto Cleanup;
			}

			// Drowning boy

			if (DobjArtifact?.Uid == 9)
			{
				DobjArtifact.SetInLimbo();

				gEngine.PrintEffectDesc(5);

				gGameState.Karma += 25;

				goto Cleanup;
			}

			// Old woman

			if (DobjArtifact?.Uid == 10)
			{
				DobjArtifact.SetInLimbo();

				gEngine.PrintEffectDesc(6);

				goldRingArtifact.SetInRoom(ActorRoom);

				gGameState.Karma += 25;

				goto Cleanup;
			}

			// Little girl

			if (DobjArtifact?.Uid == 11)
			{
				DobjArtifact.SetInLimbo();

				gEngine.PrintEffectDesc(7);

				gGameState.Karma += 25;

				goto Cleanup;
			}

			gOut.Print("{0} cannot be helped.", Dobj.GetTheName(true));

			NextState = gEngine.CreateInstance<IStartState>();

		Cleanup:

			if (NextState == null)
			{
				NextState = gEngine.CreateInstance<IMonsterStartState>();
			}
		}

		public HelpCommand()
		{
			SortOrder = 295;

			IsNew = true;

			Name = "HelpCommand";

			Verb = "help";

			Type = CommandType.Interactive;
		}
	}
}
