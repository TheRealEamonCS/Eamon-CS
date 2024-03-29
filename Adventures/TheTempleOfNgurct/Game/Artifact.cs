﻿
// Artifact.cs

// Copyright (c) 2014+ by Michael Penner.  All rights reserved.

using Eamon.Framework;
using Eamon.Framework.Primitive.Classes;
using Eamon.Game.Attributes;
using static TheTempleOfNgurct.Game.Plugin.Globals;

namespace TheTempleOfNgurct.Game
{
	[ClassMappings]
	public class Artifact : Eamon.Game.Artifact, IArtifact
	{
		public override bool IsReadyableByMonsterUid(long monsterUid)
		{
			// Only player can wield fireball wand

			return Uid != 63 || monsterUid == gCharMonster.Uid ? base.IsReadyableByMonsterUid(monsterUid) : false;
		}

		public override bool IsAttackable(ref IArtifactCategory ac)
		{
			// Statue is attackable

			if (Uid == 50)
			{
				ac = GetCategory(0);

				return true;
			}
			else
			{
				return base.IsAttackable(ref ac);
			}
		}

		public override bool ShouldAllowBlastSkillGains()
		{
			// Statue allows Blast skill gains

			return Uid == 50 || base.ShouldAllowBlastSkillGains();
		}
	}
}
