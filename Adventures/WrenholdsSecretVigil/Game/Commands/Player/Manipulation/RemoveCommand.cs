
// RemoveCommand.cs

// Copyright (c) 2014+ by Michael Penner.  All rights reserved.

using System.Diagnostics;
using Eamon.Game.Attributes;
using EamonRT.Framework.Commands;
using EamonRT.Framework.States;
using static WrenholdsSecretVigil.Game.Plugin.PluginContext;

namespace WrenholdsSecretVigil.Game.Commands
{
	[ClassMappings]
	public class RemoveCommand : EamonRT.Game.Commands.RemoveCommand, IRemoveCommand
	{
		public override void Execute()
		{
			Debug.Assert(DobjArtifact != null);

			// Remove orb from metal pedestal

			if (DobjArtifact.Uid == 4 && IobjArtifact != null && IobjArtifact.Uid == 43)
			{
				gOut.Print("{0} {1} stuck to {2} and won't budge.", DobjArtifact.GetTheName(true), DobjArtifact.EvalPlural("is", "are"), IobjArtifact.GetTheName(buf: Globals.Buf01));

				NextState = Globals.CreateInstance<IMonsterStartState>();
			}
			else
			{
				base.Execute();
			}
		}
	}
}
