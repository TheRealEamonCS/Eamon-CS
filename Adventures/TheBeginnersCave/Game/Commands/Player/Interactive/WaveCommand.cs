
// WaveCommand.cs

// Copyright (c) 2014+ by Michael Penner.  All rights reserved.

using Eamon.Framework.Primitive.Enums;
using Eamon.Game.Attributes;
using EamonRT.Framework.Commands;
using EamonRT.Framework.States;
using static TheBeginnersCave.Game.Plugin.Globals;

namespace TheBeginnersCave.Game.Commands
{
	[ClassMappings]
	public class WaveCommand : EamonRT.Game.Commands.WaveCommand, IWaveCommand
	{
		public override void Execute()
		{
			// historical response from original

			if (gGameState.GetNBTL(Friendliness.Enemy) > 0)
			{
				gOut.Print("As you wave, the enemy attacks you!");

				NextState = gEngine.CreateInstance<IMonsterStartState>();
			}
			else
			{
				base.Execute();
			}
		}
	}
}
