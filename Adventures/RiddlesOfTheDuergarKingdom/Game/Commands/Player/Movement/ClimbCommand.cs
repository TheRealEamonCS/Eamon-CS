
// ClimbCommand.cs

// Copyright (c) 2014+ by Michael Penner.  All rights reserved.

using System.Diagnostics;
using Eamon.Framework.Primitive.Enums;
using Eamon.Game.Attributes;
using EamonRT.Framework.Commands;
using EamonRT.Framework.States;
using static RiddlesOfTheDuergarKingdom.Game.Plugin.PluginContext;

namespace RiddlesOfTheDuergarKingdom.Game.Commands
{
	[ClassMappings]
	public class ClimbCommand : EamonRT.Game.Commands.Command, Framework.Commands.IClimbCommand
	{
		public override void Execute()
		{
			Debug.Assert(DobjArtifact != null);

			var mesquiteTreeArtifact = gADB[70];

			Debug.Assert(mesquiteTreeArtifact != null);

			var isClimbableRope = DobjArtifact.Uid == 69 && (DobjArtifact.IsCarriedByContainer(mesquiteTreeArtifact) || DobjArtifact.IsInRoomUid(37));

			// Large boulder / Wooden ladder / Bottomless pit / Rope

			if (DobjArtifact.Uid == 2 || DobjArtifact.Uid == 14 || DobjArtifact.Uid == 53 || isClimbableRope)
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

			Name = "ClimbCommand";

			Verb = "climb";

			Type = CommandType.Movement;
		}
	}
}
