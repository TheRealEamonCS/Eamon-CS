
// CloseCommand.cs

// Copyright (c) 2014+ by Michael Penner.  All rights reserved.

using System.Diagnostics;
using Eamon.Game.Attributes;
using EamonRT.Framework.Commands;
using EamonRT.Framework.States;
using static AlternateBeginnersCave.Game.Plugin.PluginContext;

namespace AlternateBeginnersCave.Game.Commands
{
	[ClassMappings]
	public class CloseCommand : EamonRT.Game.Commands.CloseCommand, ICloseCommand
	{
		public override void Execute()
		{
			Debug.Assert(DobjArtifact != null);

			// Waspicide and Illusionary wall

			if (DobjArtifact.Uid == 5 || DobjArtifact.Uid == 37)
			{
				PrintDontFollowYou();

				NextState = Globals.CreateInstance<IStartState>();
			}

			// Great grate

			else if (DobjArtifact.Uid == 22)
			{
				PrintDontNeedTo();

				NextState = Globals.CreateInstance<IStartState>();
			}
			else
			{
				base.Execute();
			}
		}
	}
}
