
// PlayerMoveCheckState.cs

// Copyright (c) 2014+ by Michael Penner.  All rights reserved.

using System;
using System.Diagnostics;
using System.Text.RegularExpressions;
using Eamon;
using Eamon.Framework.Helpers;
using Eamon.Framework.Primitive.Enums;
using Eamon.Game.Attributes;
using Eamon.Game.Extensions;
using EamonRT.Framework.Components;
using EamonRT.Framework.Primitive.Enums;
using EamonRT.Framework.States;
using static WrenholdsSecretVigil.Game.Plugin.Globals;

namespace WrenholdsSecretVigil.Game.States
{
	[ClassMappings]
	public class PlayerMoveCheckState : EamonRT.Game.States.PlayerMoveCheckState, IPlayerMoveCheckState
	{
		public override void ProcessEvents(EventType eventType)
		{
			RetCode rc;

			base.ProcessEvents(eventType);

			if (eventType == EventType.BeforeCanMoveToRoomCheck)
			{
				var room5 = gRDB[5];

				Debug.Assert(room5 != null);

				var room40 = gRDB[40];

				Debug.Assert(room40 != null);

				var room45 = gRDB[45];

				Debug.Assert(room45 != null);

				// Auto-reveal secret doors if necessary

				if (gGameState.R2 == 5 && gGameState.Ro == 8 && room5.GetDir(Direction.South) == -8)
				{
					room5.SetDir(Direction.South, 8);
				}
				else if (gGameState.R2 == 40 && gGameState.Ro == 41 && room40.GetDir(Direction.South) == -41)
				{
					room40.SetDir(Direction.South, 41);
				}
				else if (gGameState.R2 == 45 && gGameState.Ro == 43 && room45.GetDir(Direction.East) == -43)
				{
					room45.SetDir(Direction.East, 43);
				}

				// Falling down a drop-off (injury)

				if (gGameState.R2 == 25 && (gGameState.Ro == 24 || gGameState.Ro == 27) && Direction != Direction.Down)
				{
					gEngine.PrintEffectDesc(24);

					var combatComponent = gEngine.CreateInstance<ICombatComponent>(x =>
					{
						x.SetNextStateFunc = s => NextState = s;

						x.ActorRoom = gCharMonster.GetInRoom();

						x.Dobj = gCharMonster;

						x.OmitArmor = true;
					});

					combatComponent.ExecuteCalculateDamage(1, 1);

					if (gGameState.Die > 0)
					{
						GotoCleanup = true;
					}
				}

				// Falling down a drop-off (no injury)

				else if (gGameState.R2 == 18 && gGameState.Ro == 17 && Direction != Direction.Down)
				{
					gEngine.PrintEffectDesc(5);
				}
			}
			else if (eventType == EventType.AfterBlockingArtifactCheck)
			{
				var ropeArtifact = gADB[28];

				Debug.Assert(ropeArtifact != null);

				// Down the airshaft

				if (gGameState.R2 == -100)
				{
					gEngine.PrintEffectDesc(42);

					gGameState.R2 = 36;

					NextState = gEngine.CreateInstance<IAfterPlayerMoveState>();

					GotoCleanup = true;
				}

				// Over the cliff

				else if (gGameState.R2 == -101)
				{
					gEngine.PrintEffectDesc(1);

					gGameState.Die = 1;

					NextState = gEngine.CreateInstance<IPlayerDeadState>(x =>
					{
						x.PrintLineSep = true;
					});

					GotoCleanup = true;
				}

				// Up rope rings bell

				else if (gGameState.R2 == -102)
				{
					gEngine.RevealEmbeddedArtifact(OldRoom, ropeArtifact);

					gEngine.PrintEffectDesc(25);

					gGameState.R2 = gGameState.Ro;

					if (!gGameState.PulledRope)
					{
						var monsterList = gEngine.GetMonsterList(m => m.Uid >= 14 && m.Uid <= 16);

						foreach (var monster in monsterList)
						{
							monster.SetInRoomUid(48);
						}

						gGameState.PulledRope = true;
					}

					PrintCantGoThatWay();

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
						var lifeOrbArtifact = gADB[4];

						Debug.Assert(lifeOrbArtifact != null);

						var carryingLifeOrb = lifeOrbArtifact.IsCarriedByCharacter();

						var lifeOrbInPedestal = lifeOrbArtifact.IsCarriedByContainerUid(43);

						lifeOrbArtifact.SetInLimbo();

						gOut.Print("{0}", gEngine.LineSep);

						// If life-orb carried by player character or in metal pedestal

						if (carryingLifeOrb || lifeOrbInPedestal)
						{
							if (carryingLifeOrb)
							{
								gEngine.PrintEffectDesc(31);
							}
							else
							{
								gEngine.PrintEffectDesc(35);

								gEngine.PrintEffectDesc(36);
							}

							// Name magic elven bow

							var magicBowArtifact = gADB[50];

							Debug.Assert(magicBowArtifact != null);

							magicBowArtifact.SetCarriedByCharacter();

							gOut.Print("King Argas hands you the bow and one arrow.  You nock then let loose the arrow...");

							gOut.Write("{0}What will you name your bow? ", Environment.NewLine);

							gEngine.Buf.Clear();

							rc = gEngine.In.ReadField(gEngine.Buf, gEngine.CharArtNameLen, null, ' ', '\0', false, null, null, null, null);

							Debug.Assert(gEngine.IsSuccess(rc));

							gEngine.Buf.SetFormat("{0}", Regex.Replace(gEngine.Buf.ToString(), @"\s+", " ").Trim());

							magicBowArtifact.Name = gEngine.Capitalize(gEngine.Buf.ToString());

							var artifactHelper = gEngine.CreateInstance<IArtifactHelper>(x =>
							{
								x.Record = magicBowArtifact;
							});

							Debug.Assert(artifactHelper != null);

							if (!artifactHelper.ValidateField("Name") || magicBowArtifact.Name.Equals("NONE", StringComparison.OrdinalIgnoreCase))
							{
								magicBowArtifact.Name = "Whisperwind";
							}

							if (carryingLifeOrb)
							{
								gEngine.PrintEffectDesc(32);
							}
							else
							{
								gEngine.PrintEffectDesc(37);
							}

							gCharacter.HeldGold += 2000;

							gGameState.Die = 0;

							gEngine.ExitType = ExitType.FinishAdventure;

							gEngine.MainLoop.ShouldShutdown = true;
						}
						else     // The rabid animals attack
						{
							for (var i = 38; i <= 41; i++)
							{
								gEngine.PrintEffectDesc(i);
							}

							gOut.Print("You are dead.");

							gGameState.Die = 1;

							NextState = gEngine.CreateInstance<IPlayerDeadState>(x =>
							{
								x.PrintLineSep = true;
							});
						}
					}

					GotoCleanup = true;
				}
			}
		}
	}
}
