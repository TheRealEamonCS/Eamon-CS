
// DropCommand.cs

// Copyright (c) 2014+ by Michael Penner.  All rights reserved.

using System;
using System.Diagnostics;
using Eamon.Framework;
using Eamon.Game.Attributes;
using EamonRT.Framework.Commands;
using static TheDeepCanyon.Game.Plugin.PluginContext;

namespace TheDeepCanyon.Game.Commands
{
	[ClassMappings]
	public class DropCommand : EamonRT.Game.Commands.DropCommand, IDropCommand
	{
		public override void ProcessArtifact(IArtifact artifact)
		{
			Debug.Assert(artifact != null);

			base.ProcessArtifact(artifact);

			// Drop in vertical shaft

			if (ActorRoom.Uid == 70 || ActorRoom.Uid == 71)
			{
				gOut.Print("{0}{1} fall{2} down into the darkness.", Environment.NewLine, artifact.GetTheName(true), artifact.EvalPlural("s", ""));

				artifact.SetInRoomUid(69);
			}
		}
	}
}
