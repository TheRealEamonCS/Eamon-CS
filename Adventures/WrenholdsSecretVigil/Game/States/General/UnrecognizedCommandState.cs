
// UnrecognizedCommandState.cs

// Copyright (c) 2014+ by Michael Penner.  All rights reserved.

using Eamon.Game.Attributes;
using EamonRT.Framework.States;
using static WrenholdsSecretVigil.Game.Plugin.PluginContext;

namespace WrenholdsSecretVigil.Game.States
{
	[ClassMappings]
	public class UnrecognizedCommandState : EamonRT.Game.States.UnrecognizedCommandState, IUnrecognizedCommandState
	{
		public override void Execute()
		{
			gOut.Print("Pray thee adventurer, please use these commands ---");

			base.Execute();
		}
	}
}
