
// Monster.cs

// Copyright (c) 2014+ by Michael Penner.  All rights reserved.

using System.Linq;
using Eamon.Framework;
using Eamon.Framework.Primitive.Enums;
using Eamon.Game.Attributes;
using Eamon.Game.Extensions;
using static OrbOfMyLife.Game.Plugin.Globals;

namespace OrbOfMyLife.Game
{
	[ClassMappings]
	public class Monster : Eamon.Game.Monster, IMonster
	{
		public override bool HasWornInventory()
		{
			var monsterUids = new long[] { 2, 14, 16, 17, 21 };		// Negative set

			// Some monsters have no worn inventory list

			return Uid == gGameState?.Cm || !monsterUids.Contains(Uid) ? base.HasWornInventory() : false;
		}

		public override bool HasCarriedInventory()
		{
			var monsterUids = new long[] { 2, 14, 16, 17, 21 };     // Negative set

			// Some monsters have no carried inventory list

			return Uid == gGameState?.Cm || !monsterUids.Contains(Uid) ? base.HasCarriedInventory() : false;
		}

		public override bool HasHumanNaturalAttackDescs()
		{
			var monsterUids = new long[] { 3, 4, 7, 8, 15, 18, 19, 20, 23, 24, 26 };        // Positive set

			// Use appropriate natural attack descriptions for humans

			return monsterUids.Contains(Uid) || base.HasHumanNaturalAttackDescs();
		}

		public override bool IsAttackable(IMonster monster)
		{
			var monsterUids = new long[] { 22 };

			// Various monsters can't be attacked

			return !monsterUids.Contains(Uid) ? base.IsAttackable(monster) : false;
		}

		public override bool CanMoveToRoomUid(long roomUid, bool fleeing)
		{
			var result = true;

			var room = gRDB[roomUid] as Framework.IRoom;

			switch(Uid)
			{
				case 1:			// Darrk Ness

					result = (room == null || room.IsDreamDimensionRoom()) && StateDesc.Length <= 0 ? base.CanMoveToRoomUid(roomUid, fleeing) : false;

					break;

				case 2:         // Hero
				case 3:         // Blind priest
				case 4:         // High Priest
				case 13:        // Tolor
				case 15:        // Priest
				case 19:        // Harold the Dup
				case 20:        // Ferdinand
				case 23:        // Falconer
				case 26:        // Centurion
				case 27:        // Broglia

					result = roomUid != 34 && (Reaction == Friendliness.Friend || room == null || !room.IsDreamDimensionRoom()) ? base.CanMoveToRoomUid(roomUid, fleeing) : false;

					break;

				case 5:         // Siren Witch
				case 6:         // Elders
				case 7:         // Hogard
				case 8:         // Jolard
				case 9:         // Prince of Darkness
				case 10:        // Yazarik
				case 11:        // Zylumik
				case 12:        // Demon of Trowsk
				case 18:        // Tobias
				case 25:		// Demon

					result = room == null || room.IsDreamDimensionRoom() ? base.CanMoveToRoomUid(roomUid, fleeing) : false;

					break;

				case 14:        // Pike
				{
					var roomUids = new long[] { 15, 16, 17, 18, 19, 20, 21, 43 };

					result = roomUids.Contains(roomUid) ? base.CanMoveToRoomUid(roomUid, fleeing) : false;

					break;
				}

				case 21:		// Serpent
				{
					var roomUids = new long[] { 13, 14, 15, 16, 17, 18, 19, 20, 21, 43 };

					result = roomUids.Contains(roomUid) ? base.CanMoveToRoomUid(roomUid, fleeing) : false;

					break;
				}

				case 16:        // Squirrel
				case 17:        // Cougar
				case 24:        // Hobo
				{
					var roomUids = new long[] { 3, 12, 35 };

					result = roomUids.Contains(roomUid) ? base.CanMoveToRoomUid(roomUid, fleeing) : false;

					break;
				}

				case 22:        // Sagonne

					result = false;

					break;
			}

			return result;
		}

		public override bool ShouldCheckToAttackNonEnemy()
		{
			// Unconscious Darrk Ness needs no confirmation

			return Uid != 1 || StateDesc.Length <= 0 ? base.ShouldCheckToAttackNonEnemy() : false;
		}

		public override bool ShouldProcessInGameLoop()
		{
			// Suppress Darrk Ness when unconscious

			return Uid != 1 || StateDesc.Length <= 0 ? base.ShouldProcessInGameLoop() : false;
		}
	}
}
