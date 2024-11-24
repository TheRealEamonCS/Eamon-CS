
// PrayCommand.cs

// Copyright (c) 2014+ by Michael Penner.  All rights reserved.

using System.Diagnostics;
using Eamon.Framework;
using Eamon.Framework.Primitive.Enums;
using Eamon.Game.Attributes;
using EamonRT.Framework.States;
using static Dharmaquest.Game.Plugin.Globals;

namespace Dharmaquest.Game.Commands
{
	[ClassMappings]
	public class PrayCommand : EamonRT.Game.Commands.Command, Framework.Commands.IPrayCommand
	{
		public override void ExecuteForPlayer()
		{
			var apolloStatueArtifact = gADB[13];

			Debug.Assert(apolloStatueArtifact != null);

			var poseidonStatueArtifact = gADB[14];

			Debug.Assert(poseidonStatueArtifact != null);

			var aphroditeStatueArtifact = gADB[15];

			Debug.Assert(aphroditeStatueArtifact != null);

			var artemisStatueArtifact = gADB[16];

			Debug.Assert(artemisStatueArtifact != null);

			var rl = gEngine.RollDice(1, 100, 0);

			if (ActorRoom.Uid == 25)
			{
				if (poseidonStatueArtifact.IsInRoom(ActorRoom))
				{
					if (rl > 50)
					{
						HandlePrayerAnswered(39);
					}
					else
					{
						HandlePrayerIgnored(poseidonStatueArtifact);
					}
				}
				else
				{
					HandleNoGodPresent();
				}

				goto Cleanup;
			}

			if (ActorRoom.Uid == 26)
			{
				if (apolloStatueArtifact.IsInRoom(ActorRoom))
				{
					if (rl > 50)
					{
						HandlePrayerAnswered(45);
					}
					else
					{
						HandlePrayerIgnored(apolloStatueArtifact);
					}
				}
				else
				{
					HandleNoGodPresent();
				}

				goto Cleanup;
			}

			if (ActorRoom.Uid == 27)
			{
				if (aphroditeStatueArtifact.IsInRoom(ActorRoom))
				{
					if (rl > 50)
					{
						HandlePrayerAnswered(63);
					}
					else
					{
						HandlePrayerIgnored(aphroditeStatueArtifact);
					}
				}
				else
				{
					HandleNoGodPresent();
				}

				goto Cleanup;
			}

			if (ActorRoom.Uid == 28)
			{
				if (artemisStatueArtifact.IsInRoom(ActorRoom))
				{
					if (rl > 50)
					{
						HandlePrayerAnswered(52);
					}
					else
					{
						HandlePrayerIgnored(artemisStatueArtifact);
					}
				}
				else
				{
					HandleNoGodPresent();
				}

				goto Cleanup;
			}

			if (ActorRoom.Uid == 68)
			{
				/*
					The gods are more frugal in this version for balance sake (but still *very* generous)

					1. All Stats boosted by 1
					2. All known Spells boosted by 5
					3. All Weapon skills boosted by 5
					4. Armor Expertise boosted by 5
					5. 1000 gp payout
				*/

				gEngine.ResetMonsterStats(ActorMonster);

				gOut.Print("Zeus smiles at you and says, \"{0}, you are a brave and noble warrior. Let the ceremony begin!\"", gCharacter.Name);

				gEngine.In.KeyPress(gEngine.Buf);

				gOut.Print("{0}", gEngine.LineSep);

				// TODO: use gEngine.GetStat, gEngine.GetSpellAbility, gEngine.GetWeaponAbility to enforce maximum values ???

				gCharacter.ModStat(Stat.Intellect, 1);						// 5

				if (gCharacter.GetStat(Stat.Intellect) > 24)
				{
					gCharacter.SetStat(Stat.Intellect, 24);
				}

				gOut.Print("Artemis raises your Intellect to {0}.", gCharacter.GetStat(Stat.Intellect));

				ActorMonster.Hardiness += 1;								// 5

				if (ActorMonster.Hardiness > 25)
				{
					ActorMonster.Hardiness = 25;
				}

				gOut.Print("Poseidon raises your Hardiness to {0}.", ActorMonster.Hardiness);

				ActorMonster.Agility += 1;									// 5

				if (ActorMonster.Agility > 25)
				{
					ActorMonster.Agility = 25;
				}

				gOut.Print("Hermes raises your Agility to {0}.", ActorMonster.Agility);

				gCharacter.ModStat(Stat.Charisma, 1);						// 5

				if (gCharacter.GetStat(Stat.Charisma) > 24)
				{
					gCharacter.SetStat(Stat.Charisma, 24);
				}

				gOut.Print("Aphrodite raises your Charisma to {0}.", gCharacter.GetStat(Stat.Charisma));

				if (gCharacter.GetSpellAbility(Spell.Blast) > 0)
				{
					gCharacter.ModSpellAbility(Spell.Blast, 5);             // 10

					if (gCharacter.GetSpellAbility(Spell.Blast) > 100)
					{
						gCharacter.SetSpellAbility(Spell.Blast, 100);
					}
				}

				if (gCharacter.GetSpellAbility(Spell.Heal) > 0)
				{
					gCharacter.ModSpellAbility(Spell.Heal, 5);              // 10

					if (gCharacter.GetSpellAbility(Spell.Heal) > 100)
					{
						gCharacter.SetSpellAbility(Spell.Heal, 100);
					}
				}

				if (gCharacter.GetSpellAbility(Spell.Blast) > 0 || gCharacter.GetSpellAbility(Spell.Heal) > 0)
				{
					gOut.Print("Apollo raises your spell abilities.{0}{1}", 
						gCharacter.GetSpellAbility(Spell.Blast) > 0 ? string.Format(" Blast is {0}%.", gCharacter.GetSpellAbility(Spell.Blast)) : "", 
						gCharacter.GetSpellAbility(Spell.Heal) > 0 ? string.Format(" Heal is {0}%.", gCharacter.GetSpellAbility(Spell.Heal)) : "");
				}

				if (gCharacter.GetSpellAbility(Spell.Speed) > 0)
				{
					gCharacter.ModSpellAbility(Spell.Speed, 5);             // 10

					if (gCharacter.GetSpellAbility(Spell.Speed) > 100)
					{
						gCharacter.SetSpellAbility(Spell.Speed, 100);
					}
				}

				if (gCharacter.GetSpellAbility(Spell.Power) > 0)
				{
					gCharacter.ModSpellAbility(Spell.Power, 5);             // 10

					if (gCharacter.GetSpellAbility(Spell.Power) > 100)
					{
						gCharacter.SetSpellAbility(Spell.Power, 100);
					}
				}

				if (gCharacter.GetSpellAbility(Spell.Speed) > 0 || gCharacter.GetSpellAbility(Spell.Power) > 0)
				{
					gOut.Print("Hera raises your other spells.{0}{1}",
						gCharacter.GetSpellAbility(Spell.Speed) > 0 ? string.Format(" Speed is {0}%.", gCharacter.GetSpellAbility(Spell.Speed)) : "",
						gCharacter.GetSpellAbility(Spell.Power) > 0 ? string.Format(" Power is {0}%.", gCharacter.GetSpellAbility(Spell.Power)) : "");
				}

				gCharacter.ModWeaponAbility(Weapon.Axe, 5);                 // 10

				if (gCharacter.GetWeaponAbility(Weapon.Axe) > 100)
				{
					gCharacter.SetWeaponAbility(Weapon.Axe, 100);
				}

				gCharacter.ModWeaponAbility(Weapon.Bow, 5);                 // 10

				if (gCharacter.GetWeaponAbility(Weapon.Bow) > 100)
				{
					gCharacter.SetWeaponAbility(Weapon.Bow, 100);
				}

				gOut.Print("Athena raises your weapons ability. Axe is {0}%. Bow is {1}%.", gCharacter.GetWeaponAbility(Weapon.Axe), gCharacter.GetWeaponAbility(Weapon.Bow));

				gCharacter.ModWeaponAbility(Weapon.Club, 5);                // 10

				if (gCharacter.GetWeaponAbility(Weapon.Club) > 100)
				{
					gCharacter.SetWeaponAbility(Weapon.Club, 100);
				}

				gCharacter.ModWeaponAbility(Weapon.Spear, 5);               // 10

				if (gCharacter.GetWeaponAbility(Weapon.Spear) > 100)
				{
					gCharacter.SetWeaponAbility(Weapon.Spear, 100);
				}

				gOut.Print("Hephaestus gives you Club ability of {0}%. Spear ability of {1}%.", gCharacter.GetWeaponAbility(Weapon.Club), gCharacter.GetWeaponAbility(Weapon.Spear));

				gCharacter.ModWeaponAbility(Weapon.Sword, 5);               // 10

				if (gCharacter.GetWeaponAbility(Weapon.Sword) > 100)
				{
					gCharacter.SetWeaponAbility(Weapon.Sword, 100);
				}

				gCharacter.ArmorExpertise += 5;								// 10

				if (gCharacter.ArmorExpertise > 100)
				{
					gCharacter.ArmorExpertise = 100;
				}

				gOut.Print("Ares raises your Sword ability to {0}%. Your Armour Experience is now {1}%.", gCharacter.GetWeaponAbility(Weapon.Sword), gCharacter.ArmorExpertise);

				gCharacter.HeldGold += 1000;

				gOut.Print("Zeus increases your gold to {0} gold pieces.", gCharacter.HeldGold);

				gOut.Print("He smiles once more and bids you farewell.");

				gEngine.In.KeyPress(gEngine.Buf);

				gOut.Print("{0}", gEngine.LineSep);

				PrintRideOffIntoSunset();

				gEngine.ExitType = ExitType.FinishAdventure;

				gEngine.MainLoop.ShouldShutdown = true;

				GotoCleanup = true;

				goto Cleanup;
			}

			gOut.Print("This is not a temple of Dharma.");

			NextState = gEngine.CreateInstance<IStartState>();

		Cleanup:

			if (NextState == null)
			{
				NextState = gEngine.CreateInstance<IMonsterStartState>();
			}
		}

		public virtual void HandlePrayerAnswered(long roomUid)
		{
			Debug.Assert(roomUid > 0);

			gEngine.PrintEffectDesc(15);

			gGameState.R2 = roomUid;

			NextState = gEngine.CreateInstance<IAfterPlayerMoveState>();
		}

		public virtual void HandlePrayerIgnored(IArtifact statueArtifact)
		{
			Debug.Assert(statueArtifact != null);

			gOut.Print("The god has ignored your prayer.");

			gOut.Print("{0} vanishes!", statueArtifact.GetTheName(true));

			statueArtifact.SetInLimbo();
		}

		public virtual void HandleNoGodPresent()
		{
			gOut.Print("The god no longer inhabits this temple.");

			NextState = gEngine.CreateInstance<IStartState>();
		}

		public PrayCommand()
		{
			SortOrder = 440;

			IsNew = true;

			Name = "PrayCommand";

			Verb = "pray";

			Type = CommandType.Miscellaneous;
		}
	}
}
