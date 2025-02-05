
// GiveCommand.cs

// Copyright (c) 2014+ by Michael Penner.  All rights reserved.

using System.Diagnostics;
using Eamon.Framework.Primitive.Enums;
using Eamon.Game.Attributes;
using EamonRT.Framework.Commands;
using EamonRT.Framework.Primitive.Enums;
using static OrbOfMyLife.Game.Plugin.Globals;

namespace OrbOfMyLife.Game.Commands
{
	[ClassMappings]
	public class GiveCommand : EamonRT.Game.Commands.GiveCommand, IGiveCommand
	{
		public override void ProcessEvents(EventType eventType)
		{
			base.ProcessEvents(eventType);

			if (eventType == EventType.AfterEnforceMonsterWeightLimitsCheck)
			{
				var woodenBoxArtifact = gADB[10];

				Debug.Assert(woodenBoxArtifact != null);

				var jewelOfReomeArtifact = gADB[14];

				Debug.Assert(jewelOfReomeArtifact != null);

				// Siren Witch

				if (IobjMonster.Uid == 5)
				{
					// Wooden box

					if (DobjArtifact.Uid == 10)
					{
						gOut.Print("The witch takes it and gives you something.");

						DobjArtifact.SetCarriedByMonster(IobjMonster);

						if (ActorMonster.CanCarryArtifactWeight(jewelOfReomeArtifact))
						{
							jewelOfReomeArtifact.SetCarriedByMonster(ActorMonster);
						}
						else
						{
							jewelOfReomeArtifact.SetInRoom(ActorRoom);
						}

						IobjMonster.Friendliness = (Friendliness)200;
					}
					else
					{
						IobjMonster.Friendliness = (Friendliness)100;
					}

					IobjMonster.ResolveReaction(gCharacter);

					IobjMonster.Friendliness = IobjMonster.Reaction;

					gEngine.PrintMonsterEmotes(IobjMonster);

					gOut.WriteLine();

					GotoCleanup = true;
				}

				// Elders

				else if (IobjMonster.Uid == 6)
				{
					// Emerald Key of Isk

					if (DobjArtifact.Uid == 5 && woodenBoxArtifact.IsInRoom(ActorRoom))
					{
						gOut.Print("The Council takes the key and hands you the box.");

						DobjArtifact.SetCarriedByMonster(IobjMonster);

						if (ActorMonster.CanCarryArtifactWeight(woodenBoxArtifact))
						{
							woodenBoxArtifact.SetCarriedByMonster(ActorMonster);
						}

						GotoCleanup = true;
					}
				}

				// Tolor

				else if (IobjMonster.Uid == 13)
				{
					gOut.Print("{0} says, \"No thanks.\"", IobjMonster.GetTheName(true));

					GotoCleanup = true;
				}
			}
			else if (eventType == EventType.BeforeTakePlayerGold)
			{
				// Tolor

				if (IobjMonster.Uid == 13)
				{
					gOut.Print("{0} says, \"No thanks.\"", IobjMonster.GetTheName(true));

					GotoCleanup = true;
				}
			}
		}
	}
}
