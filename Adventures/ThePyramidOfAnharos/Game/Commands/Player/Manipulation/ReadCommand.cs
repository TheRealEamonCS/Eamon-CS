
// ReadCommand.cs

// Copyright (c) 2014+ by Michael Penner.  All rights reserved.

using System;
using System.Diagnostics;
using System.Text;
using Eamon;
using Eamon.Game.Attributes;
using EamonRT.Framework.Commands;
using EamonRT.Framework.Components;
using EamonRT.Framework.Primitive.Enums;
using EamonRT.Framework.States;
using static ThePyramidOfAnharos.Game.Plugin.Globals;

namespace ThePyramidOfAnharos.Game.Commands
{
	[ClassMappings]
	public class ReadCommand : EamonRT.Game.Commands.ReadCommand, IReadCommand
	{
		public override void ProcessEvents(EventType eventType)
		{
			base.ProcessEvents(eventType);

			if (eventType == EventType.BeforePrintArtifactReadText)
			{
				gOut.Print("The glyphs read:");
			}
		}

		public override void Execute()
		{
			RetCode rc;

			Debug.Assert(DobjArtifact != null);

			// Obelisk

			if (DobjArtifact.Uid == 14)
			{
				gOut.Write("{0}Which face of the obelisk do you want to read (N/S/E/W): ", Environment.NewLine);

				var buf = new StringBuilder(gEngine.BufSize);

				rc = gEngine.In.ReadField(buf, gEngine.BufSize02, null, ' ', '\0', false, null, gEngine.ModifyCharToUpper, gEngine.IsCharNOrSOrEOrW, null);

				Debug.Assert(gEngine.IsSuccess(rc));

				if (buf.Length > 0 && buf[0] == 'N')
				{
					gEngine.PrintTheGlyphsRead(29);

					gEngine.PrintEffectDesc(30);

					gGameState.KN = 1;
				}
				else if (buf.Length > 0 && buf[0] == 'S')
				{
					gEngine.PrintTheGlyphsRead(31);

					gEngine.PrintEffectDesc(32);

					gGameState.KO = 1;
				}
				else if (buf.Length > 0 && buf[0] == 'E')
				{
					gEngine.PrintTheGlyphsRead(33);

					gEngine.PrintEffectDesc(34);

					gGameState.KP = 1;
				}
				else if (buf.Length > 0 && buf[0] == 'W')
				{
					gEngine.PrintTheGlyphsRead(27);

					gEngine.PrintEffectDesc(28);

					gGameState.KQ = 1;
				}

				var combatComponent = gEngine.CreateInstance<ICombatComponent>(x =>
				{
					x.SetNextStateFunc = s => NextState = s;

					x.ActorRoom = ActorRoom;

					x.Dobj = ActorMonster;

					x.OmitArmor = true;
				});

				combatComponent.ExecuteCalculateDamage(1, 1);

				if (gGameState.Die > 0)
				{
					goto Cleanup;
				}

				gGameState.KR = gGameState.KN + gGameState.KO + gGameState.KP + gGameState.KQ;

				NextState = gEngine.CreateInstance<IMonsterStartState>();
			}

			// Pedestal

			else if (DobjArtifact.Uid == 30)
			{
				gEngine.PrintTheGlyphsRead(18);

				NextState = gEngine.CreateInstance<IMonsterStartState>();
			}

			// Door / Arch

			else if (DobjArtifact.Uid == 76)
			{
				switch(ActorRoom.Uid)
				{
					case 6:

						gEngine.PrintTheGlyphsRead(26);

						NextState = gEngine.CreateInstance<IMonsterStartState>();

						break;

					case 29:

						gEngine.PrintTheGlyphsRead(25);

						NextState = gEngine.CreateInstance<IMonsterStartState>();

						break;

					default:

						Debug.Assert(1 == 0);

						break;
				}
			}
			else
			{
				base.Execute();
			}

		Cleanup:

			;
		}
	}
}
