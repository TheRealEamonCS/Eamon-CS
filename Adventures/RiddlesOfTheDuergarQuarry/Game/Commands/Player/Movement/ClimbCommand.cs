
// ClimbCommand.cs

// Copyright (c) 2014+ by Michael Penner.  All rights reserved.

using System.Diagnostics;
using Eamon.Framework.Primitive.Enums;
using Eamon.Game.Attributes;
using EamonRT.Framework.Commands;
using EamonRT.Framework.States;
using static RiddlesOfTheDuergarQuarry.Game.Plugin.PluginContext;

namespace RiddlesOfTheDuergarQuarry.Game.Commands
{
	[ClassMappings]
	public class ClimbCommand : EamonRT.Game.Commands.Command, Framework.Commands.IClimbCommand
	{
		public override void Execute()
		{
			Debug.Assert(DobjArtifact != null);

			// Large boulder

			if (DobjArtifact.Uid == 78)
			{
				var command = Globals.CreateInstance<IGoCommand>();

				CopyCommandData(command);

				NextState = command;
			}
			else
			{
				PrintDontFollowYou();

				NextState = Globals.CreateInstance<IStartState>();
			}
		}

		public ClimbCommand()
		{
			SortOrder = 98;

			IsNew = true;

			Uid = 95;

			Name = "ClimbCommand";

			Verb = "climb";

			Type = CommandType.Movement;
		}
	}
}
