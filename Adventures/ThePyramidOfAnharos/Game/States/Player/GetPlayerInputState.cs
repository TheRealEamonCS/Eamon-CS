
// GetPlayerInputState.cs

// Copyright (c) 2014+ by Michael Penner.  All rights reserved.

using System;
using System.Diagnostics;
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
		public virtual ICombatComponent CombatComponent { get; set; }

		public override void ProcessEvents(EventType eventType)
		{
			base.ProcessEvents(eventType);

			if (eventType == EventType.BeforePrintCommandPrompt && gEngine.ShouldPreTurnProcess)
			{
				Debug.Assert(gCharMonster != null);

				var room = gCharMonster.GetInRoom();

				Debug.Assert(room != null);

				var guardsMonster = gMDB[20];

				Debug.Assert(guardsMonster != null);

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
						CombatComponent = gEngine.CreateInstance<ICombatComponent>(x =>
						{
							x.SetNextStateFunc = s => NextState = s;

							x.ActorMonster = guardsMonster;

							x.ActorRoom = room;

							x.Dobj = gCharMonster;

							x.MemberNumber = i;

							x.AttackNumber = 1;
						});

						CombatComponent.ExecuteAttack();

						if (gGameState.Die > 0)
						{
							GotoCleanup = true;

							goto Cleanup;
						}
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

				gEngine.PrintGuideMonsterDirection();
			}

		Cleanup:

			;
		}
	}
}
