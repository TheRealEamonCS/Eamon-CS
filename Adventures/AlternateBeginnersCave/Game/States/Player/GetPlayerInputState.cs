
// GetPlayerInputState.cs

// Copyright (c) 2014+ by Michael Penner.  All rights reserved.

using System.Diagnostics;
using Eamon.Game.Attributes;
using EamonRT.Framework.Combat;
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
			if (eventType == EventType.BeforePrintCommandPrompt && ShouldPreTurnProcess())
			{
				Debug.Assert(gCharMonster != null);

				// Magic dagger - Temp Hardiness increase spell

				if (gGameState.MagicDaggerCounter > 0 && --gGameState.MagicDaggerCounter <= 0)
				{
					gGameState.MagicDaggerCounter = -1;

					gCharMonster.Hardiness -= 5;

					gOut.Print("The spell of the dagger just wore off!");

					var combatSystem = Globals.CreateInstance<ICombatSystem>(x =>
					{
						x.SetNextStateFunc = s => NextState = s;

						x.DfMonster = gCharMonster;
					});

					combatSystem.ExecuteCheckMonsterStatus();

					if (gGameState.Die > 0)
					{
						GotoCleanup = true;
					}
				}
			}

			base.ProcessEvents(eventType);
		}
	}
}
