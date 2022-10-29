
// BlastCommand.cs

// Copyright (c) 2014+ by Michael Penner.  All rights reserved.

using System.Diagnostics;
using Eamon.Game.Attributes;
using EamonRT.Framework.Commands;
using EamonRT.Framework.States;
using static ARuncibleCargo.Game.Plugin.Globals;

namespace ARuncibleCargo.Game.Commands
{
	[ClassMappings]
	public class BlastCommand : EamonRT.Game.Commands.BlastCommand, IBlastCommand
	{
		public override void Execute()
		{
			Debug.Assert(DobjArtifact != null || DobjMonster != null);

			// Can't blast the Runcible Cargo

			if (DobjArtifact != null && DobjArtifact.Uid == 129)
			{
				gOut.Print("That sounds quite dangerous!");

				NextState = gEngine.CreateInstance<IMonsterStartState>();
			}
			else
			{
				base.Execute();
			}
		}

		public override bool IsAllowedInRoom()
		{
			// Disable BlastCommand in water rooms

			return !gActorRoom(this).IsWaterRoom();
		}
	}
}
