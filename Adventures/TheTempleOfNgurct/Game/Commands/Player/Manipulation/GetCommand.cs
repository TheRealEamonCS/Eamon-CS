
// GetCommand.cs

// Copyright (c) 2014+ by Michael Penner.  All rights reserved.

using System;
using System.Collections.Generic;
using System.Diagnostics;
using Eamon.Framework;
using Eamon.Framework.Primitive.Enums;
using Eamon.Game.Attributes;
using EamonRT.Framework.Combat;
using EamonRT.Framework.Commands;
using EamonRT.Framework.States;
using static TheTempleOfNgurct.Game.Plugin.PluginContext;

namespace TheTempleOfNgurct.Game.Commands
{
	[ClassMappings]
	public class GetCommand : EamonRT.Game.Commands.GetCommand, IGetCommand
	{
		public override void Execute()
		{
			if (DobjArtifact != null && DobjArtifact.GeneralWeapon == null && gGameState.GetNBTL(Friendliness.Enemy) > 0)
			{
				PrintEnemiesNearby();

				NextState = Globals.CreateInstance<IStartState>();
			}
			else
			{
				base.Execute();

				var scimitarArtifact = gADB[41];

				Debug.Assert(scimitarArtifact != null);

				// Alignment conflict

				if (!gGameState.AlignmentConflict && scimitarArtifact.IsCarriedByCharacter())
				{
					gEngine.PrintEffectDesc(28);

					var combatSystem = Globals.CreateInstance<ICombatSystem>(x =>
					{
						x.SetNextStateFunc = s => NextState = s;

						x.DfMonster = ActorMonster;

						x.OmitArmor = true;
					});

					combatSystem.ExecuteCalculateDamage(1, 8, 1);

					gGameState.AlignmentConflict = true;
				}
			}
		}
	}
}
