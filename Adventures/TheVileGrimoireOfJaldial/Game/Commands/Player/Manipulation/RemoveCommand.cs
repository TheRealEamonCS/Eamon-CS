
// RemoveCommand.cs

// Copyright (c) 2014+ by Michael Penner.  All rights reserved.

using System.Diagnostics;
using Eamon.Framework.Primitive.Enums;
using Eamon.Game.Attributes;
using Eamon.Game.Utilities;
using EamonRT.Framework.Commands;
using EamonRT.Framework.Primitive.Enums;
using EamonRT.Framework.States;
using static TheVileGrimoireOfJaldial.Game.Plugin.PluginContext;

namespace TheVileGrimoireOfJaldial.Game.Commands
{
	[ClassMappings]
	public class RemoveCommand : EamonRT.Game.Commands.RemoveCommand, IRemoveCommand
	{
		public override void ProcessEvents(EventType eventType)
		{
			// Steel gauntlets boost weapon skills

			if (eventType == EventType.AfterRemoveWornArtifact && DobjArtifact.Uid == 16)
			{
				var weaponValues = EnumUtil.GetValues<Weapon>();

				foreach (var wv in weaponValues)
				{
					gCharacter.ModWeaponAbilities(wv, -5);
				}
			}
			else
			{
				base.ProcessEvents(eventType);
			}
		}

		public override void Execute()
		{
			Debug.Assert(DobjArtifact != null);

			if (IobjArtifact == null)
			{
				var armorArtifact = gGameState.Ar > 0 ? gADB[gGameState.Ar] : null;

				var cloakArtifact = gADB[19];

				Debug.Assert(cloakArtifact != null);

				// Crimson cloak boosts armor class

				if (DobjArtifact.Uid == 19 && armorArtifact != null)
				{
					armorArtifact.Wearable.Field1 -= 2;
				}

				// Remove crimson cloak if removing armor

				if (DobjArtifact.Uid != 19 && DobjArtifact.Uid == gGameState.Ar && cloakArtifact.IsWornByCharacter())
				{
					gOut.Print("[Removing {0} first.]", cloakArtifact.GetTheName());

					cloakArtifact.SetCarriedByCharacter();

					armorArtifact.Wearable.Field1 -= 2;
				}

				base.Execute();
			}

			// Large fountain

			else if (IobjArtifact.Uid == 24)
			{
				var bucketArtifact = gADB[6];

				Debug.Assert(bucketArtifact != null);

				// Bail out water

				if (DobjArtifact.Uid == 40)
				{
					// Use the wooden bucket 

					if (bucketArtifact.IsCarriedByCharacter() || bucketArtifact.IsInRoom(ActorRoom))
					{
						gOut.Print("[Using {0}.]", bucketArtifact.GetTheName());

						var command = Globals.CreateInstance<IUseCommand>();

						CopyCommandData(command, false);

						command.Dobj = bucketArtifact;

						NextState = command;
					}
					else
					{
						gOut.Print("There's no obvious way to do that.");

						NextState = Globals.CreateInstance<IStartState>();
					}
				}
				else
				{
					base.Execute();
				}
			}
			else
			{
				base.Execute();
			}
		}
	}
}
