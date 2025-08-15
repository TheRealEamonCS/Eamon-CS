
// UseCommand.cs

// Copyright (c) 2014+ by Michael Penner.  All rights reserved.

using System.Diagnostics;
using Eamon.Framework.Primitive.Enums;
using Eamon.Game.Attributes;
using EamonRT.Framework.Commands;
using EamonRT.Framework.Primitive.Enums;
using EamonRT.Framework.States;
using static Firestorm.Game.Plugin.Globals;

namespace Firestorm.Game.Commands
{
	[ClassMappings]
	public class UseCommand : EamonRT.Game.Commands.UseCommand, IUseCommand
	{
		public override void ProcessEvents(EventType eventType)
		{
			base.ProcessEvents(eventType);

			if (eventType == EventType.AfterUseArtifact)
			{
				if (DobjArtAc.Type == ArtifactType.LightSource)
				{
					NextState = gEngine.CreateInstance<ILightCommand>();

					CopyCommandData(NextState as ICommand);

					GotoCleanup = true;

					goto Cleanup;
				}

				if (DobjArtAc.Type == ArtifactType.Readable)
				{
					NextState = gEngine.CreateInstance<IReadCommand>();

					CopyCommandData(NextState as ICommand);

					GotoCleanup = true;

					goto Cleanup;
				}
			}

		Cleanup:

			;
		}

		public override void ExecuteForPlayer()
		{
			Debug.Assert(DobjArtifact != null);

			// Pebbles

			if (DobjArtifact.Uid == 40)
			{
				if (gGameState.PZ == 1)
				{
					DobjArtifact.Field1 -= 1;

					gOut.Print("The poison's effects have been stopped.");

					gGameState.PZ = 0;

					if (DobjArtifact.Field1 == 0)
					{
						gOut.Print("You're all out of pebbles!");

						DobjArtifact.SetInLimbo();
					}
					else
					{
						gEngine.PrintPebblesLeft(DobjArtifact);
					}

					NextState = gEngine.CreateInstance<IMonsterStartState>();
				}
				else
				{
					gOut.Print("That would be a real waste.");

					NextState = gEngine.CreateInstance<IStartState>();
				}
			}

			// Healing herbs

			else if (DobjArtifact.Uid == 41)
			{
				DobjArtifact.Field1 -= 1;

				ActorMonster.DmgTaken = 0;

				gOut.Print("You're healed.");

				if (DobjArtifact.Field1 == 0)
				{
					gOut.Print("There's none left!");

					DobjArtifact.SetInLimbo();
				}
				else
				{
					gEngine.PrintHealingHerbsLeft(DobjArtifact);
				}

				NextState = gEngine.CreateInstance<IMonsterStartState>();
			}
			else
			{
				base.ExecuteForPlayer();
			}
		}

		public UseCommand()
		{
			ArtTypes = new ArtifactType[] { ArtifactType.DisguisedMonster, ArtifactType.Weapon, ArtifactType.MagicWeapon, ArtifactType.LightSource, ArtifactType.Drinkable, ArtifactType.Readable, ArtifactType.Edible, ArtifactType.Wearable };
		}
	}
}
