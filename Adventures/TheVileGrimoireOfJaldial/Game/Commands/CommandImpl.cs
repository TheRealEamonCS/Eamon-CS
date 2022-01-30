
// CommandImpl.cs

// Copyright (c) 2014+ by Michael Penner.  All rights reserved.

using System.Diagnostics;
using Eamon.Framework;
using Eamon.Game.Attributes;
using EamonRT.Framework.Commands;
using static TheVileGrimoireOfJaldial.Game.Plugin.PluginContext;

namespace TheVileGrimoireOfJaldial.Game.Commands
{
	[ClassMappings]
	public class CommandImpl : EamonRT.Game.Commands.CommandImpl, ICommandImpl
	{
		public override void PrintLightObj(IArtifact artifact)
		{
			Debug.Assert(artifact != null);

			if (artifact.Uid == 1)
			{
				gOut.Print("A magical flame bursts from {0}.", artifact.GetTheName());
			}
			else
			{
				base.PrintLightObj(artifact);
			}
		}

		public override void PrintLightExtinguished(IArtifact artifact)
		{
			Debug.Assert(artifact != null);

			if (artifact.Uid == 1)
			{
				gOut.Print("The fire is violently extinguished.");
			}
			else
			{
				base.PrintLightExtinguished(artifact);
			}
		}

		public override bool ShouldShowUnseenArtifacts(IRoom room, IArtifact artifact)
		{
			return !gGameState.ParalyzedTargets.ContainsKey(gGameState.Cm) && base.ShouldShowUnseenArtifacts(room, artifact);
		}
	}
}
