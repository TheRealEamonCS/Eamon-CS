
// UnrecognizedCommandState.cs

// Copyright (c) 2014+ by Michael Penner.  All rights reserved.

using Eamon.Game.Attributes;
using EamonRT.Framework.States;
using static TheVileGrimoireOfJaldial.Game.Plugin.PluginContext;

namespace TheVileGrimoireOfJaldial.Game.States
{
	[ClassMappings]
	public class UnrecognizedCommandState : EamonRT.Game.States.UnrecognizedCommandState, IUnrecognizedCommandState
	{
		public override void Execute()
		{
			gOut.Print("What's that?  I only understand these commands ---");

			base.Execute();

			gOut.Print("You may press ENTER to repeat the last action.");
		}
	}
}
