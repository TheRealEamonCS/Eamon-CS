
// WearCommand.cs

// Copyright (c) 2014+ by Michael Penner.  All rights reserved.

using System.Diagnostics;
using Eamon.Framework.Primitive.Enums;
using Eamon.Game.Attributes;
using Eamon.Game.Utilities;
using EamonRT.Framework.Commands;
using EamonRT.Framework.Primitive.Enums;
using static TheVileGrimoireOfJaldial.Game.Plugin.PluginContext;

namespace TheVileGrimoireOfJaldial.Game.Commands
{
	[ClassMappings]
	public class WearCommand : EamonRT.Game.Commands.WearCommand, IWearCommand
	{
		public override void ProcessEvents(EventType eventType)
		{
			// Steel gauntlets boost weapon skills

			if (eventType == EventType.AfterWearArtifact && DobjArtifact.Uid == 16)
			{
				var weaponValues = EnumUtil.GetValues<Weapon>();

				foreach (var wv in weaponValues)
				{
					var weapon = gEngine.GetWeapons(wv);

					Debug.Assert(weapon != null);

					gCharacter.ModWeaponAbilities(wv, 5);

					if (gCharacter.GetWeaponAbilities(wv) > weapon.MaxValue)
					{
						gCharacter.SetWeaponAbilities(wv, weapon.MaxValue);
					}
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

			// Crimson cloak boosts armor class

			if (DobjArtifact.Uid == 19 && DobjArtifact.IsCarriedByCharacter())
			{
				var armorArtifact = gGameState.Ar > 0 ? gADB[gGameState.Ar] : null;

				if (armorArtifact != null)
				{
					armorArtifact.Wearable.Field1 += 2;
				}
				else
				{
					DobjArtifact.Wearable.Field1 += 2;
				}
			}

			base.Execute();
		}
	}
}
