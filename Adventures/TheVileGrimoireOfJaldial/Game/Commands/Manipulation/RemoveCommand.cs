
// RemoveCommand.cs

// Copyright (c) 2014+ by Michael Penner.  All rights reserved.

using System.Diagnostics;
using Eamon.Framework.Primitive.Enums;
using Eamon.Game.Attributes;
using Eamon.Game.Utilities;
using EamonRT.Framework.Commands;
using EamonRT.Framework.Primitive.Enums;
using EamonRT.Framework.States;
using static TheVileGrimoireOfJaldial.Game.Plugin.Globals;

namespace TheVileGrimoireOfJaldial.Game.Commands
{
	[ClassMappings]
	public class RemoveCommand : EamonRT.Game.Commands.RemoveCommand, IRemoveCommand
	{
		public override void ProcessEvents(EventType eventType)
		{
			base.ProcessEvents(eventType);

			// Steel gauntlets boost weapon skills

			if (eventType == EventType.AfterRemoveWornArtifact && DobjArtifact.Uid == 16)
			{
				var weaponValues = EnumUtil.GetValues<Weapon>();

				foreach (var wv in weaponValues)
				{
					gCharacter.ModWeaponAbility(wv, -5);
				}
			}
		}

		public override void ExecuteForPlayer()
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

				if (DobjArtifact.Uid != 19 && DobjArtifact.Uid == gGameState.Ar && cloakArtifact.IsWornByMonster(ActorMonster))
				{
					gOut.Print("[Removing {0} first{1}]", cloakArtifact.GetTheName(), gEngine.EnableScreenReaderMode ? "" : ".");

					cloakArtifact.SetCarriedByMonster(ActorMonster);

					armorArtifact.Wearable.Field1 -= 2;
				}

				base.ExecuteForPlayer();
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

					if (bucketArtifact.IsCarriedByMonster(ActorMonster) || bucketArtifact.IsInRoom(ActorRoom))
					{
						gOut.Print("[Using {0}{1}]", bucketArtifact.GetTheName(), gEngine.EnableScreenReaderMode ? "" : ".");

						var command = gEngine.CreateInstance<IUseCommand>();

						CopyCommandData(command, false);

						command.Dobj = bucketArtifact;

						NextState = command;
					}
					else
					{
						gOut.Print("There's no obvious way to do that.");

						NextState = gEngine.CreateInstance<IStartState>();
					}
				}
				else
				{
					base.ExecuteForPlayer();
				}
			}
			else
			{
				base.ExecuteForPlayer();
			}
		}
	}
}
