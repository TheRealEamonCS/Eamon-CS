
// WearCommand.cs

// Copyright (c) 2014+ by Michael Penner.  All rights reserved.

using System.Diagnostics;
using Eamon.Framework;
using Eamon.Framework.Primitive.Classes;
using Eamon.Framework.Primitive.Enums;
using Eamon.Game.Attributes;
using EamonRT.Framework.Commands;
using EamonRT.Framework.Primitive.Enums;
using EamonRT.Framework.States;
using static EamonRT.Game.Plugin.PluginContext;

namespace EamonRT.Game.Commands
{
	[ClassMappings]
	public class WearCommand : Command, IWearCommand
	{
		/// <summary></summary>
		public virtual IArtifactCategory DobjArtAc { get; set; }

		/// <summary></summary>
		public virtual IArtifactCategory ArmorArtifactAc { get; set; }

		/// <summary></summary>
		public virtual IArtifactCategory ShieldArtifactAc { get; set; }

		/// <summary></summary>
		public virtual IArtifactCategory WeaponArtifactAc { get; set; }

		/// <summary></summary>
		public virtual IArtifact ArmorArtifact { get; set; }

		/// <summary></summary>
		public virtual IArtifact ShieldArtifact { get; set; }

		/// <summary></summary>
		public virtual IArtifact WeaponArtifact { get; set; }

		public override void Execute()
		{
			Debug.Assert(DobjArtifact != null);

			DobjArtAc = DobjArtifact.Wearable;

			if (DobjArtAc == null)
			{
				PrintCantVerbObj(DobjArtifact);

				NextState = Globals.CreateInstance<IStartState>();

				goto Cleanup;
			}

			if (DobjArtifact.IsWornByCharacter())
			{
				gOut.Print("You're already wearing {0}!", DobjArtifact.EvalPlural("it", "them"));

				NextState = Globals.CreateInstance<IStartState>();

				goto Cleanup;
			}

			if (!DobjArtifact.IsCarriedByCharacter())
			{
				if (!GetCommandCalled)
				{
					RedirectToGetCommand<IWearCommand>(DobjArtifact);
				}
				else if (DobjArtifact.DisguisedMonster == null)
				{
					NextState = Globals.CreateInstance<IStartState>();
				}

				goto Cleanup;
			}

			if (DobjArtAc.Field1 > 0)
			{
				ArmorArtifact = gADB[gGameState.Ar];

				ShieldArtifact = gADB[gGameState.Sh];

				ArmorArtifactAc = ArmorArtifact != null ? ArmorArtifact.Wearable : null;

				ShieldArtifactAc = ShieldArtifact != null ? ShieldArtifact.Wearable : null;

				if (DobjArtAc.Field1 > 1)
				{
					if (DobjArtAc.Field1 > 14)
					{
						DobjArtAc.Field1 = 14;
					}

					if (ArmorArtifactAc != null)
					{
						gOut.Print("You're already wearing armor!");

						NextState = Globals.CreateInstance<IStartState>();

						goto Cleanup;
					}

					gGameState.Ar = DobjArtifact.Uid;

					ActorMonster.Armor = (DobjArtAc.Field1 / 2) + ((DobjArtAc.Field1 / 2) >= 3 ? 2 : 0) + (ShieldArtifactAc != null ? ShieldArtifactAc.Field1 : 0);
				}
				else
				{
					if (ShieldArtifactAc != null)
					{
						gOut.Print("You're already wearing a shield!");

						NextState = Globals.CreateInstance<IStartState>();

						goto Cleanup;
					}

					// can't wear shield while using two-handed weapon

					WeaponArtifact = ActorMonster.Weapon > 0 ? gADB[ActorMonster.Weapon] : null;

					WeaponArtifactAc = WeaponArtifact != null ? WeaponArtifact.GeneralWeapon : null;

					if (WeaponArtifactAc != null && WeaponArtifactAc.Field5 > 1)
					{
						PrintCantWearShieldWithWeapon(DobjArtifact, WeaponArtifact);

						NextState = Globals.CreateInstance<IStartState>();

						goto Cleanup;
					}

					gGameState.Sh = DobjArtifact.Uid;

					ActorMonster.Armor = (ArmorArtifactAc != null ? (ArmorArtifactAc.Field1 / 2) + ((ArmorArtifactAc.Field1 / 2) >= 3 ? 2 : 0) : 0) + DobjArtAc.Field1;
				}
			}

			DobjArtifact.SetWornByCharacter();

			PrintWorn(DobjArtifact);

			ProcessEvents(EventType.AfterWearArtifact);

			if (GotoCleanup)
			{
				goto Cleanup;
			}

		Cleanup:

			if (NextState == null)
			{
				NextState = Globals.CreateInstance<IMonsterStartState>();
			}
		}

		public WearCommand()
		{
			SortOrder = 240;

			if (Globals.IsRulesetVersion(5))
			{
				IsPlayerEnabled = false;
			}

			Uid = 55;

			Name = "WearCommand";

			Verb = "wear";

			Type = CommandType.Manipulation;
		}
	}
}
