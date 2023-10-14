
// AttackCommand.cs

// Copyright (c) 2014+ by Michael Penner.  All rights reserved.

using System.Diagnostics;
using Eamon.Framework.Primitive.Enums;
using Eamon.Game.Attributes;
using EamonRT.Framework.Commands;
using EamonRT.Framework.States;
using static TheSubAquanLaboratory.Game.Plugin.Globals;

namespace TheSubAquanLaboratory.Game.Commands
{
	[ClassMappings]
	public class AttackCommand : EamonRT.Game.Commands.AttackCommand, IAttackCommand
	{
		public override void ExecuteForPlayer()
		{
			Debug.Assert(DobjArtifact != null || DobjMonster != null);

			if ((BlastSpell || ActorMonster.Weapon > 0) && DobjArtifact != null)
			{
				switch (DobjArtifact.Uid)
				{
					case 83:
					case 84:

						// Fake-looking back wall / Glass walls

						gEngine.ProcessWallAttack(ActorRoom, ActorMonster, DobjArtifact, BlastSpell);

						NextState = gGameState.GetNBTL(Friendliness.Enemy) > 0 && gEngine.ApplyWallDamage(ActorRoom) ? gEngine.CreateInstance<IStartState>() : (IState)gEngine.CreateInstance<IMonsterStartState>();

						break;

					case 85:

						// Electrified floor

						gEngine.PrintEffectDesc(65);

						DobjArtifact.SetInLimbo();

						var brokenFloorTrapArtifact = gADB[107];

						Debug.Assert(brokenFloorTrapArtifact != null);

						brokenFloorTrapArtifact.SetInRoom(ActorRoom);

						NextState = gEngine.CreateInstance<IMonsterStartState>();

						break;

					case 89:

						// Dismantled worker android

						base.ExecuteForPlayer();

						var plasticCardArtifact = gADB[82];

						Debug.Assert(plasticCardArtifact != null);

						if (plasticCardArtifact.IsInLimbo())
						{
							gEngine.MiscEventFuncList02.Add(() =>
							{
								plasticCardArtifact.Desc = "Destroying the remains of the android reveals a small featureless card made out of a durable plastic.";

								gOut.Print("{0}", plasticCardArtifact.Desc);

								plasticCardArtifact.SetInRoom(ActorRoom);

								plasticCardArtifact.Seen = true;
							});
						}

						break;

					default:

						base.ExecuteForPlayer();

						break;
				}
			}
			else
			{
				base.ExecuteForPlayer();
			}
		}
	}
}
