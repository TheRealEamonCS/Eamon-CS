
// DisassembleCommand.cs

// Copyright (c) 2014+ by Michael Penner.  All rights reserved.

using System.Diagnostics;
using Eamon.Framework.Primitive.Enums;
using Eamon.Game.Attributes;
using EamonRT.Framework.States;
using static Firestorm.Game.Plugin.Globals;

namespace Firestorm.Game.Commands
{
	[ClassMappings]
	public class DisassembleCommand : EamonRT.Game.Commands.Command, Framework.Commands.IDisassembleCommand
	{
		public override void ExecuteForPlayer()
		{
			Debug.Assert(DobjArtifact != null);

			var zephetteMonster = gMDB[4];

			Debug.Assert(zephetteMonster != null);

			var fliprootArtifact = gADB[10];

			Debug.Assert(fliprootArtifact != null);

			var fertilizerArtifact = gADB[14];

			Debug.Assert(fertilizerArtifact != null);

			var redswordArtifact = gADB[17];

			Debug.Assert(redswordArtifact != null);

			var scrapMetalArtifact = gADB[26];

			Debug.Assert(scrapMetalArtifact != null);

			var scatteredRockPileArtifact = gADB[35];

			Debug.Assert(scatteredRockPileArtifact != null);

			// Sweatbox

			if (DobjArtifact.Uid == 25)
			{
				gOut.Print("The sweatbox is no more.");

				DobjArtifact.SetInLimbo();

				scrapMetalArtifact.SetInRoom(ActorRoom);

				if (redswordArtifact.IsInLimbo())
				{
					gOut.Print("You find a sword!");

					redswordArtifact.SetInRoom(ActorRoom);
				}
				else
				{
					gOut.Print("Lot of good that did.");
				}

				goto Cleanup;
			}

			// Rock pile

			if (DobjArtifact.Uid == 34)
			{
				gOut.Print("The rocks are scattered all over.");

				DobjArtifact.SetInLimbo();

				scatteredRockPileArtifact.SetInRoom(ActorRoom);

				if (fliprootArtifact.IsInLimbo())
				{
					if (fertilizerArtifact.IsCarriedByMonster(zephetteMonster))
					{
						gOut.Print("You see an odd-looking plant.");

						fliprootArtifact.SetInRoom(ActorRoom);
					}
					else
					{
						gOut.Print("The soil is terrible for gardening.");
					}
				}
				else
				{
					gOut.Print("You already have the fliproot! Are you getting greedy?");
				}

				goto Cleanup;
			}

			// Scrap metal / Scattered rock pile

			if (DobjArtifact.Uid == 26 || DobjArtifact.Uid == 35)
			{
				gOut.Print("It's already in pieces.");

				NextState = gEngine.CreateInstance<IStartState>();

				goto Cleanup;
			}

			gOut.Print("Yeah... might happen.");

			NextState = gEngine.CreateInstance<IStartState>();

		Cleanup:

			if (NextState == null)
			{
				NextState = gEngine.CreateInstance<IMonsterStartState>();
			}
		}

		public DisassembleCommand()
		{
			SortOrder = 450;

			IsNew = true;

			Name = "DisassembleCommand";

			Verb = "disassemble";

			Type = CommandType.Manipulation;
		}
	}
}
