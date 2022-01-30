
// Artifact.cs

// Copyright (c) 2014+ by Michael Penner.  All rights reserved.

using Eamon.Framework;
using Eamon.Framework.Primitive.Classes;
using Eamon.Game.Attributes;

namespace TheTempleOfNgurct.Game
{
	[ClassMappings]
	public class Artifact : Eamon.Game.Artifact, IArtifact
	{
		public override bool IsReadyableByMonsterUid(long monsterUid)
		{
			// Only player can wield fireball wand

			return Uid != 63 && base.IsReadyableByMonsterUid(monsterUid);
		}

		public override bool IsAttackable01(ref IArtifactCategory ac)
		{
			// Statue is attackable

			if (Uid == 50)
			{
				ac = GetCategories(0);

				return true;
			}
			else
			{
				return base.IsAttackable01(ref ac);
			}
		}
	}
}
