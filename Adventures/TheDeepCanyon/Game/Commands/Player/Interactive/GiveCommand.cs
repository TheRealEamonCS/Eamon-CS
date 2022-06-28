
// GiveCommand.cs

// Copyright (c) 2014+ by Michael Penner.  All rights reserved.

using Eamon.Game.Attributes;
using EamonRT.Framework.Commands;
using EamonRT.Framework.Primitive.Enums;
using static TheDeepCanyon.Game.Plugin.PluginContext;

namespace TheDeepCanyon.Game.Commands
{
	[ClassMappings]
	public class GiveCommand : EamonRT.Game.Commands.GiveCommand, IGiveCommand
	{
		public override void ProcessEvents(EventType eventType)
		{
			base.ProcessEvents(eventType);

			if (eventType == EventType.AfterEnforceMonsterWeightLimitsCheck)
			{
				// Fido

				if (IobjMonster.Uid == 11)
				{
					// Dead bodies

					if (DobjArtifact.DeadBody != null && gGameState.FidoSleepCounter <= 0)
					{
						gOut.Print("You give {0} to Fido.", DobjArtifact.GetTheName());

						gOut.EnableOutput = false;

						var dropCommand = Globals.CreateInstance<IDropCommand>(x =>
						{
							x.ActorMonster = ActorMonster;

							x.ActorRoom = ActorRoom;

							x.Dobj = DobjArtifact;
						});

						dropCommand.Execute();

						gOut.EnableOutput = true;

						GotoCleanup = true;
					}
					else
					{
						gEngine.PrintMonsterEmotes(IobjMonster);

						gOut.WriteLine();

						GotoCleanup = true;
					}
				}

				// Elephants

				else if (IobjMonster.Uid == 24)
				{
					// Peanuts

					if (DobjArtifact.Uid == 20)
					{
						gOut.Print("The elephants take the peanuts and walk guiltily away.");

						IobjMonster.SetInLimbo();

						DobjArtifact.SetInLimbo();

						GotoCleanup = true;
					}

					// Mouse

					else if (DobjArtifact.Uid == 19)
					{
						gOut.Print("You give the mouse to the elephants.");

						gOut.EnableOutput = false;

						var dropCommand = Globals.CreateInstance<IDropCommand>(x =>
						{
							x.ActorMonster = ActorMonster;

							x.ActorRoom = ActorRoom;

							x.Dobj = DobjArtifact;
						});

						dropCommand.Execute();

						gOut.EnableOutput = true;

						GotoCleanup = true;
					}
					else
					{
						gOut.Print("The elephants refuse to take your gift.");

						GotoCleanup = true;
					}
				}

				// Disable further bribing

				else if (!IobjMonster.HasCarriedInventory())
				{
					gEngine.PrintMonsterEmotes(IobjMonster);

					gOut.WriteLine();

					GotoCleanup = true;
				}
			}
			else if (eventType == EventType.BeforeTakePlayerGold)
			{
				// Give $ to Elephants

				if (IobjMonster.Uid == 24)
				{
					gOut.Print("The elephants refuse to take your gold.");

					GotoCleanup = true;
				}

				// Disable further bribing

				else if (!IobjMonster.HasCarriedInventory())
				{
					gEngine.PrintMonsterEmotes(IobjMonster);

					gOut.WriteLine();

					GotoCleanup = true;
				}
			}
		}
	}
}
