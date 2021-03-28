
// ReadyCommand.cs

// Copyright (c) 2014+ by Michael Penner.  All rights reserved.

using System;
using System.Diagnostics;
using Eamon.Game.Attributes;
using EamonRT.Framework.Commands;
using static WrenholdsSecretVigil.Game.Plugin.PluginContext;

namespace WrenholdsSecretVigil.Game.Commands
{
	[ClassMappings]
	public class ReadyCommand : EamonRT.Game.Commands.ReadyCommand, IReadyCommand
	{
		public override void Execute()
		{
			Debug.Assert(DobjArtifact != null);

			base.Execute();

			// Player readies Trollsfire

			if (ActorMonster.Weapon == DobjArtifact.Uid && DobjArtifact.Name.Equals("Trollsfire", StringComparison.OrdinalIgnoreCase) && DobjArtifact.Field4 == 10)
			{
				gEngine.PrintEffectDesc(6);
			}
		}
	}
}
