
// CommandParser.cs

// Copyright (c) 2014+ by Michael Penner.  All rights reserved.

using System.Diagnostics;
using System.Linq;
using Eamon.Framework;
using Eamon.Framework.Primitive.Enums;
using Eamon.Game.Attributes;
using EamonRT.Framework.Commands;
using EamonRT.Framework.Parsing;
using EamonRT.Framework.States;
using TheWayfarersInn.Framework.Primitive.Enums;
using static TheWayfarersInn.Game.Plugin.Globals;

namespace TheWayfarersInn.Game.Parsing
{
	[ClassMappings]
	public class CommandParser : EamonRT.Game.Parsing.CommandParser, ICommandParser
	{
		public override void SetLastNameStrings(IGameBase obj, string objDataName, IArtifact artifact, IMonster monster)
		{
			var artifactUids = new long[] { 27, 43, 49, 51, 54, 139, 141, 143, 144, 145, 146, 147, 148, 149, 185 };

			base.SetLastNameStrings(obj, objDataName, artifact, monster);

			// Some artifacts can be referred to as both "it" and "them" for ease of use

			if (artifact != null && artifactUids.Contains(artifact.Uid))
			{
				artifact.IsPlural = !artifact.IsPlural;

				base.SetLastNameStrings(obj, objDataName, artifact, monster);

				artifact.IsPlural = !artifact.IsPlural;
			}
		}

		public override void CheckPlayerCommand(bool afterFinishParsing)
		{
			Debug.Assert(NextCommand != null);

			base.CheckPlayerCommand(afterFinishParsing);

			if (afterFinishParsing)
			{
				// Gaping hole

				if (DobjArtifact != null && DobjArtifact.Uid == 42 && gGameState.TotalCentipedeCounter > 0)
				{
					var eventState = gGameState.GetEventState(EventState.BlueBandedCentipedes);

					if (eventState == 0)
					{
						eventState++;

						gGameState.SetEventState(EventState.BlueBandedCentipedes, eventState);

						gGameState.AfterPrintPlayerRoomEventHeap.Insert02(gGameState.CurrTurn + 1, "BlueBandedCentipedes", 0, null);
					}
				}

				// Haunting / Windows

				else if (!(NextCommand is IExamineCommand) && DobjArtifact != null && (DobjArtifact.Uid == 151 || DobjArtifact.Uid == 153))
				{
					gOut.Print("{0} {1} not accessible from here.", DobjArtifact.GetTheName(true), DobjArtifact.EvalPlural("is", "are"));

					NextState = gEngine.CreateInstance<IMonsterStartState>();
				}

				// ATTACK/BLAST various things

				else if ((NextCommand is IBlastCommand && gCharacter.GetSpellAbility(Spell.Blast) > 0) || (NextCommand is IAttackCommand && ActorMonster.Weapon > 0))
				{
					// Back wall

					if (DobjArtifact != null && DobjArtifact.Uid == 53)
					{
						if (gGameState.GetNBTL(Friendliness.Enemy) > 0)
						{
							NextCommand.PrintEnemiesNearby();

							NextState = gEngine.CreateInstance<IMonsterStartState>();
						}
						else if (NextCommand is IBlastCommand || ActorMonster.Weapon != 12)
						{
							gOut.Print("Surely there are tools better suited for that job!");

							NextState = gEngine.CreateInstance<IMonsterStartState>();
						}
					}

					// Mirror

					else if (DobjArtifact != null && DobjArtifact.Uid == 71)
					{
						gOut.Print("That would buy you seven years of bad luck.");

						NextState = gEngine.CreateInstance<IMonsterStartState>();
					}

					// Window
	
					else if (DobjArtifact != null && DobjArtifact.Uid == 137)
					{
						gOut.Print("The investors syndicate would not be happy with your vandalism.");

						NextState = gEngine.CreateInstance<IMonsterStartState>();
					}
				}
			}
		}
	}
}
