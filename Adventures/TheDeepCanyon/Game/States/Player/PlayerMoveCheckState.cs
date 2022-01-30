
// PlayerMoveCheckState.cs

// Copyright (c) 2014+ by Michael Penner.  All rights reserved.

using System.Diagnostics;
using Eamon.Game.Attributes;
using EamonRT.Framework.Primitive.Enums;
using EamonRT.Framework.States;
using static TheDeepCanyon.Game.Plugin.PluginContext;

namespace TheDeepCanyon.Game.States
{
	[ClassMappings]
	public class PlayerMoveCheckState : EamonRT.Game.States.PlayerMoveCheckState, IPlayerMoveCheckState
	{
		public override void ProcessEvents(EventType eventType)
		{
			base.ProcessEvents(eventType);

			Debug.Assert(gCharMonster != null);

			var room = gCharMonster.GetInRoom();

			Debug.Assert(room != null);

			if (eventType == EventType.BeforeCanMoveToRoomCheck)
			{
				var rl = gEngine.RollDice(1, 100, 0);

				if (!room.IsLit() && rl > 70)
				{
					gOut.Print("You bumped into an obstacle.");

					GotoCleanup = true;
				}
				else if (gGameState.R2 == -44)
				{
					gOut.Print("You feel the ground fall out beneath your feet.");

					gGameState.R2 = 23;
				}
				else if (gGameState.R2 == -60)
				{
					gOut.Print("You feel the ground fall out beneath your feet.");

					gGameState.R2 = 26;
				}
			}
			else if (eventType == EventType.AfterBlockingArtifactCheck)
			{
				if (gGameState.R2 == -25)
				{
					gOut.Print("{0} is blocking the passage to the south.", room.IsLit() ? "Fido" : "Something");

					GotoCleanup = true;
				}
				else if (gGameState.R2 == -45)
				{
					gOut.Print("{0} you from traveling west.", room.IsLit() ? "The elephants prevent" : "Something prevents");

					GotoCleanup = true;
				}
				else if (!room.IsLit() && gGameState.R2 == 0)
				{
					gOut.Print("You bumped into an obstacle.");

					GotoCleanup = true;
				}
			}
		}
	}
}
