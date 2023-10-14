
// Artifact.cs

// Copyright (c) 2014+ by Michael Penner.  All rights reserved.

using System.Linq;
using Eamon.Framework;
using Eamon.Framework.Primitive.Classes;
using Eamon.Game.Attributes;
using static TheSubAquanLaboratory.Game.Plugin.Globals;

namespace TheSubAquanLaboratory.Game
{
	[ClassMappings]
	public class Artifact : Eamon.Game.Artifact, IArtifact
	{
		public override bool IsAttackable(ref IArtifactCategory ac)
		{
			var artifactUids = new long[] { 83, 84, 85 };

			// Fake-looking back wall / Glass walls / Electrified floor are attackable

			if (artifactUids.Contains(Uid))
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
			var artifactUids = new long[] { 83, 84, 85 };

			// Fake-looking back wall / Glass walls / Electrified floor allow Blast skill gains

			return artifactUids.Contains(Uid) || base.ShouldAllowBlastSkillGains();
		}
	}
}
