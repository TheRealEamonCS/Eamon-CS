﻿
// Artifact.cs

// Copyright (c) 2014+ by Michael Penner.  All rights reserved.

using System.Text;
using Eamon;
using Eamon.Framework;
using Eamon.Framework.Primitive.Classes;
using Eamon.Game.Attributes;
using static TheVileGrimoireOfJaldial.Game.Plugin.Globals;

namespace TheVileGrimoireOfJaldial.Game
{
	[ClassMappings(typeof(IArtifact))]
	public class Artifact : Eamon.Game.Artifact, Framework.IArtifact
	{
		public override string Desc
		{
			get
			{
				var result = base.Desc;

				var room = GetInRoom(true) as Framework.IRoom;

				if (gEngine.EnableMutateProperties && room != null && room.Uid == gGameState.Ro && room.IsDimLightRoomWithoutGlowingMonsters() && gGameState.Ls <= 0)
				{
					result = string.Format("You can vaguely make out {0} in the {1}.", GetTheName(), gGameState.IsNightTime() ? "darkness" : "white haze");
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

				var room = GetInRoom(true) as Framework.IRoom;

				if (gEngine.EnableMutateProperties && room != null && room.Uid == gGameState.Ro && room.IsDimLightRoomWithoutGlowingMonsters() && gGameState.Ls <= 0 && !IsCharOwned && !IsDecoration())
				{
					result = DimLightSeen;
				}

				return result;
			}

			set
			{
				var room = GetInRoom(true) as Framework.IRoom;

				if (gEngine.EnableMutateProperties && room != null && room.Uid == gGameState.Ro && room.IsDimLightRoomWithoutGlowingMonsters() && gGameState.Ls <= 0 && !IsCharOwned && !IsDecoration())
				{
					DimLightSeen = value;
				}
				else
				{
					base.Seen = value;
				}
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

		public override RetCode BuildPrintedFullDesc(StringBuilder buf, bool showName, bool showVerboseName)
		{
			var result = base.BuildPrintedFullDesc(buf, showName, showVerboseName);

			// Reset solitary tombstone's Desc value after initial viewing

			if (Uid == 10 && buf != null && buf.ToString().Contains("You glimpse a solitary tombstone"))
			{
				Desc = "This tombstone is very old, possibly several hundred years.  Why is it all by itself, you pause to wonder?";
			}

			return result;
		}

		public override bool IsAttackable(ref IArtifactCategory ac)
		{
			bool result;

			gEngine.PushRulesetVersion(0);

			result = base.IsAttackable(ref ac);

			gEngine.PopRulesetVersion();

			return result;
		}

		public virtual bool IsDecoration()
		{
			return Uid == 41 || Uid == 42;
		}

		public virtual long GetLeverageBonus()
		{
			return Uid == 7 ? 5 : Uid == 28 ? 9 : GeneralWeapon != null && GeneralWeapon.Field2 > 3 ? GeneralWeapon.Field2 : 0;
		}
	}
}
