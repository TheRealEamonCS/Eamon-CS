
// LightCommand.cs

// Copyright (c) 2014+ by Michael Penner.  All rights reserved.

using System.Diagnostics;
using Eamon.Game.Attributes;
using EamonRT.Framework.Commands;
using EamonRT.Framework.States;
using static ThePyramidOfAnharos.Game.Plugin.Globals;

namespace ThePyramidOfAnharos.Game.Commands
{
	[ClassMappings]
	public class LightCommand : EamonRT.Game.Commands.LightCommand, ILightCommand
	{
		public override void Execute()
		{
			Debug.Assert(DobjArtifact != null);

			// Can't light torch in Black room

			if ((DobjArtifact.Uid == 16 || DobjArtifact.Uid == 17) && ActorRoom.Uid == 39)
			{
				PrintWontLight(DobjArtifact);

				NextState = gEngine.CreateInstance<IMonsterStartState>();
			}
			else
			{
				base.Execute();
			}
		}
	}
}
