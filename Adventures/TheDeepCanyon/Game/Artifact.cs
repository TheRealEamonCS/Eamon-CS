
// Artifact.cs

// Copyright (c) 2014+ by Michael Penner.  All rights reserved.

using Eamon.Framework;
using Eamon.Framework.Primitive.Classes;
using Eamon.Game.Attributes;
using static TheDeepCanyon.Game.Plugin.Globals;

namespace TheDeepCanyon.Game
{
	[ClassMappings]
	public class Artifact : Eamon.Game.Artifact, IArtifact
	{
		public override bool IsAttackable(ref IArtifactCategory ac)
		{
			bool result;

			gEngine.PushRulesetVersion(0);

			result = base.IsAttackable(ref ac);

			gEngine.PopRulesetVersion();

			return result;
		}
	}
}
