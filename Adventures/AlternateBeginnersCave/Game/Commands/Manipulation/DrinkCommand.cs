﻿
// DrinkCommand.cs

// Copyright (c) 2014+ by Michael Penner.  All rights reserved.

using System.Diagnostics;
using Eamon.Game.Attributes;
using EamonRT.Framework.Commands;
using EamonRT.Framework.States;
using static AlternateBeginnersCave.Game.Plugin.Globals;

namespace AlternateBeginnersCave.Game.Commands
{
	[ClassMappings]
	public class DrinkCommand : EamonRT.Game.Commands.DrinkCommand, IDrinkCommand
	{
		public override void ExecuteForPlayer()
		{
			Debug.Assert(DobjArtifact != null);

			// Waspicide and vial

			if (DobjArtifact.Uid == 5 || DobjArtifact.Uid == 6)
			{
				DobjArtifact.SetInLimbo();

				var effectUid = 6;

				if (DobjArtifact.Uid == 6)
				{
					effectUid = 11;

					gGameState.DrankVial = true;
				}

				gEngine.PrintEffectDesc(effectUid);

				NextState = gEngine.CreateInstance<IMonsterStartState>();
			}
			else
			{
				base.ExecuteForPlayer();
			}
		}
	}
}
