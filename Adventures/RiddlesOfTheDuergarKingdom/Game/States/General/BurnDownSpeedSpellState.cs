
// BurnDownSpeedSpellState.cs

// Copyright (c) 2014+ by Michael Penner.  All rights reserved.

using Eamon.Game.Attributes;
using EamonRT.Framework.States;
using static RiddlesOfTheDuergarKingdom.Game.Plugin.PluginContext;

namespace RiddlesOfTheDuergarKingdom.Game.States
{
	[ClassMappings]
	public class BurnDownSpeedSpellState : EamonRT.Game.States.BurnDownSpeedSpellState, IBurnDownSpeedSpellState
	{
		public override void PrintSpeedSpellExpired()
		{
			if (gGameState.CoffeePotUsed)
			{
				gOut.Print("The effect of the coffee has worn off!");

				gGameState.CoffeePotUsed = false;
			}
			else
			{
				base.PrintSpeedSpellExpired();
			}
		}
	}
}
