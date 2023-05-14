
// PowerCommand.cs

// Copyright (c) 2014+ by Michael Penner.  All rights reserved.

using System;
using System.Collections.Generic;
using System.Linq;
using Eamon.Framework;
using Eamon.Framework.Primitive.Enums;
using Eamon.Game.Attributes;
using EamonRT.Framework.Commands;
using static TheVileGrimoireOfJaldial.Game.Plugin.Globals;

namespace TheVileGrimoireOfJaldial.Game.Commands
{
	[ClassMappings]
	public class PowerCommand : EamonRT.Game.Commands.PowerCommand, IPowerCommand
	{
		public virtual IList<long> FriendDeadBodyUidList { get; set; }

		public PowerCommand()
		{
			FriendDeadBodyUidList = gEngine.GetMonsterList(m => m.Reaction == Friendliness.Friend && !m.IsCharacterMonster()).Select(m => m.DeadBody).ToList();

			// Can't resurrect dead friends

			ResurrectWhereClauseFuncs = new Func<IArtifact, bool>[]
			{
				a => (a.IsCarriedByMonsterUid(gGameState.Cm) || a.IsInRoomUid(gGameState.Ro)) && a.DeadBody != null && !FriendDeadBodyUidList.Contains(a.Uid)
			};

			// Can't make dead friends vanish

			VanishWhereClauseFuncs = new Func<IArtifact, bool>[]
			{
				a => a.IsInRoomUid(gGameState.Ro) && !a.IsUnmovable() && !FriendDeadBodyUidList.Contains(a.Uid)
			};
		}
	}
}
