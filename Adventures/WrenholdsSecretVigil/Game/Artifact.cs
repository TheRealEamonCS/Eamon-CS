
// Artifact.cs

// Copyright (c) 2014+ by Michael Penner.  All rights reserved.

using System.Diagnostics;
using System.Linq;
using Eamon.Framework;
using Eamon.Framework.Primitive.Classes;
using Eamon.Game.Attributes;

namespace WrenholdsSecretVigil.Game
{
	[ClassMappings(typeof(IArtifact))]
	public class Artifact : Eamon.Game.Artifact, Framework.IArtifact
	{
		public override bool IsReadyableByCharacter()
		{
			// Only one-eyed ogre can wield large tree limb

			return Uid != 7 && base.IsReadyableByCharacter();
		}

		public override bool IsReadyableByMonsterUid(long monsterUid)
		{
			// Only one-eyed ogre can wield large tree limb

			return Uid != 7 || monsterUid == 3 ? base.IsReadyableByMonsterUid(monsterUid) : false;
		}

		public override bool IsAttackable()
		{
			var artifactUids = new long[] { 24, 25 };

			// Slime is attackable

			return artifactUids.Contains(Uid) || base.IsAttackable();
		}

		public override bool IsAttackable01(ref IArtifactCategory ac)
		{
			var artifactUids = new long[] { 24, 25 };

			// Slime is attackable

			if (artifactUids.Contains(Uid))
			{
				ac = GetCategories(0);

				return true;
			}
			else
			{
				return base.IsAttackable01(ref ac);
			}
		}

		public override string GetBrokenDesc()
		{
			// Swallower shark

			return Uid == 31 ? "(mangled)" : base.GetBrokenDesc();
		}

		public virtual bool IsBuriedInRoomUid(long roomUid)
		{
			return Location == (roomUid + 7000);
		}

		public virtual bool IsBuriedInRoom(IRoom room)
		{
			Debug.Assert(room != null);

			return IsBuriedInRoomUid(room.Uid);
		}
	}
}
