
// SetCommand.cs

// Copyright (c) 2014+ by Michael Penner.  All rights reserved.

using System.Diagnostics;
using Eamon.Framework.Primitive.Enums;
using Eamon.Game.Attributes;
using EamonRT.Framework.States;
using static TheDeepCanyon.Game.Plugin.PluginContext;

namespace TheDeepCanyon.Game.Commands
{
	[ClassMappings]
	public class SetCommand : EamonRT.Game.Commands.Command, Framework.Commands.ISetCommand
	{
		public override void Execute()
		{
			Debug.Assert(DobjArtifact != null);

			// Mouse trap

			if (DobjArtifact.Uid == 17)
			{
				var mouseArtifact = gADB[19];

				Debug.Assert(mouseArtifact != null);

				var cheeseArtifact = gADB[21];

				Debug.Assert(cheeseArtifact != null);

				if (gGameState.TrapSet)
				{
					gOut.Print("The trap is already set.");

					NextState = Globals.CreateInstance<IStartState>();
				}
				else if (!cheeseArtifact.IsCarriedByCharacter() && !cheeseArtifact.IsInRoom(ActorRoom))
				{
					gOut.Print("You have no bait.");

					NextState = Globals.CreateInstance<IStartState>();
				}
				else
				{
					if (!mouseArtifact.IsInLimbo())
					{
						if (mouseArtifact.IsCarriedByCharacter() || mouseArtifact.IsCarriedByContainer(DobjArtifact) || mouseArtifact.IsInRoom(ActorRoom))
						{
							gOut.Print("The mouse escapes as you set the trap.");
						}

						mouseArtifact.SetInLimbo();
					}

					gOut.Print("Okay, the trap is set.");

					// DobjArtifact.SetInRoom(ActorRoom);

					DobjArtifact.InContainer.SetOpen(true);

					gGameState.TrapSet = true;
				}
			}
			else
			{
				PrintCantVerbObj(DobjArtifact);

				NextState = Globals.CreateInstance<IStartState>();
			}

			if (NextState == null)
			{
				NextState = Globals.CreateInstance<IMonsterStartState>();
			}
		}

		public SetCommand()
		{
			SortOrder = 245;

			IsNew = true;

			Uid = 96;

			Name = "SetCommand";

			Verb = "set";

			Type = CommandType.Manipulation;
		}
	}
}
