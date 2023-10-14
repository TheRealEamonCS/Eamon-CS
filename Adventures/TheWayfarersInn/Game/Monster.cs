
// Monster.cs

// Copyright (c) 2014+ by Michael Penner.  All rights reserved.

using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Eamon.Framework;
using Eamon.Framework.Primitive.Enums;
using Eamon.Game.Attributes;
using static TheWayfarersInn.Game.Plugin.Globals;

namespace TheWayfarersInn.Game
{
	[ClassMappings]
	public class Monster : Eamon.Game.Monster, IMonster
	{
		public override string StateDesc
		{
			get
			{
				var result = base.StateDesc;

				// Blue-banded centipedes

				if (gEngine.EnableMutateProperties && Uid == 3)
				{
					if (CurrGroupCount > 5)
					{
						result += " crawling on the walls, floor, and ceiling";
					}

					if (gGameState.AttackingCentipedeCounter > 0)
					{
						result += string.Format(" ({0} {1} swarming)", gEngine.GetStringFromNumber(gGameState.AttackingCentipedeCounter, false, gEngine.Buf01), gGameState.AttackingCentipedeCounter > 1 ? "are" : "is");
					}
				}

				return result;
			}

			set
			{
				base.StateDesc = value;
			}
		}

		public override long Location 
		{ 
			get
			{
				var result = base.Location;

				if (gEngine.EnableMutateProperties)
				{
					// Unseen apparition

					if (Uid == 2 && !gGameState.CharlotteDeathSeen)
					{
						var roomUids = new long[] { 37, 38, 39, 66 };

						if (roomUids.Contains(gGameState.Ro))
						{
							result = gGameState.Ro;
						}
					}

					// Charlotte

					else if (Uid == 4 && gGameState.CharlotteDeathSeen)
					{
						var roomUids = new long[] { 34 };

						if (roomUids.Contains(gGameState.Ro))
						{
							result = gGameState.Ro;
						}
					}
				}

				return result;
			}

			set
			{
				base.Location = value;
			}
		}

		public override bool HasWornInventory()
		{
			// Charlotte has no worn inventory list

			return Uid != 4 ? base.HasWornInventory() : false;
		}

		public override bool HasCarriedInventory()
		{
			// Charlotte has no carried inventory list

			return Uid != 4 ? base.HasCarriedInventory() : false;
		}

		public override bool HasHumanNaturalAttackDescs()
		{
			var monsterUids = new long[] { 8, 24 };

			// Use appropriate natural attack descriptions for humans

			return monsterUids.Contains(Uid) || base.HasHumanNaturalAttackDescs();
		}

		public override bool IsAttackable(IMonster monster)
		{
			var monsterUids = new long[] { 4 };

			// Various monsters can't be attacked

			return !monsterUids.Contains(Uid) ? base.IsAttackable(monster) : false;
		}

		public override bool CanMoveToRoomUid(long roomUid, bool fleeing)
		{
			if (gEngine.WanderRoomUids[Uid] == null || gEngine.WanderRoomUids[Uid].Length > 1)
			{
				// Unseen apparition / Charlotte

				if (Uid == 2 || Uid == 4)
				{
					var roomUids = new List<long> { 9, 10, 11, 12, 13, 42 };

					if (Uid == 4)
					{
						roomUids.Add(38);
					}

					return !roomUids.Contains(roomUid) ? base.CanMoveToRoomUid(roomUid, fleeing) : false;
				}

				// Dire wolves defend the pups

				else if (Uid == 7)
				{
					var direWolfPupsArtifact = gADB[117];

					Debug.Assert(direWolfPupsArtifact != null);

					var direWolvesRoom = GetInRoom();

					var direWolfPupRoom = direWolfPupsArtifact.GetInRoom(true) ?? direWolfPupsArtifact.GetEmbeddedInRoom(true);

					var direWolfPupMonster = direWolfPupsArtifact.GetCarriedByMonster(true);

					var direWolfPupsCarried = direWolfPupsArtifact.IsCarriedByMonster(gCharMonster, true) || (direWolfPupMonster != null && direWolfPupMonster.Reaction == Friendliness.Friend);

					return roomUid != 67 && (direWolvesRoom?.Uid != direWolfPupRoom?.Uid || direWolfPupsCarried) ? base.CanMoveToRoomUid(roomUid, fleeing) : false;
				}

				// Restrict movement if necessary

				else
				{
					return gEngine.WanderRoomUids[Uid] == null || roomUid == 0 || gEngine.WanderRoomUids[Uid].Contains(roomUid) ? base.CanMoveToRoomUid(roomUid, fleeing) : false;
				}
			}
			else
			{
				// Restrict movement if necessary

				return false;
			}
		}

