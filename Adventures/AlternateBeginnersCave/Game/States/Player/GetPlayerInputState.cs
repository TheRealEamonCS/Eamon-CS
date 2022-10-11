
// GetPlayerInputState.cs

// Copyright (c) 2014+ by Michael Penner.  All rights reserved.

using System.Diagnostics;
using Eamon.Game.Attributes;
using EamonRT.Framework.Components;
using EamonRT.Framework.Primitive.Enums;
using EamonRT.Framework.States;
using static AlternateBeginnersCave.Game.Plugin.PluginContext;

namespace AlternateBeginnersCave.Game.States
{
	[ClassMappings]
	public class GetPlayerInputState : EamonRT.Game.States.GetPlayerInputState, IGetPlayerInputState
	{
		public override void ProcessEvents(EventType eventType)
		{
			base.ProcessEvents(eventType);

			if (eventType == EventType.BeforePrintCommandPrompt && Globals.ShouldPreTurnProcess)
			{
				Debug.Assert(gCharMonster != null);

				// Magic dagger - Temp Hardiness increase spell

				if (gGameState.MagicDaggerCounter > 0 && --gGameState.MagicDaggerCounter <= 0)
				{
					gGameState.MagicDaggerCounter = -1;

					gCharMonster.Hardiness -= 5;

					gOut.Print("The spell of the dagger just wore off!");

					var combatComponent = Globals.CreateInstance<ICombatComponent>(x =>
					{
						x.SetNextStateFunc = s => NextState = s;

						x.ActorRoom = gCharMonster.GetInRoom();

						x.Dobj = gCharMonster;
					});

					combatComponent.ExecuteCheckMonsterStatus();

					if (gGameState.Die > 0)
					{
						GotoCleanup = true;
					}
				}
			}
		}
	}
}
