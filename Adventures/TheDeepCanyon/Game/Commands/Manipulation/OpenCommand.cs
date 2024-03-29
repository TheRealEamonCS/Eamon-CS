﻿
// OpenCommand.cs

// Copyright (c) 2014+ by Michael Penner.  All rights reserved.

using System.Diagnostics;
using Eamon.Game.Attributes;
using EamonRT.Framework.Commands;
using EamonRT.Framework.States;
using static TheDeepCanyon.Game.Plugin.Globals;

namespace TheDeepCanyon.Game.Commands
{
	[ClassMappings]
	public class OpenCommand : EamonRT.Game.Commands.OpenCommand, IOpenCommand
	{
		public override void ExecuteForPlayer()
		{
			Debug.Assert(DobjArtifact != null);

			// Leather bag

			if (DobjArtifact.Uid == 2)
			{
				gOut.Print("The gold dust is in there, alright.");

				NextState = gEngine.CreateInstance<IMonsterStartState>();
			}
			else
			{
				base.ExecuteForPlayer();
			}
		}
	}
}
