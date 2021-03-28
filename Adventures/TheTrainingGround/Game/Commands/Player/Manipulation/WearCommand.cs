
// WearCommand.cs

// Copyright (c) 2014+ by Michael Penner.  All rights reserved.

using System.Diagnostics;
using Eamon.Game.Attributes;
using EamonRT.Framework.Commands;
using EamonRT.Framework.States;
using static TheTrainingGround.Game.Plugin.PluginContext;

namespace TheTrainingGround.Game.Commands
{
	[ClassMappings]
	public class WearCommand : EamonRT.Game.Commands.WearCommand, IWearCommand
	{
		public override void Execute()
		{
			Debug.Assert(DobjArtifact != null);

			// Can't wear backpack

			if (DobjArtifact.Uid == 13)
			{
				PrintDontNeedTo();

				NextState = Globals.CreateInstance<IStartState>();
			}
			else
			{
				base.Execute();
			}
		}
	}
}
