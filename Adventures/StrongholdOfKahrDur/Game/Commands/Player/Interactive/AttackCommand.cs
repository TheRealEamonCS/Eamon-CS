
// AttackCommand.cs

// Copyright (c) 2014+ by Michael Penner.  All rights reserved.

using System.Diagnostics;
using Eamon.Framework.Primitive.Enums;
using Eamon.Game.Attributes;
using EamonRT.Framework.Commands;
using static StrongholdOfKahrDur.Game.Plugin.PluginContext;

namespace StrongholdOfKahrDur.Game.Commands
{
	[ClassMappings]
	public class AttackCommand : EamonRT.Game.Commands.AttackCommand, IAttackCommand
	{
		public override void Execute()
		{
			Debug.Assert(DobjArtifact != null || DobjMonster != null);

			// Can't attack armoire/bookshelf/pouch

			if ((BlastSpell || ActorMonster.Weapon > 0) && DobjArtifact != null && (DobjArtifact.Uid == 3 || DobjArtifact.Uid == 11 || DobjArtifact.Uid == 15))
			{
				var ac = DobjArtifact.GetArtifactCategory(new ArtifactType[] { ArtifactType.InContainer, ArtifactType.User1 });

				Debug.Assert(ac != null);

				var type = ac.Type;

				ac.Type = ArtifactType.Gold;

				base.Execute();

				ac.Type = type;
			}
			else
			{
				base.Execute();
			}
		}
	}
}
