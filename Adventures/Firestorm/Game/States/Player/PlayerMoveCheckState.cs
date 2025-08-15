
// PlayerMoveCheckState.cs

// Copyright (c) 2014+ by Michael Penner.  All rights reserved.

using System;
using System.Diagnostics;
using Eamon;
using Eamon.Framework.Primitive.Enums;
using Eamon.Game.Attributes;
using EamonRT.Framework.Primitive.Enums;
using EamonRT.Framework.States;
using static Firestorm.Game.Plugin.Globals;

namespace Firestorm.Game.States
{
	[ClassMappings]
	public class PlayerMoveCheckState : EamonRT.Game.States.PlayerMoveCheckState, IPlayerMoveCheckState
	{
		public override void ProcessEvents(EventType eventType)
		{
			RetCode rc;

			base.ProcessEvents(eventType);

			Debug.Assert(gGameState != null);

			Debug.Assert(gCharMonster != null);

			Debug.Assert(gCharRoom != null);

			var darlonMonster = gMDB[3];

			Debug.Assert(darlonMonster != null);

			var zephetteMonster = gMDB[4];

			Debug.Assert(zephetteMonster != null);

			var healingHerbsArtifact = gADB[41];

			Debug.Assert(healingHerbsArtifact != null);

			if (eventType == EventType.BeforeCanMoveToRoomCheck)
			{
				if (gGameState.R2 == 83)
				{
					var rl = gEngine.RollDice(1, gCharMonster.Agility, 0);

					if (rl > 10)
					{
						gEngine.PrintEffectDesc(49);

						gEngine.PrintEffectDesc(50);

						gGameState.PZ = 1;
					}
				}
				else if (gGameState.R2 == 21 && gCharRoom.Uid == 22 && darlonMonster.IsInRoom(gCharRoom) && gGameState.GetNBTL(Friendliness.Enemy) <= 0 && !zephetteMonster.IsInLimbo() && zephetteMonster.Seen && zephetteMonster.Reaction == Friendliness.Neutral && !healingHerbsArtifact.Seen)
				{
					gOut.Print("Darlon says, \"There's still more she can do for you.\"");

					gGameState.R2 = gCharRoom.Uid;

					GotoCleanup = true;
				}
			}
			else if (eventType == EventType.AfterBlockingArtifactCheck)
			{
				if (gGameState.R2 == -3)
				{
					gOut.Print("The trapdoor is too high up!");

					NextState = gEngine.CreateInstance<IStartState>();

					GotoCleanup = true;
				}
				else if (gGameState.R2 == -2)
				{
					gEngine.PrintEffectDesc(21);

					gGameState.R2 = 69;

					NextState = gEngine.CreateInstance<IPlayerMoveCheckState>(x =>
					{
						x.Direction = Direction;
					});

					GotoCleanup = true;
				}
				else if (gGameState.R2 == -9)
				{
					gOut.Print("You are not a ghost. Don't try to walk through walls.");

					NextState = gEngine.CreateInstance<IStartState>();

					GotoCleanup = true;
				}
				else if (gGameState.R2 == gEngine.DirectionExit)
				{
					gOut.Write("{0}Leave this adventure (Y/N): ", Environment.NewLine);

					gEngine.Buf.Clear();

					rc = gEngine.In.ReadField(gEngine.Buf, gEngine.BufSize02, null, ' ', '\0', false, null, gEngine.ModifyCharToUpper, gEngine.IsCharYOrN, null);

					Debug.Assert(gEngine.IsSuccess(rc));

					if (gEngine.Buf.Length > 0 && gEngine.Buf[0] == 'Y')
					{
						// gOut.Print("{0}", gEngine.LineSep);

						gOut.Print("You travel for days and finally make your way back to the Main Hall.");

						gEngine.In.KeyPress(gEngine.Buf);

						gGameState.Die = 0;

						gEngine.ExitType = ExitType.FinishAdventure;

						gEngine.MainLoop.ShouldShutdown = true;
					}

					GotoCleanup = true;
				}
			}
		}
	}
}
