
// LightCommand.cs

// Copyright (c) 2014+ by Michael Penner.  All rights reserved.

using System.Diagnostics;
using Eamon.Game.Attributes;
using EamonRT.Framework.Commands;
using static TheBeginnersCave.Game.Plugin.Globals;

namespace TheBeginnersCave.Game.Commands
{
	[ClassMappings]
	public class LightCommand : EamonRT.Game.Commands.LightCommand, ILightCommand
	{
		public override void Execute()
		{
			Debug.Assert(DobjArtifact != null);

			if (DobjArtifact.Uid == 10)
			{
				var command = gEngine.CreateInstance<Framework.Commands.ITrollsfireCommand>();

				CopyCommandData(command);

				NextState = command;
			}
			else
			{
				base.Execute();
			}
		}
	}
}
