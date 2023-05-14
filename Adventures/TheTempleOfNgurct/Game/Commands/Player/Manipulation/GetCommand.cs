
// GetCommand.cs

// Copyright (c) 2014+ by Michael Penner.  All rights reserved.

using System.Diagnostics;
using Eamon.Framework.Primitive.Enums;
using Eamon.Game.Attributes;
using EamonRT.Framework.Commands;
using EamonRT.Framework.Components;
using EamonRT.Framework.States;
using static TheTempleOfNgurct.Game.Plugin.Globals;

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

				NextState = gEngine.CreateInstance<IStartState>();
			}
			else
			{
				base.Execute();

				var scimitarArtifact = gADB[41];

				Debug.Assert(scimitarArtifact != null);

				// Alignment conflict

				if (!gGameState.AlignmentConflict && scimitarArtifact.IsCarriedByMonster(ActorMonster))
				{
					gEngine.PrintEffectDesc(28);

					var combatComponent = gEngine.CreateInstance<ICombatComponent>(x =>
					{
						x.SetNextStateFunc = s => NextState = s;

						x.ActorRoom = ActorRoom;

						x.Dobj = ActorMonster;

						x.OmitArmor = true;
					});

					combatComponent.ExecuteCalculateDamage(1, 8, 1);

					gGameState.AlignmentConflict = true;
				}
			}
		}
	}
}