		public override bool ShouldShowHealthStatusWhenExamined()
		{
			var monsterUids = new long[] { 2, 4 };

			// Unseen apparition / Charlotte

			return !monsterUids.Contains(Uid) ? base.ShouldShowHealthStatusWhenExamined() : false;
		}

		public override bool ShouldRefuseToAcceptGift(IArtifact artifact)
		{
			var artifactUids = new long[] { 30, 43, 52, 54, 55, 87, 93, 102, 110 };

			Debug.Assert(artifact != null);

			// Charlotte accepts a few gifts

			return Uid != 4 || !artifactUids.Contains(artifact.Uid) ? base.ShouldRefuseToAcceptGift(artifact) : false;
		}

		public override bool ShouldPreferNaturalWeaponsToWeakerWeapon(IArtifact artifact)
		{
			// Nolan

			return Uid != 24 /* || !gEngine.NolanAttacksBlackPudding */ ? base.ShouldPreferNaturalWeaponsToWeakerWeapon(artifact) : false;         // TODO: implement gEngine.NolanAttacksBlackPudding
		}

		public override long GetMaxMemberActionCount()
		{
			// Blue-banded centipedes

			return gGameState != null && Uid == 3 ? gGameState.AttackingCentipedeCounter : base.GetMaxMemberActionCount();
		}

		public override string[] GetWeaponAttackDescs(IArtifact artifact)
		{
			// Machete

			return artifact != null && artifact.Uid == 152 ? new string[] { "swing{0} at", "chop{0} at", "hack{0} at" } : base.GetWeaponAttackDescs(artifact);
		}

		public override string[] GetNaturalAttackDescs()
		{
			// Black pudding

			return Uid == 1 ? new string[] { "lunge{0} at", "engulf{0}", "strike{0} at" } :

				// Blue-banded centipedes

				Uid == 3 ? new string[] { "lunge{0} at", "bite{0} at", "strike{0} at" } :

				// Dire wolves

				Uid == 7 ? new string[] { "lunge{0} at", "bite{0} at", "claw{0} at", "pounce{0} on" } :

				// Giant yellow jackets

				Uid == 9 ? new string[] { "sting{0}", "jab{0} at" } :

				// Giant bombardier beetles

				Uid == 10 ? new string[] { "spray{0} caustic acid at", "bite{0} at" } :

				// Giant fire beetles

				Uid == 11 ? new string[] { "spray{0} fiery resin at", "bite{0} at" } :

				// Giant brown recluses

				Uid == 12 ? new string[] { "lunge{0} at", "bite{0} at", "pounce{0} on" } :

				// Giant black widows

				Uid == 13 ? new string[] { "lunge{0} at", "bite{0} at", "grapple{0} with" } :

				// Violet fungus

				Uid == 14 ? new string[] { "expel{0} toxic spores at", "grapple{0} with" } :

				// Witherbloom

				Uid == 18 ? new string[] { "drain{0} life force from" } :

				// Assassin vines

				Uid == 19 ? new string[] { "constrict{0} around", "whip{0} at", "grapple{0} with" } :

				// Rust monster

				Uid == 20 ? new string[] { "charge{0} at", "bite{0} at" } :

				// Peryton

				Uid == 21 ? new string[] { "charge{0} at", "snap{0} at", "gore{0} at" } :

				// Harpy

				Uid == 22 ? new string[] { "dive{0} at", "claw{0} at", "grasp{0} at" } :

				// Hearthwatcher

				Uid == 23 ? (gEngine.RollDice(1, 100, 0) < 51 ? base.GetNaturalAttackDescs() : new string[] { "charge{0} at", "bite{0} at", "grapple{0} with" }) :

				base.GetNaturalAttackDescs();
		}

		public override string GetArmorDescString()
		{
			var armorDesc = base.GetArmorDescString();

			if (IsInRoomLit())
			{
				// Black pudding

				if (Uid == 1)
				{
					armorDesc = "its gelatinous form";
				}

				// Blue-banded centipedes

				else if (Uid == 3)
				{
					armorDesc = "its chitinous carapace";
				}

				// Dire wolves / Hearthwatcher

				else if (Uid == 7 || Uid == 23)
				{
					armorDesc = "its coarse fur";
				}

				// Giant yellow jackets / Rust monster

				else if (Uid == 9 || Uid == 20)
				{
					armorDesc = "its sturdy exoskeleton";
				}

				// Giant bombardier beetles / Giant fire beetles

				else if (Uid == 10 || Uid == 11)
				{
					armorDesc = "its rugged exoskeleton";
				}

				// Assassin vines

				else if (Uid == 19)
				{
					armorDesc = "its tough bark";
				}

				// Peryton / Harpy

				else if (Uid == 21 || Uid == 22)
				{
					armorDesc = gEngine.RollDice(1, 100, 0) < 51 ? "its tough hide" : "its thick feathers";
				}
			}

			return armorDesc;
		}
	}
}



