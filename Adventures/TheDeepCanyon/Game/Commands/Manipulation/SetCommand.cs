
// SetCommand.cs

// Copyright (c) 2014+ by Michael Penner.  All rights reserved.

using System.Diagnostics;
using Eamon.Framework.Primitive.Enums;
using Eamon.Game.Attributes;
using EamonRT.Framework.States;
using static TheDeepCanyon.Game.Plugin.Globals;

namespace TheDeepCanyon.Game.Commands
{
	[ClassMappings]
	public class SetCommand : EamonRT.Game.Commands.Command, Framework.Commands.ISetCommand
	{
		public override void ExecuteForPlayer()
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

					NextState = gEngine.CreateInstance<IStartState>();
				}
				else if (!cheeseArtifact.IsCarriedByMonster(ActorMonster) && !cheeseArtifact.IsInRoom(ActorRoom))
				{
					gOut.Print("You have no bait.");

					NextState = gEngine.CreateInstance<IStartState>();
				}
				else
				{
					if (!DobjArtifact.InContainer.IsOpen())
					{
						PrintOpened(DobjArtifact);

						DobjArtifact.InContainer.SetOpen(true);
					}

					if (!mouseArtifact.IsInLimbo())
					{
						if (mouseArtifact.IsCarriedByMonster(ActorMonster) || mouseArtifact.IsCarriedByContainer(DobjArtifact) || mouseArtifact.IsInRoom(ActorRoom))
						{
							gOut.Print("The mouse escapes as you set the trap.");
						}

						mouseArtifact.SetInLimbo();
					}

					gOut.Print("Okay, the trap is set.");

					// DobjArtifact.SetInRoom(ActorRoom);

					gGameState.TrapSet = true;
				}
			}
			else
			{
				PrintCantVerbObj(DobjArtifact);

				NextState = gEngine.CreateInstance<IStartState>();
			}

			if (NextState == null)
			{
				NextState = gEngine.CreateInstance<IMonsterStartState>();
			}
		}

		public SetCommand()
		{
			SortOrder = 245;

			IsNew = true;

			Name = "SetCommand";

			Verb = "set";

			Type = CommandType.Manipulation;
		}
	}
}
