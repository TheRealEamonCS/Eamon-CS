
// OpenCommand.cs

// Copyright (c) 2014+ by Michael Penner.  All rights reserved.

using System.Diagnostics;
using Eamon.Game.Attributes;
using EamonRT.Framework.Commands;
using EamonRT.Framework.States;
using static BeginnersCaveII.Game.Plugin.PluginContext;

namespace BeginnersCaveII.Game.Commands
{
	[ClassMappings]
	public class OpenCommand : EamonRT.Game.Commands.OpenCommand, IOpenCommand
	{
		public override void Execute()
		{
			Debug.Assert(DobjArtifact != null);

			// open treasure chest

			if (DobjArtifact.Uid == 5)
			{
				gEngine.PrintEffectDesc(1);

				NextState = Globals.CreateInstance<IMonsterStartState>();
			}
			else
			{
				base.Execute();
			}
		}
	}
}
