﻿
// Room.cs

// Copyright (c) 2014+ by Michael Penner.  All rights reserved.

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using Eamon;
using Eamon.Framework;
using Eamon.Framework.Primitive.Enums;
using Eamon.Game.Attributes;
using static TheVileGrimoireOfJaldial.Game.Plugin.Globals;

namespace TheVileGrimoireOfJaldial.Game
{
	[ClassMappings(typeof(IRoom))]
	public class Room : Eamon.Game.Room, Framework.IRoom
	{
		public override string Name
		{
			get
			{
				var result = base.Name;

				if (gEngine.EnableMutateProperties && Uid == 55)
				{
					result = !gGameState.GetSecretDoor(1) ? "Passageway, Dead-End" : "Secret Passage, Bend";
				}

				return result;
			}

			set
			{
				base.Name = value;
			}
		}

		public override string Desc
		{
			get
			{
				var result = base.Desc;

				if (gEngine.EnableMutateProperties && IsDimLightRoom())
				{
					result = string.Format("Through the {0}, you can vaguely make out your surroundings.", gGameState.IsNightTime() ? "darkness" : "white haze");
				}

				return result;
			}

			set
			{
				base.Desc = value;
			}
		}

		public override bool Seen
		{
			get
			{
				var result = base.Seen;

				if (gEngine.EnableMutateProperties && IsDimLightRoom())
				{
					result = DimLightSeen;
				}

				return result;
			}

			set
			{
				if (gEngine.EnableMutateProperties && IsDimLightRoom())
				{
					DimLightSeen = value;
				}
				else
				{
					base.Seen = value;
				}
			}
		}

		public override LightLevel LightLvl 
		{ 
			get
			{
				var result = base.LightLvl;

				if (gEngine.EnableMutateProperties && result == LightLevel.Dark)
				{
					var willOTheWispMonster = gMDB[10];

					Debug.Assert(willOTheWispMonster != null);

					var efreetiMonster = gMDB[50];

					Debug.Assert(efreetiMonster != null);

					// The Room is lit if either Monster is present

					if (willOTheWispMonster.IsInRoom(this) || efreetiMonster.IsInRoom(this))
					{
						result = LightLevel.Light;
					}
				}

				return result;
			}
			
			set
			{
				base.LightLvl = value;
			}
		}

		public virtual bool DimLightSeen { get; set; }

		public virtual bool Seen02
		{
			get
			{
				return DimLightSeen || base.Seen;
			}
		}

		public override long GetDir(long index)
		{
			if (gEngine.EnableMutateProperties)
			{
				if (Uid == 54)
				{
					return gGameState.GetSecretDoor(1) && index == 1 ? 55 : gGameState.GetSecretDoor(2) && index == 2 ? 56 : base.GetDir(index);
				}
				else if (Uid == 55)
				{
					return gGameState.GetSecretDoor(1) && index == 2 ? 54 : base.GetDir(index);
				}
				else if (Uid == 56)
				{
					return gGameState.GetSecretDoor(2) && index == 1 ? 54 : gGameState.GetSecretDoor(4) && index == 3 ? 68 : base.GetDir(index);
				}
				else if (Uid == 58)
				{
					return gGameState.GetSecretDoor(3) && index == 3 ? 63 : base.GetDir(index);
				}
				else if (Uid == 63)
				{
					return gGameState.GetSecretDoor(3) && index == 4 ? 58 : base.GetDir(index);
				}
				else if (Uid == 68)
				{
					return gGameState.GetSecretDoor(4) && index == 4 ? 56 : base.GetDir(index);
				}
				else if (Uid == 74)
				{
					return gGameState.GetSecretDoor(5) && index == 1 ? 75 : gGameState.GetSecretDoor(6) && index == 2 ? 76 : base.GetDir(index);
				}
				else if (Uid == 87)
				{
					return gGameState.GetSecretDoor(7) && index == 6 ? 90 : base.GetDir(index);
				}
				else if (Uid == 100)
				{
					return gGameState.GetSecretDoor(9) && index == 1 ? 99 : base.GetDir(index);
				}
				else if (Uid == 101)
				{
					return gGameState.GetSecretDoor(8) && index == 4 ? 100 : base.GetDir(index);
				}
				else if (Uid == 102)
				{
					return gGameState.GetSecretDoor(11) && index == 3 ? 105 : base.GetDir(index);
				}
				else if (Uid == 115)
				{
					return gGameState.GetSecretDoor(10) && index == 1 ? 116 : base.GetDir(index);
				}
				else if (Uid == 116)
				{
					var largeFountainArtifact = gADB[24];

					return largeFountainArtifact?.DoorGate != null && index == 6 ? 117 : base.GetDir(index);
				}
				else
				{
					return base.GetDir(index);
				}
			}
			else
			{
				return base.GetDir(index);
			}
		}

