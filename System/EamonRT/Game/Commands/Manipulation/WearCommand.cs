
// WearCommand.cs

// Copyright (c) 2014+ by Michael Penner.  All rights reserved.

using System.Diagnostics;
using System.Linq;
using Eamon.Framework;
using Eamon.Framework.Primitive.Classes;
using Eamon.Framework.Primitive.Enums;
using Eamon.Game.Attributes;
using EamonRT.Framework.Commands;
using EamonRT.Framework.Primitive.Enums;
using EamonRT.Framework.States;
using static EamonRT.Game.Plugin.Globals;

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

		public override void ExecuteForPlayer()
		{
			Debug.Assert(DobjArtifact != null);

			DobjArtAc = DobjArtifact.Wearable;

			if (DobjArtAc == null)
			{
				PrintCantVerbObj(DobjArtifact);

				NextState = gEngine.CreateInstance<IStartState>();

				goto Cleanup;
			}

			if (DobjArtifact.IsWornByMonster(ActorMonster))
			{
				PrintAlreadyWearingObj(DobjArtifact);

				NextState = gEngine.CreateInstance<IStartState>();

				goto Cleanup;
			}

			if (!DobjArtifact.IsCarriedByMonster(ActorMonster))
			{
				if (!ShouldAllowRedirectToGetCommand())
				{
					PrintDontHaveIt02(DobjArtifact);

					NextState = gEngine.CreateInstance<IStartState>();
				}
				else if (!GetCommandCalled)
				{
					RedirectToGetCommand<IWearCommand>(DobjArtifact);
				}
				else if (DobjArtifact.DisguisedMonster == null)
				{
					NextState = gEngine.CreateInstance<IStartState>();
				}

				goto Cleanup;
			}

			ArmorArtifact = gADB[gGameState.Ar];

			ShieldArtifact = gADB[gGameState.Sh];

			WeaponArtifact = ActorMonster.Weapon > 0 ? gADB[ActorMonster.Weapon] : null;

			ArmorArtifactAc = ArmorArtifact != null ? ArmorArtifact.Wearable : null;

			ShieldArtifactAc = ShieldArtifact != null ? ShieldArtifact.Wearable : null;

			WeaponArtifactAc = WeaponArtifact != null ? WeaponArtifact.GeneralWeapon : null;

			if (DobjArtAc.Field1 > 1 && ArmorArtifactAc != null)
			{
				PrintAlreadyWearingArmor();

				if (gEngine.LastCommandList.FirstOrDefault(c => c is IGetCommand) == null)
				{
					NextState = gEngine.CreateInstance<IStartState>();
				}

				goto Cleanup;
			}

			if (DobjArtAc.Field1 == 1 && ShieldArtifactAc != null)
			{
				PrintAlreadyWearingShield();

				if (gEngine.LastCommandList.FirstOrDefault(c => c is IGetCommand) == null)
				{
					NextState = gEngine.CreateInstance<IStartState>();
				}

				goto Cleanup;
			}

			// Can't wear shield while using two-handed weapon

			if (DobjArtAc.Field1 == 1 && WeaponArtifactAc != null && WeaponArtifactAc.Field5 > 1)
			{
				PrintCantWearShieldWithWeapon(DobjArtifact, WeaponArtifact);

				if (gEngine.LastCommandList.FirstOrDefault(c => c is IGetCommand) == null)
				{
					NextState = gEngine.CreateInstance<IStartState>();
				}

				goto Cleanup;
			}

			ProcessEvents(EventType.BeforeWearArtifact);

			if (GotoCleanup)
			{
				goto Cleanup;
			}

			if (DobjArtAc.Field1 > 1)
			{
				if (DobjArtAc.Field1 > 14)
				{
					DobjArtAc.Field1 = 14;
				}

				gGameState.Ar = DobjArtifact.Uid;

				ActorMonster.Armor = (DobjArtAc.Field1 / 2) + ((DobjArtAc.Field1 / 2) >= 3 ? 2 : 0) + (ShieldArtifactAc != null ? ShieldArtifactAc.Field1 : 0);
			}
			
			if (DobjArtAc.Field1 == 1)
			{
				gGameState.Sh = DobjArtifact.Uid;

				ActorMonster.Armor = (ArmorArtifactAc != null ? (ArmorArtifactAc.Field1 / 2) + ((ArmorArtifactAc.Field1 / 2) >= 3 ? 2 : 0) : 0) + DobjArtAc.Field1;
			}

			DobjArtifact.SetWornByMonster(ActorMonster);

			PrintWorn(DobjArtifact);

			ProcessEvents(EventType.AfterWearArtifact);

			if (GotoCleanup)
			{
				goto Cleanup;
			}

		Cleanup:

			if (NextState == null)
			{
				NextState = gEngine.CreateInstance<IMonsterStartState>();
			}
		}

		public WearCommand()
		{
			SortOrder = 240;

			if (gEngine.IsRulesetVersion(5, 62))
			{
				IsPlayerEnabled = false;
			}

			Name = "WearCommand";

			Verb = "wear";

			Type = CommandType.Manipulation;
		}
	}
}
