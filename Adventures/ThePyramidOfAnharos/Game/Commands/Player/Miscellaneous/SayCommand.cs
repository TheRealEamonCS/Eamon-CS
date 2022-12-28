
// SayCommand.cs

// Copyright (c) 2014+ by Michael Penner.  All rights reserved.

using System;
using System.Diagnostics;
using System.Text;
using Eamon;
using Eamon.Framework.Primitive.Enums;
using Eamon.Game.Attributes;
using EamonRT.Framework.Commands;
using EamonRT.Framework.Primitive.Enums;
using EamonRT.Framework.States;
using static ThePyramidOfAnharos.Game.Plugin.Globals;

namespace ThePyramidOfAnharos.Game.Commands
{
	[ClassMappings]
	public class SayCommand : EamonRT.Game.Commands.SayCommand, ISayCommand
	{
		public override void ProcessEvents(EventType eventType)
		{
			RetCode rc;

			base.ProcessEvents(eventType);

			if (eventType == EventType.AfterPrintSayText)
			{
				var carpetArtifact = gADB[40];

				Debug.Assert(carpetArtifact != null);

				if (ProcessedPhrase.Equals("for anharos", StringComparison.OrdinalIgnoreCase))
				{
					if (gGameState.Ro == 6 || gGameState.Ro == 14)
					{
						gGameState.KE = gGameState.KE == 0 ? 1 : 0;

						gEngine.PrintEffectDesc(11 - gGameState.KE);
					}
					else if (gGameState.Ro == 12 || gGameState.Ro == 16)
					{
						gGameState.KF = gGameState.KF == 0 ? 1 : 0;

						gEngine.PrintEffectDesc(13 - gGameState.KF);
					}
				}
				else if (ProcessedPhrase.Equals("soar with alaxar", StringComparison.OrdinalIgnoreCase) && carpetArtifact.IsCarriedByCharacter())
				{
					gEngine.PrintEffectDesc(14);

					gOut.Write("{0}1=Pyramid; 2=Obelisk; 3=Oasis; 4=Main Hall: ", Environment.NewLine);

					var buf = new StringBuilder(gEngine.BufSize);

					rc = gEngine.In.ReadField(buf, gEngine.BufSize02, null, ' ', '\0', false, null, gEngine.ModifyCharToUpper, gEngine.IsChar1To4, null);

					Debug.Assert(gEngine.IsSuccess(rc));

					if (buf.Length > 0)
					{
						gGameState.R2 = buf[0] == '1' ? 12 : buf[0] == '2' ? 49 : buf[0] == '3' ? 55 : 1;
					}
					else
					{
						gGameState.R2 = gGameState.Ro;
					}

					// Friendlies also ride the flying carpet

					var monsterList = gEngine.GetMonsterList(m => !m.IsCharacterMonster() && m.Reaction == Friendliness.Friend && m.IsInRoom(ActorRoom));

					foreach (var monster in monsterList)
					{
						monster.SetInRoomUid(gGameState.R2);
					}

					NextState = gEngine.CreateInstance<IAfterPlayerMoveState>(x =>
					{
					 	x.MoveMonsters = false;
					});

					GotoCleanup = true;

					goto Cleanup;
				}
			}

		Cleanup:

			;
		}
	}
}