		public override bool IsDirectionInObviousExitsList(long index)
		{
			// Suppress secret doors in obvious exits list

			var secretDoorExitDir = (Uid == 54 && (index == 1 || index == 2)) || (Uid == 55 && index == 2) || (Uid == 56 && (index == 1 || index == 3)) ||
				(Uid == 58 && index == 3) || (Uid == 63 && index == 4) || (Uid == 68 && index == 4) || (Uid == 74 && (index == 1 || index == 2)) ||
				(Uid == 87 && index == 6) || (Uid == 100 && index == 1) || (Uid == 101 && index == 4) || (Uid == 102 && index == 3) ||
				(Uid == 115 && index == 1) || (Uid == 116 && index == 6);

			return base.IsDirectionInObviousExitsList(index) && !secretDoorExitDir;
		}

		public override string GetYouAlsoSee(bool showDesc, IList<IMonster> monsterList, IList<IArtifact> artifactList, IList<IGameBase> recordList)
		{
			Debug.Assert(monsterList != null && artifactList != null && recordList != null);

			var plural = recordList.Count > 1 || (monsterList.Count > 0 && monsterList[0].CurrGroupCount > 1) || (artifactList.Count > 0 && artifactList[0].IsPlural);

			return string.Format("{0}You {1}{2}{3}",
					!showDesc ? Environment.NewLine : "",
					showDesc ? "also " : "",
					showDesc && !monsterList.Any() ? "notice " : "see ",
					IsDimLightRoomWithoutGlowingMonsters() && gGameState.Ls <= 0 ? "the dim image" + (plural ? "s" : "") + " of " : "");
		}

		public override RetCode GetExitList(StringBuilder buf, Func<string, string> modFunc = null, bool useNames = true)
		{
			// Use exit direction names or abbreviations based on configuration setting

			return base.GetExitList(buf, modFunc, gGameState.ExitDirNames);
		}

		public virtual bool IsGroundsRoom()
		{
			return (Uid > 0 && Uid < 54) || (Uid > 117 && Uid < 122);
		}

		public virtual bool IsCryptRoom()
		{
			return !IsGroundsRoom() && Type == RoomType.Indoors;
		}

		public virtual bool IsFenceRoom()
		{
			var roomUids = new long[] { 1, 2, 3, 4, 5, 6, 7, 11, 12, 16, 17, 21, 22, 24, 29, 30, 35, 36, 40 };
			
			return roomUids.Contains(Uid);
		}

		public virtual bool IsSwampRoom()
		{
			var roomUids = new long[] { 41, 42, 43, 44, 45 };

			return roomUids.Contains(Uid);
		}

		public virtual bool IsBodyChamberRoom()
		{
			var roomUids = new long[] { 59, 60, 61, 73, 79, 80, 85, 86 };

			return roomUids.Contains(Uid);
		}

		public virtual bool IsRainyRoom()
		{
			return IsGroundsRoom() && gGameState.IsRaining();
		}

		public virtual bool IsFoggyRoom()
		{
			return IsGroundsRoom() && gGameState.IsFoggy();
		}

		public virtual bool IsDimLightRoom()
		{
			return gGameState != null && IsGroundsRoom() && (gGameState.IsNightTime() || (IsFoggyRoom() && GetWeatherIntensity() >= 3));
		}

		public virtual bool IsDimLightRoomWithoutGlowingMonsters()
		{
			var willOTheWispMonster = gMDB[10];

			Debug.Assert(willOTheWispMonster != null);

			var efreetiMonster = gMDB[50];

			Debug.Assert(efreetiMonster != null);

			return IsDimLightRoom() && !willOTheWispMonster.IsInRoom(this) && !efreetiMonster.IsInRoom(this);
		}

		public virtual long GetWeatherIntensity()
		{
			return IsRainyRoom() ? gGameState.WeatherIntensity : IsFoggyRoom() ? gGameState.FoggyRoomWeatherIntensity : 0;
		}
	}
}
