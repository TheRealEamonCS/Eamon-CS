
// MainLoop.cs

// Copyright (c) 2014+ by Michael Penner.  All rights reserved.

using System;
using System.Diagnostics;
using Eamon.Framework.Primitive.Enums;
using Eamon.Game.Attributes;
using EamonRT.Framework;
using static TheVileGrimoireOfJaldial.Game.Plugin.Globals;

namespace TheVileGrimoireOfJaldial.Game
{
	[ClassMappings]
	public class MainLoop : EamonRT.Game.MainLoop, IMainLoop
	{
		public override void Shutdown()
		{
			Debug.Assert(gCharMonster != null);

			gCharMonster.SetInLimbo();

			var leatherBoundBookArtifact = gADB[9];

			Debug.Assert(leatherBoundBookArtifact != null);

			var grimoireArtifact = gADB[27];

			Debug.Assert(grimoireArtifact != null);

			var parchmentArtifact = gADB[33];

			Debug.Assert(parchmentArtifact != null);

			var carryingParchment = parchmentArtifact.IsCarriedByMonster(gCharMonster);

			parchmentArtifact.SetInLimbo();

			gOut.Print("{0}", gEngine.LineSep);

			var hour = gGameState.Day == 0 ? gGameState.Hour - gEngine.StartHour : gGameState.Hour;

			var minute = gGameState.Day == 0 && gGameState.Hour == gEngine.StartHour ? gGameState.Minute - gEngine.StartMinute : gGameState.Minute;

			gOut.Print("After exploring the graveyard for {0} day{1}, {2} hour{3}, and {4} minute{5} you finally head homeward.", gGameState.Day, gGameState.Day != 1 ? "s" : "", hour, hour != 1 ? "s" : "", minute, minute != 1 ? "s" : "");

			if (grimoireArtifact.IsCarriedByMonster(gCharMonster))
			{
				var reward = (gCharacter.GetStat(Stat.Charisma) * grimoireArtifact.Value) / 10;

				gEngine.PrintEffectDesc(162);

				gOut.Print("You hand over the book for inspection.  Kreqor accepts it, almost with a mixed look of wonder and disgust, and begins to leaf through the pages.  He scans the manuscript to determine its condition, but you note he avoids reading its contents.  Finally, he looks up at you and says, \"The Wizard's Council will undoubtedly authorize the release of the reward, which amounts to {0} gold pieces.  But you must have had many adventures in the graveyard, come inside and lodge for the night.  I am curious about what you have seen.\"", reward);

				gEngine.In.KeyPress(gEngine.Buf);

				gOut.Print("{0}", gEngine.LineSep);

				gEngine.PrintEffectDesc(163);

				gOut.Print("The next morning, after breakfast, you re-provision and set out on the final leg of your journey back to the Main Hall.  {0}Kreqor again thanks you profusely for your services and bids you a final farewell.  He watches from his doorstep as you fade into the distance.", carryingParchment ? "Before doing so, you hand over the ancient parchment so it can be returned to its rightful owner.  " : "");

				gOut.Print("You have no encounters of note on the way home.");

				gCharacter.HeldGold += reward;

				grimoireArtifact.SetInLimbo();
			}
			else if (leatherBoundBookArtifact.IsCarriedByMonster(gCharMonster))
			{
				gEngine.PrintEffectDesc(164);

				gEngine.PrintEffectDesc(165);

				gEngine.In.KeyPress(gEngine.Buf);

				gOut.Print("{0}", gEngine.LineSep);

				gEngine.PrintEffectDesc(166);

				gOut.Print("\"Charlatan and swindler!  Do you take me for a doddering fool?\" he splutters.  \"This manual is but a shoddy imitation; I've seen finer magic from a cobbler's apprentice!\"  You note with alarm that his physical frame seems to grow larger in a menacing way.  \"Count yourself lucky the Wizard's Council has a standing rule against transmogrification!  But clearly, you are an amateur, and we have no use for your services.  Good day!\"  In disgust, he {0}ejects you from his residence.", carryingParchment ? "snatches the ancient parchment from your grip and then " : "");

				gOut.Print("You have no encounters of note on the way home.");

				leatherBoundBookArtifact.SetInLimbo();
			}
			else
			{
				gOut.Print("You wander down the path, but give a wide berth to Kreqor's estate, as you don't have the grimoire in your possession.  You carry the long-held suspicion that wizards can be very ill-tempered.  If you appeared at his doorstep empty-handed, he'd probably turn you into something... unnatural.{0}", carryingParchment ? "  However, you do arrange to have the ancient parchment returned to him via courier." : "");

				gOut.Print("The rest of the journey home is uneventful.");
			}

			gOut.Print("Your adventure is over.");

			gEngine.In.KeyPress(gEngine.Buf);

			var torchArtifact = gADB[1];

			Debug.Assert(torchArtifact != null);

			// Scale torch value based on rounds remaining

			torchArtifact.Value = (long)Math.Round((double)torchArtifact.Value * ((double)torchArtifact.LightSource.Field1 / (double)gGameState.TorchRounds));

			var armorArtifact = gGameState.Ar > 0 ? gADB[gGameState.Ar] : null;

			// Crimson cloak (if worn with armor) goes with the player character - move to limbo but let armor bonus remain

			var cloakArtifact = gADB[19];

			Debug.Assert(cloakArtifact != null);

			if (cloakArtifact.IsWornByMonster(gCharMonster))
			{
				var newArmorDesc = armorArtifact != null && armorArtifact.Uid != 19 ? string.Format("{1}{0}{0}{2}", Environment.NewLine, armorArtifact.Desc, cloakArtifact.Desc) : null;

				if (newArmorDesc != null && newArmorDesc.Length <= gEngine.ArtDescLen)
				{
					armorArtifact.Desc = newArmorDesc;
				}

				if (armorArtifact != null && armorArtifact.Uid == 19)
				{
					armorArtifact.Wearable.Field2 = (long)Clothing.ArmorShields;
				}

				cloakArtifact.SetInLimbo();
			}

			// Steel gauntlets (if worn) go with the player character - move to limbo but let skill bonuses remain

			var gauntletsArtifact = gADB[16];

			Debug.Assert(gauntletsArtifact != null);

			if (gauntletsArtifact.IsWornByMonster(gCharMonster))
			{
				var newArmorDesc = armorArtifact != null ? string.Format("{1}{0}{0}{2}", Environment.NewLine, armorArtifact.Desc, gauntletsArtifact.Desc) : null;

				if (newArmorDesc != null && newArmorDesc.Length <= gEngine.ArtDescLen)
				{
					armorArtifact.Desc = newArmorDesc;
				}

				gauntletsArtifact.SetInLimbo();
			}

			base.Shutdown();
		}
	}
}
