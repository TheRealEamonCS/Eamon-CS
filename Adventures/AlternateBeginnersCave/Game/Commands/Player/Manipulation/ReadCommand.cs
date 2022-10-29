
// ReadCommand.cs

// Copyright (c) 2014+ by Michael Penner.  All rights reserved.

using System.Diagnostics;
using Eamon.Game.Attributes;
using EamonRT.Framework.Commands;
using EamonRT.Framework.States;
using static AlternateBeginnersCave.Game.Plugin.Globals;

namespace AlternateBeginnersCave.Game.Commands
{
	[ClassMappings]
	public class ReadCommand : EamonRT.Game.Commands.ReadCommand, IReadCommand
	{
		public override void Execute()
		{
			Debug.Assert(DobjArtifact != null);

			// Unremarkable box

			if (DobjArtifact.Uid == 7)
			{
				gEngine.PrintEffectDesc(13);

				NextState = gEngine.CreateInstance<IMonsterStartState>();
			}

			// Sign in first room

			else if (DobjArtifact.Uid == 23)
			{
				var command = gEngine.CreateInstance<IExamineCommand>();

				CopyCommandData(command);

				NextState = command;
			}
			else
			{
				base.Execute();
			}
		}
	}
}
