
// EndOfRoundState.cs

// Copyright (c) 2014+ by Michael Penner.  All rights reserved.

using System;
using System.Diagnostics;
using Eamon;
using Eamon.Game.Attributes;
using EamonRT.Framework.Primitive.Enums;
using EamonRT.Framework.States;
using static Firestorm.Game.Plugin.Globals;

namespace Firestorm.Game.States
{
	[ClassMappings]
	public class EndOfRoundState : EamonRT.Game.States.EndOfRoundState, IEndOfRoundState
	{
		public override void ProcessEvents(EventType eventType)
		{
			RetCode rc;

			base.ProcessEvents(eventType);

			Debug.Assert(gGameState != null);

			Debug.Assert(gCharRoom != null);

			if (eventType == EventType.AfterEndRound && gEngine.ShouldPreTurnProcess)
			{
				// Opponents less stunned

				if (gGameState.ST > 0)
				{
					gGameState.ST -= 1;
				}

				// Super soldier picks fight

				if (gCharRoom.Uid == 82 && gGameState.RZ == 0)
				{
					gEngine.PrintEffectDesc(29);

					gOut.Write("{0}Do you want to fight (Y/N): ", Environment.NewLine);

					gEngine.Buf.Clear();

					rc = gEngine.In.ReadField(gEngine.Buf, gEngine.BufSize02, null, ' ', '\0', false, null, gEngine.ModifyCharToUpper, gEngine.IsCharYOrN, null);

					Debug.Assert(gEngine.IsSuccess(rc));

					if (gEngine.Buf.Length > 0 && gEngine.Buf[0] == 'Y')
					{
						gEngine.PrintEffectDesc(30);

						gGameState.Die = 1;

						NextState = gEngine.CreateInstance<IPlayerDeadState>(x =>
						{
							x.PrintLineSep = true;
						});

						GotoCleanup = true;

						goto Cleanup;
					}

					gEngine.PrintEffectDesc(31);

					gGameState.RZ = 1;
				}
			}

		Cleanup:

			;
		}
	}
}
