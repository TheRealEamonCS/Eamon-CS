
// GetCommand.cs

// Copyright (c) 2014+ by Michael Penner.  All rights reserved.

using System.Diagnostics;
using Eamon.Framework;
using Eamon.Framework.Primitive.Classes;
using Eamon.Game.Attributes;
using EamonRT.Framework.Commands;
using static OrbOfMyLife.Game.Plugin.Globals;

namespace OrbOfMyLife.Game.Commands
{
	[ClassMappings]
	public class GetCommand : EamonRT.Game.Commands.GetCommand, IGetCommand
	{
		public override void ProcessArtifact(IArtifact artifact, IArtifactCategory ac, ref bool nlFlag)
		{
			Debug.Assert(artifact != null);

			Debug.Assert(ac != null);

			var eldersMonster = gMDB[6];

			Debug.Assert(eldersMonster != null);

			var keyOfIskArtifact = gADB[5];

			Debug.Assert(keyOfIskArtifact != null);

			var orbOfLifeArtifact = gADB[23];

			Debug.Assert(orbOfLifeArtifact != null);

			var woodenCrateArtifact = gADB[32];

			Debug.Assert(woodenCrateArtifact != null);

			// Wooden box

			if (artifact.Uid == 10 && eldersMonster.IsInRoom(ActorRoom) && !keyOfIskArtifact.IsCarriedByMonster(eldersMonster))
			{
				ProcessAction(100, () => gEngine.PrintEffectDesc(19), ref nlFlag);
			}

			// Orb of Life (force field)

			else if (artifact.Uid == 9 && !gGameState.IV)
			{
				ProcessAction(101, () => gOut.Print("You can't reach {0}!", artifact.GetTheName()), ref nlFlag);
			}
			else
			{
				// Orb of Life (no force field)

				if (artifact.Uid == 9)
				{
					artifact.SetInLimbo();

					orbOfLifeArtifact.SetInRoom(ActorRoom);

					artifact = orbOfLifeArtifact;
				}

				base.ProcessArtifact(artifact, ac, ref nlFlag);
			}

			if (gGameState.IV && artifact.IsCarriedByMonster(ActorMonster))
			{
				gEngine.MonstersGetUnnerved(true);
			}

			// Tarp

			if (artifact.Uid == 39 && artifact.IsCarriedByMonster(ActorMonster) && woodenCrateArtifact.IsEmbeddedInRoom(ActorRoom))
			{
				woodenCrateArtifact.SetInRoom(ActorRoom);
			}
		}
	}
}
