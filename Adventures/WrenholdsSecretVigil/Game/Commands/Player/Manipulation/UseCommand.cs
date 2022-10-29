
// UseCommand.cs

// Copyright (c) 2014+ by Michael Penner.  All rights reserved.

using System.Diagnostics;
using Eamon.Game.Attributes;
using EamonRT.Framework.Commands;
using static WrenholdsSecretVigil.Game.Plugin.Globals;

namespace WrenholdsSecretVigil.Game.Commands
{
	[ClassMappings]
	public class UseCommand : EamonRT.Game.Commands.UseCommand, IUseCommand
	{
		public override void Execute()
		{
			Debug.Assert(DobjArtifact != null);

			var deviceArtifact = gADB[44];

			Debug.Assert(deviceArtifact != null);

			// Use lever

			if (DobjArtifact.Uid == 48 && deviceArtifact.IsInRoom(ActorRoom))
			{
				var command = gEngine.CreateInstance<IGetCommand>();

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
