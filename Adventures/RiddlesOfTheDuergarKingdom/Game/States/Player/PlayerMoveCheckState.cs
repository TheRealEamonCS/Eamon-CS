
// PlayerMoveCheckState.cs

// Copyright (c) 2014+ by Michael Penner.  All rights reserved.

using System.Diagnostics;
using Eamon.Framework.Primitive.Enums;
using Eamon.Game.Attributes;
using EamonRT.Framework.Primitive.Enums;
using EamonRT.Framework.States;
using static RiddlesOfTheDuergarKingdom.Game.Plugin.Globals;

namespace RiddlesOfTheDuergarKingdom.Game.States
{
	[ClassMappings]
	public class PlayerMoveCheckState : EamonRT.Game.States.PlayerMoveCheckState, IPlayerMoveCheckState
	{
		public override void ProcessEvents(EventType eventType)
		{
			base.ProcessEvents(eventType);

			if (eventType == EventType.BeforeCanMoveToRoomCheck)
			{
				Debug.Assert(gCharMonster != null);

				var room = gCharMonster.GetInRoom();

				Debug.Assert(room != null);

				var highlandIbexMonster = gMDB[21];

				Debug.Assert(highlandIbexMonster != null);

				// Cannot climb to summit without ibex's assistance

				if ((gGameState.Ro == 148 && gGameState.R2 == 150) ||
					 (gGameState.Ro == 150 && gGameState.R2 == 151) || 
					 (gGameState.Ro == 151 && gGameState.R2 == 152) || 
					 (gGameState.Ro == 152 && gGameState.R2 == 153) || 
					 (gGameState.Ro == 153 && gGameState.R2 == 44))
				{
					var artifactList = gCharMonster.GetContainedList();

					if (!highlandIbexMonster.IsInRoom(room) || highlandIbexMonster.Reaction < Friendliness.Friend || artifactList.Count > 0)
					{
						gEngine.PrintEffectDesc(88);

						GotoCleanup = true;

						goto Cleanup;
					}

					gEngine.PrintEffectDesc(90);
				}
			}

		Cleanup:

			;
		}
	}
}
