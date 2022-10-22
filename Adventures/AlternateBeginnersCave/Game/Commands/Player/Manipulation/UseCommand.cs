
// UseCommand.cs

// Copyright (c) 2014+ by Michael Penner.  All rights reserved.

using System.Diagnostics;
using Eamon.Game.Attributes;
using EamonRT.Framework.Commands;
using static AlternateBeginnersCave.Game.Plugin.Globals;

namespace AlternateBeginnersCave.Game.Commands
{
	[ClassMappings]
	public class UseCommand : EamonRT.Game.Commands.UseCommand, IUseCommand
	{
		public override void Execute()
		{
			Debug.Assert(DobjArtifact != null);

			// Waspicide and grate

			if (DobjArtifact.Uid == 5 || DobjArtifact.Uid == 22)
			{
				var command = gEngine.CreateInstance<IOpenCommand>();

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
