
// UseCommand.cs

// Copyright (c) 2014+ by Michael Penner.  All rights reserved.

using Eamon.Game.Attributes;
using EamonRT.Framework.Commands;
using EamonRT.Framework.Primitive.Enums;
using EamonRT.Framework.States;
using static RiddlesOfTheDuergarQuarry.Game.Plugin.PluginContext;

namespace RiddlesOfTheDuergarQuarry.Game.Commands
{
	[ClassMappings]
	public class UseCommand : EamonRT.Game.Commands.UseCommand, IUseCommand
	{
		public override void ProcessEvents(EventType eventType)
		{
			if (eventType == EventType.BeforeUseArtifact)
			{
				switch (DobjArtifact.Uid)
				{
					case 79:

						// Brass Bell

						gEngine.PrintEffectDesc(39);

						NextState = Globals.CreateInstance<IMonsterStartState>();

						GotoCleanup = true;

						break;

					default:

						base.ProcessEvents(eventType);

						break;
				}
			}
			else
			{
				base.ProcessEvents(eventType);
			}
		}
	}
}
