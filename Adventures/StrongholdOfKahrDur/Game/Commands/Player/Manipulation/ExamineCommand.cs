
// ExamineCommand.cs

// Copyright (c) 2014+ by Michael Penner.  All rights reserved.

using System.Diagnostics;
using Eamon.Game.Attributes;
using EamonRT.Framework.Commands;
using EamonRT.Framework.Primitive.Enums;
using static StrongholdOfKahrDur.Game.Plugin.PluginContext;

namespace StrongholdOfKahrDur.Game.Commands
{
	[ClassMappings]
	public class ExamineCommand : EamonRT.Game.Commands.ExamineCommand, IExamineCommand
	{
		public override void ProcessEvents(EventType eventType)
		{
			if (eventType == EventType.AfterPrintArtifactFullDesc)
			{
				var eyeglassesArtifact = gADB[2];

				Debug.Assert(eyeglassesArtifact != null);

				var secretDoorArtifact = gADB[4];

				Debug.Assert(secretDoorArtifact != null);

				var secretDoorArtifact01 = gADB[10];

				Debug.Assert(secretDoorArtifact01 != null);

				// Armoire (while wearing glasses)

				if (DobjArtifact.Uid == 3 && eyeglassesArtifact.IsWornByCharacter() && !secretDoorArtifact.IsInRoom(ActorRoom))
				{
					var ac = DobjArtifact.InContainer;

					Debug.Assert(ac != null);

					ac.SetOpen(false);

					var command = Globals.CreateInstance<IOpenCommand>();

					CopyCommandData(command);

					NextState = command;

					GotoCleanup = true;
				}

				// Bookshelf/secret door in library (while wearing magic glasses)

				else if (DobjArtifact.Uid == 11 && eyeglassesArtifact.IsWornByCharacter() && !secretDoorArtifact01.IsInRoom(ActorRoom))
				{
					var ac = secretDoorArtifact01.DoorGate;

					Debug.Assert(ac != null);

					secretDoorArtifact01.SetInRoom(ActorRoom);

					ac.SetOpen(true);

					ac.Field4 = 0;
				}
				else
				{
					base.ProcessEvents(eventType);
				}
			}
			else
			{
				base.ProcessEvents(eventType);
			}
		}
	}
}
