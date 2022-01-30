
// CommandImpl.cs

// Copyright (c) 2014+ by Michael Penner.  All rights reserved.

using System.Diagnostics;
using Eamon.Framework;
using Eamon.Game.Attributes;
using EamonRT.Framework.Commands;
using static RiddlesOfTheDuergarKingdom.Game.Plugin.PluginContext;

namespace RiddlesOfTheDuergarKingdom.Game.Commands
{
	[ClassMappings]
	public class CommandImpl : EamonRT.Game.Commands.CommandImpl, ICommandImpl
	{
		public override void PrintCantVerbThat(IArtifact artifact)
		{
			Debug.Assert(artifact != null);

			// Ore cart

			if (artifact.Uid == 46)
			{
				gEngine.PrintEffectDesc(60);
			}
			else
			{
				base.PrintCantVerbThat(artifact);
			}
		}

		public override void PrintOpened(IArtifact artifact)
		{
			Debug.Assert(artifact != null);

			// Stone cabin / stone outhouse / stone hydrostation

			if (artifact.Uid == 22 || artifact.Uid == 23 || artifact.Uid == 24)
			{
				gOut.Print("{0} door opened.", artifact.GetNoneName(true, false));
			}
			else
			{
				base.PrintOpened(artifact);
			}
		}

		public override void PrintClosed(IArtifact artifact)
		{
			Debug.Assert(artifact != null);

			// Stone cabin / stone outhouse / stone hydrostation

			if (artifact.Uid == 22 || artifact.Uid == 23 || artifact.Uid == 24)
			{
				gOut.Print("{0} door closed.", artifact.GetNoneName(true, false));
			}
			else
			{
				base.PrintClosed(artifact);
			}
		}

		public override void PrintWontOpen(IArtifact artifact)
		{
			Debug.Assert(artifact != null);

			// Iron gate

			if (artifact.Uid == 5)
			{
				PrintLocked(artifact);
			}
			else
			{
				base.PrintWontOpen(artifact);
			}
		}

		public override void PrintGiveObjToActor(IArtifact artifact, IMonster monster)
		{
			Debug.Assert(artifact != null && monster != null);

			// Give highland ibex the leather panniers

			if (monster.Uid == 21 && artifact.Uid == 93)
			{
				gEngine.PrintEffectDesc(93);
			}
			else
			{
				base.PrintGiveObjToActor(artifact, monster);
			}
		}
	}
}
