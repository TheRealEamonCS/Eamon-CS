
// GetPlayerInputState.cs

// Copyright (c) 2014+ by Michael Penner.  All rights reserved.

using System;
using System.Diagnostics;
using System.Text;
using Eamon;
using Eamon.Framework;
using Eamon.Framework.Primitive.Enums;
using Eamon.Game.Attributes;
using Eamon.Game.Extensions;
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

				var avatarOfAlaxarMonster = gMDB[8];

				Debug.Assert(avatarOfAlaxarMonster != null);

				var diamondOfPurityArtifact = gADB[38];

				Debug.Assert(diamondOfPurityArtifact != null);

				var onyxCaseArtifact = gADB[39];

				Debug.Assert(onyxCaseArtifact != null);

				var columnOfWaterArtifact = gADB[48];

				Debug.Assert(columnOfWaterArtifact != null);

				var dyingMerchantArtifact = gADB[49];

				Debug.Assert(dyingMerchantArtifact != null);

				var staircaseArtifact = gADB[50];

				Debug.Assert(staircaseArtifact != null);

				var faroukBodyArtifact = gADB[61];

				Debug.Assert(faroukBodyArtifact != null);

				var statueArtifact = gADB[63];

				Debug.Assert(statueArtifact != null);

				var deadGuardsArtifact = gADB[75];

				Debug.Assert(deadGuardsArtifact != null);

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

				if (room.Uid < 30 && gGameState.KU != 0)
				{
					staircaseArtifact.SetInLimbo();

					gGameState.KU = 0;
				}

				// Riddle

				if (room.Uid == 30 && statueArtifact.IsInRoom(room) && gGameState.KU == 0)
				{
					gOut.PunctSpaceCode = PunctSpaceCode.None;

					gOut.Print("As you enter the room, a voice emanates from {0},", room.IsLit() ? "the statue" : "nearby");

					gOut.Write("{0}\"Hearken my word,", Environment.NewLine);

					gOut.Write("{0}On her is heard,", Environment.NewLine);

					gOut.Write("{0}No other may", Environment.NewLine);

					gOut.Write("{0}Open the way,", Environment.NewLine);

					gOut.Write("{0}Respond the trait", Environment.NewLine);

					gOut.Write("{0}To see his fate.\"{0}", Environment.NewLine);

					gOut.Print("What is your answer?");

					gOut.PunctSpaceCode = PunctSpaceCode.Single;

					gOut.Write("{0}Answer: ", Environment.NewLine);

					var buf = new StringBuilder(gEngine.BufSize);

					buf.SetFormat("{0}", gEngine.In.ReadLine());

					if (buf.ToString().Equals("honor", StringComparison.OrdinalIgnoreCase))
					{
						gEngine.PrintEffectDesc(52);

						staircaseArtifact.SetInRoom(room);

						gGameState.KU = 2;
					}
					else
					{
						if (room.IsLit())
						{
							gEngine.PrintEffectDesc(53);
						}
						else
						{
							gOut.Print("You hear a commotion, and something screeches at you. It advances toward you in the darkness.");
						}

						statueArtifact.SetInLimbo();

						avatarOfAlaxarMonster.SetInRoom(room);

						gGameState.KU = 1;
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

				// Diamond of Purity drains life force

				{
					IMonster monster = null;

					long dice = 0;

					if (diamondOfPurityArtifact.IsCarriedByCharacter() && gGameState.KR < 4)
					{
						monster = gCharMonster;

						dice = 4 - gGameState.KR;
					}
					else if (diamondOfPurityArtifact.IsCarriedByContainer(onyxCaseArtifact) && onyxCaseArtifact.IsCarriedByCharacter() && gGameState.GD == 1 && gGameState.KR < 4)
					{
						monster = gCharMonster;

						dice = 1;
					}
					else if (diamondOfPurityArtifact.IsCarriedByMonster() && gGameState.KR < 4)
					{
						monster = diamondOfPurityArtifact.GetCarriedByMonster();

						dice = 4 - gGameState.KR;
					}
					else if (diamondOfPurityArtifact.IsCarriedByContainer(onyxCaseArtifact) && onyxCaseArtifact.IsCarriedByMonster() && gGameState.GD == 1 && gGameState.KR < 4)
					{
						monster = onyxCaseArtifact.GetCarriedByMonster();

						dice = 1;
					}

					if (monster != null && monster.IsInRoom(room))
					{
						gOut.Print("{0} drains {1} life force.", monster.IsCharacterMonster() || room.IsLit() ? diamondOfPurityArtifact.GetTheName(true) : "Something", monster.IsCharacterMonster() ? "your" : room.EvalLightLevel("an entity's", monster.GetTheName(false).AddPossessiveSuffix()));

						var combatComponent = gEngine.CreateInstance<ICombatComponent>(x =>
						{
							x.SetNextStateFunc = s => NextState = s;

							x.ActorRoom = room;

							x.Dobj = monster;

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

				{
					// Friendlies burn one water unit each

					var monsterList = gEngine.GetMonsterList(m => !m.IsCharacterMonster() && m.Reaction == Friendliness.Friend && m.IsInRoom(room));

					gGameState.KW -= monsterList.Count;

					// Player burns water units based on armor

					gGameState.KW -= (gCharMonster.Armor < 3 ? 1 : gCharMonster.Armor < 8 ? gCharMonster.Armor - 1 : 6);

					var waterUnits = Math.Max(gGameState.KW, 0);

					gOut.Print("You have {0} unit{1} of water left.", waterUnits, waterUnits != 1 ? "s" : "");

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
