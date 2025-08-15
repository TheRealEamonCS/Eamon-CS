
// GreetCommand.cs

// Copyright (c) 2014+ by Michael Penner.  All rights reserved.

using System.Diagnostics;
using Eamon.Framework.Primitive.Enums;
using Eamon.Game.Attributes;
using EamonRT.Framework.Commands;
using EamonRT.Framework.States;
using static Firestorm.Game.Plugin.Globals;

namespace Firestorm.Game.Commands
{
	[ClassMappings]
	public class GreetCommand : EamonRT.Game.Commands.Command, Framework.Commands.IGreetCommand
	{
		public override void ExecuteForPlayer()
		{
			Debug.Assert(DobjArtifact != null || DobjMonster != null);

			var translatorEarplugArtifact = gADB[16];

			Debug.Assert(translatorEarplugArtifact != null);

			var beerArtifact = gADB[22];

			Debug.Assert(beerArtifact != null);

			var tokenArtifact = gADB[56];

			Debug.Assert(tokenArtifact != null);

			var buzzSwordArtifact = gADB[66];

			Debug.Assert(buzzSwordArtifact != null);

			if (DobjArtifact != null)
			{
				gOut.Print("What would that accomplish?");

				NextState = gEngine.CreateInstance<IStartState>();

				goto Cleanup;
			}

			if (DobjMonster.Reaction == Friendliness.Enemy && ActorMonster.Weapon > 0)
			{
				gOut.Print("Let the combat begin...");

				var command = gEngine.CreateInstance<IAttackCommand>();

				CopyCommandData(command);

				NextState = command;

				goto Cleanup;
			}

			// Zephette

			if (DobjMonster.Uid == 4)
			{
				var hint5 = gEngine.HDB[5];

				Debug.Assert(hint5 != null);

				hint5.Active = true;
			}

			// Mystique

			else if (DobjMonster.Uid == 6)
			{
				var hint7 = gEngine.HDB[7];

				Debug.Assert(hint7 != null);

				hint7.Active = true;
			}

			// Qualin

			else if (DobjMonster.Uid == 10)
			{
				var hint8 = gEngine.HDB[8];

				Debug.Assert(hint8 != null);

				hint8.Active = true;
			}

			// Arlen

			else if (DobjMonster.Uid == 7)
			{
				var hint10 = gEngine.HDB[10];

				Debug.Assert(hint10 != null);

				hint10.Active = true;
			}

			gOut.Print("{0} says:", DobjMonster.GetTheName(true, true, false, false, true));

			if (DobjMonster.Uid > 1 && DobjMonster.Uid < 7)
			{
				gEngine.PrintEffectDesc(DobjMonster.Uid - 1);

				goto Cleanup;
			}

			if (DobjMonster.Uid == 7)
			{
				gEngine.PrintEffectDesc(translatorEarplugArtifact.IsWornByMonster(ActorMonster) ? 6 : 7);

				goto Cleanup;
			}

			if (DobjMonster.Uid > 7 && DobjMonster.Uid < 12)
			{
				gEngine.PrintEffectDesc(DobjMonster.Uid);

				goto Cleanup;
			}

			if (DobjMonster.Uid == 12)
			{
				if (tokenArtifact.IsInLimbo())
				{
					gEngine.PrintEffectDesc(15);

					tokenArtifact.SetInRoom(ActorRoom);
				}
				else
				{
					gEngine.PrintEffectDesc(60);

					gOut.Print("Rex runs away!");

					DobjMonster.SetInLimbo();
				}

				goto Cleanup;
			}

			if (DobjMonster.Uid == 13)
			{
				gOut.Print("\"Hello.\"");

				goto Cleanup;
			}

			if (DobjMonster.Uid == 21)
			{
				gEngine.PrintEffectDesc(17);

				goto Cleanup;
			}

			// Note: limited to one beer, otherwise it becomes a magic point hack. Possibly a bug in the original, but not clear.

			if (DobjMonster.Uid == 30 && gGameState.GetPG(9) == 0)
			{
				gEngine.PrintEffectDesc(33);

				beerArtifact.SetInRoom(ActorRoom);

				gGameState.SetPG(9, 1);

				goto Cleanup;
			}

			if (DobjMonster.Uid != 45 && DobjMonster.Reaction == Friendliness.Friend)
			{
				gOut.Print("\"Let's go fight!\"");

				goto Cleanup;
			}

			if (DobjMonster.Uid == 29)
			{
				gOut.Print("\"Boo!\"");

				goto Cleanup;
			}

			if (DobjMonster.Uid == 39)
			{
				gOut.Print("\"I don't think I like you too much.\"");

				goto Cleanup;
			}

			if (DobjMonster.Uid == 45 && DobjMonster.Reaction == Friendliness.Neutral && !buzzSwordArtifact.IsInLimbo() && !buzzSwordArtifact.IsCarriedByMonster(DobjMonster))
			{
				gOut.Print("\"I need my weapon!\"");

				goto Cleanup;
			}

			if (DobjMonster.Uid == 45 && DobjMonster.Reaction == Friendliness.Friend)
			{
				gOut.Print("\"Together we will defeat Thorak.\"");

				goto Cleanup;
			}

			gOut.Print("\"Eh?\"");

		Cleanup:

			if (NextState == null)
			{
				NextState = gEngine.CreateInstance<IMonsterStartState>();
			}
		}

		public GreetCommand()
		{
			SortOrder = 440;

			IsNew = true;

			Name = "GreetCommand";

			Verb = "greet";

			Type = CommandType.Interactive;
		}
	}
}
