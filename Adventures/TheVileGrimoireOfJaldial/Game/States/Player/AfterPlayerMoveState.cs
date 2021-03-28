
// AfterPlayerMoveState.cs

// Copyright (c) 2014+ by Michael Penner.  All rights reserved.

using System.Diagnostics;
using System.Linq;
using Eamon.Game.Attributes;
using Eamon.Game.Extensions;
using EamonRT.Framework.Primitive.Enums;
using EamonRT.Framework.States;
using static TheVileGrimoireOfJaldial.Game.Plugin.PluginContext;

namespace TheVileGrimoireOfJaldial.Game.States
{
	[ClassMappings]
	public class AfterPlayerMoveState : EamonRT.Game.States.AfterPlayerMoveState, IAfterPlayerMoveState
	{
		public override void ProcessEvents(EventType eventType)
		{
			if (eventType == EventType.AfterExtinguishLightSourceCheck)
			{
				var swampRoomUids = new long[] { 42, 43, 44, 45 };

				var room01 = NewRoom as Framework.IRoom;

				Debug.Assert(room01 != null);

				var room03 = gRDB[gGameState.R3] as Framework.IRoom;

				Debug.Assert(room03 != null);

				// Traveling through the swamp at night or in dense fog with no light source - not advisable

				var odds = (room03.IsDimLightRoomWithoutGlowingMonsters() || room01.IsDimLightRoomWithoutGlowingMonsters()) && gGameState.Ls <= 0 ? 50 : 70;

				// Check for travel through the swamp

				if (gGameState.Ro != 19 && (swampRoomUids.Contains(gGameState.R3) || swampRoomUids.Contains(gGameState.Ro)) && gEngine.RollDice(1, 100, 0) > odds)
				{
					if (!gEngine.SaveThrow(0))
					{
						gEngine.PrintEffectDesc(91);

						gGameState.Die = 1;

						NextState = Globals.CreateInstance<IPlayerDeadState>(x =>
						{
							x.PrintLineSep = true;
						});

						GotoCleanup = true;

						goto Cleanup;
					}
					else
					{
						gEngine.PrintEffectDesc(94);
					}
				}

				var room03IsDimLightRoom = room03.IsDimLightRoom();

				// Check for foggy room

				gGameState.SetFoggyRoomWeatherIntensity(room01);

				var room01IsDimLightRoom = room01.IsDimLightRoom();
			}

			base.ProcessEvents(eventType);

		Cleanup:

			;
		}
	}
}
