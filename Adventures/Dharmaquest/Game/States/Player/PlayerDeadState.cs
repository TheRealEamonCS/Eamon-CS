
// PlayerDeadState.cs

// Copyright (c) 2014+ by Michael Penner.  All rights reserved.

using Eamon.Game.Attributes;
using EamonRT.Framework.States;
using static Dharmaquest.Game.Plugin.Globals;

namespace Dharmaquest.Game.States
{
	[ClassMappings]
	public class PlayerDeadState : EamonRT.Game.States.PlayerDeadState, IPlayerDeadState
	{
		public override void Execute()
		{
			var lastWordsArray = new string[] 
			{
				"whisper a prayer to Odin",
				"offer your life to Wotan",
				"snarl a curse at your killer",
				"think of the 20 gold pieces still owed to the tavern keeper",
				"see the valkyrie coming to take you to Valhalla",
				"pray to Poseidon to avenge you",
				"call on Apollo to strike down your killer",
				"think of home and close your eyes",
				"laugh at your attacker",
				"think of Sieglinde, the blonde adventuress from the Main Hall"
			};

			var lastWordsString = gEngine.GetRandomElement(lastWordsArray);

			// Adventurer's last words

			gOut.Print("In the last moments of consciousness you {0}.", lastWordsString);

			base.Execute();
		}
	}
}
