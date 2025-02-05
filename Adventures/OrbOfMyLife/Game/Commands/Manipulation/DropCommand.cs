
// DropCommand.cs

// Copyright (c) 2014+ by Michael Penner.  All rights reserved.

using System.Diagnostics;
using Eamon.Framework;
using Eamon.Game.Attributes;
using EamonRT.Framework.Commands;
using static OrbOfMyLife.Game.Plugin.Globals;

namespace OrbOfMyLife.Game.Commands
{
	[ClassMappings]
	public class DropCommand : EamonRT.Game.Commands.DropCommand, IDropCommand
	{
		public override void ExecuteForPlayer()
		{
			if (DropAll)
			{
				gGameState.SR = ActorRoom.Uid;
			}

			base.ExecuteForPlayer();
		}

		public override void ProcessArtifact(IArtifact artifact)
		{
			Debug.Assert(artifact != null);

			base.ProcessArtifact(artifact);

			// Crystal ball shatters

			if (artifact.Uid == 8)
			{
				gEngine.CrystalBallShatters(ActorRoom, artifact);
			}

			if (gGameState.IV)
			{
				gEngine.MonstersGetUnnerved(true);
			}
		}
	}
}
