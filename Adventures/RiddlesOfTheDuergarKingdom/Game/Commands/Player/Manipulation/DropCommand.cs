
// DropCommand.cs

// Copyright (c) 2014+ by Michael Penner.  All rights reserved.

using System;
using System.Diagnostics;
using Eamon.Framework;
using Eamon.Game.Attributes;
using EamonRT.Framework.Commands;
using static RiddlesOfTheDuergarKingdom.Game.Plugin.PluginContext;

namespace RiddlesOfTheDuergarKingdom.Game.Commands
{
	[ClassMappings]
	public class DropCommand : EamonRT.Game.Commands.DropCommand, IDropCommand
	{
		public override void ProcessArtifact(IArtifact artifact)
		{
			Debug.Assert(artifact != null);

			base.ProcessArtifact(artifact);

			// Drop in placid lake

			if (ActorRoom.Uid == 40)
			{
				gOut.Print("{0}{1} sink{2} into the depths of the lake!", Environment.NewLine, artifact.GetTheName(true), artifact.EvalPlural("s", ""));

				artifact.SetInLimbo();
			}
		}
	}
}
