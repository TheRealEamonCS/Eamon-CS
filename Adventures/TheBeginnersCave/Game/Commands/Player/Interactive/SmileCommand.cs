
// SmileCommand.cs

// Copyright (c) 2014+ by Michael Penner.  All rights reserved.

using Eamon.Framework.Primitive.Enums;
using Eamon.Game.Attributes;
using EamonRT.Framework.Commands;
using EamonRT.Framework.States;
using static TheBeginnersCave.Game.Plugin.PluginContext;

namespace TheBeginnersCave.Game.Commands
{
	[ClassMappings]
	public class SmileCommand : EamonRT.Game.Commands.SmileCommand, ISmileCommand
	{
		public override void Execute()
		{
			// historical response from original

			if (gGameState.GetNBTL(Friendliness.Enemy) > 0)
			{
				gOut.Print("As you smile, the enemy attacks you!");

				NextState = Globals.CreateInstance<IMonsterStartState>();
			}
			else
			{
				base.Execute();
			}
		}
	}
}
