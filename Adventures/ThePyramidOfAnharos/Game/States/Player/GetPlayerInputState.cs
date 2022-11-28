
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
		public override void ProcessEvents(EventType eventType)
		{
			base.ProcessEvents(eventType);

			if (eventType == EventType.BeforePrintCommandPrompt && gEngine.ShouldPreTurnProcess)
			{
				Debug.Assert(gCharMonster != null);

				var room = gCharMonster.GetInRoom();

				Debug.Assert(room != null);

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
