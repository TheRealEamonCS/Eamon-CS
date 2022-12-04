﻿
// GetPlayerInputState.cs

// Copyright (c) 2014+ by Michael Penner.  All rights reserved.

using System;
using System.Diagnostics;
using System.Text;
using Eamon;
using Eamon.Framework.Primitive.Enums;
using Eamon.Game.Attributes;
using EamonRT.Framework.Components;
using EamonRT.Framework.Primitive.Enums;
using EamonRT.Framework.States;
using static ThePyramidOfAnharos.Game.Plugin.Globals;

namespace ThePyramidOfAnharos.Game.States
{
	[ClassMappings]
	public class GetPlayerInputState : EamonRT.Game.States.GetPlayerInputState, IGetPlayerInputState
	{
		public override void ProcessEvents(EventType eventType)
		{
			RetCode rc;

			base.ProcessEvents(eventType);

			if (eventType == EventType.BeforePrintCommandPrompt && gEngine.ShouldPreTurnProcess)
			{
				Debug.Assert(gCharMonster != null);

				var room = gCharMonster.GetInRoom();

				Debug.Assert(room != null);

				var guardsMonster = gMDB[20];

				Debug.Assert(guardsMonster != null);

				var faroukMonster = gMDB[6];

				Debug.Assert(faroukMonster != null);

				var deadGuardsArtifact = gADB[75];

				Debug.Assert(deadGuardsArtifact != null);

				var columnOfWaterArtifact = gADB[48];

				Debug.Assert(columnOfWaterArtifact != null);

				var dyingMerchantArtifact = gADB[49];

				Debug.Assert(dyingMerchantArtifact != null);

				var faroukBodyArtifact = gADB[61];

				Debug.Assert(faroukBodyArtifact != null);

				// Guards attack

				if (room.Uid == 17 && gGameState.KH != 1)
				{
					gGameState.KH = 1;

					deadGuardsArtifact.SetInLimbo();

					guardsMonster.SetInRoom(room);

					if (room.IsLit())
					{
						gEngine.PrintEffectDesc(54);
					}
					else
					{
						gOut.Print("As you gaze into the darkness, you are startled to hear something rise from the floor. You hear a rasping voice say, 'Death to the despoilers of the sleep of mighty Anharos', as it attacks.");
					}

					for (var i = 1; i <= guardsMonster.CurrGroupCount; i++)
					{
						var combatComponent = gEngine.CreateInstance<ICombatComponent>(x =>
						{
							x.SetNextStateFunc = s => NextState = s;

							x.ActorMonster = guardsMonster;

							x.ActorRoom = room;

							x.Dobj = gCharMonster;

							x.MemberNumber = i;

							x.AttackNumber = 1;
						});

						combatComponent.ExecuteAttack();

						if (gGameState.Die > 0)
						{
							GotoCleanup = true;

							goto Cleanup;
						}
					}
				}

				// Pedestal room spectrum color puzzle

				if (room.Uid > 31 && room.Uid < 40 && room.IsLit())
				{
					switch (room.Uid)
					{
						case 32:

							if (gGameState.KS == 0)
							{
								gGameState.KS = 1;
							}

							break;

						case 33:

							if (gGameState.KS == 1)
							{
								gGameState.KS = 2;
							}

							break;

						case 34:

							if (gGameState.KS == 2)
							{
								gGameState.KS = 3;
							}

							break;

						case 35:

							if (gGameState.KS == 3)
							{
								gGameState.KS = 4;
							}

							break;

						case 36:

							if (gGameState.KS == 4)
							{
								gGameState.KS = 5;
							}

							break;

						case 37:

							if (gGameState.KS == 5)
							{
								gGameState.KS = 6;
							}

							break;

						case 38:

							if (gGameState.KS == 6)
							{
								gGameState.KS = 7;
							}

							break;
					}

					if (gGameState.KS == 7)
					{
						gGameState.KT = 1;
					}
				}

				// Moonpool waterspout

				if (room.Uid == 38 && columnOfWaterArtifact.IsInLimbo() && gGameState.KS == 7)
				{
					gEngine.PrintEffectDesc(15);

					columnOfWaterArtifact.SetInRoom(room);

					gGameState.KS = 0;

					gGameState.KT = 1;
				}

				// Dying merchant

				if (room.Uid == 52 && dyingMerchantArtifact.IsInRoom(room) && gGameState.GetNBTL(Friendliness.Enemy) <= 0 && gGameState.KW >= 20)
				{
					gOut.Write("{0}Do you wish to give Farouk any water (Y/N): ", Environment.NewLine);

					var buf = new StringBuilder(gEngine.BufSize);

					rc = gEngine.In.ReadField(buf, gEngine.BufSize02, null, ' ', '\0', false, null, gEngine.ModifyCharToUpper, gEngine.IsCharYOrN, null);

					Debug.Assert(gEngine.IsSuccess(rc));

					if (buf.Length > 0 && buf[0] == 'Y')
					{
						gOut.Print("You have revived Farouk.");

						gGameState.KW -= 20;

						dyingMerchantArtifact.SetInLimbo();

						faroukMonster.SetInRoom(room);

						faroukMonster.Seen = true;

						faroukMonster.Reaction = Friendliness.Friend;
					}
					else if (buf.Length > 0 && buf[0] == 'N')
					{
						gOut.Print("Farouk is dead.");

						dyingMerchantArtifact.SetInLimbo();

						faroukBodyArtifact.SetInRoom(room);
					}
				}

				// Friendlies burn one water unit each

				var monsterList = gEngine.GetMonsterList(m => !m.IsCharacterMonster() && m.Reaction == Friendliness.Friend && m.IsInRoom(room));

				gGameState.KW -= monsterList.Count;

				// Player burns water units based on armor

				gGameState.KW -= (gCharMonster.Armor < 3 ? 1 : gCharMonster.Armor < 8 ? gCharMonster.Armor - 1 : 6);

				gOut.Print("You have {0} units of water left.", Math.Max(gGameState.KW, 0));

				if (gGameState.KW < 0)
				{
					var dice = (long)Math.Floor((double)Math.Abs(gGameState.KW) / 12.0);

					if (dice > 0)
					{
						gOut.Print("You are suffering from thirst.");

						var combatComponent = gEngine.CreateInstance<ICombatComponent>(x =>
						{
							x.SetNextStateFunc = s => NextState = s;

							x.ActorRoom = room;

							x.Dobj = gCharMonster;

							x.OmitArmor = true;
						});

						combatComponent.ExecuteCalculateDamage(dice, 1);

						if (gGameState.Die > 0)
						{
							GotoCleanup = true;

							goto Cleanup;
						}
					}
				}

				if (gSentenceParser.IsInputExhausted)
				{
					gEngine.PrintGuideMonsterDirection();
				}
			}

		Cleanup:

			;
		}
	}
}
