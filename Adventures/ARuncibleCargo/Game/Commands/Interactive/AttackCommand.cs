
// AttackCommand.cs

// Copyright (c) 2014+ by Michael Penner.  All rights reserved.

using System.Diagnostics;
using Eamon.Game.Attributes;
using EamonRT.Framework.Commands;
using EamonRT.Framework.States;
using static ARuncibleCargo.Game.Plugin.Globals;

namespace ARuncibleCargo.Game.Commands
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
					case 30:
					case 80:

						if (BlastSpell)
						{
							PrintZapDirectHit();
						}
						else
						{
							PrintWhamHitObj(DobjArtifact);
						}

						// Can't attack oven or safe

						gEngine.PrintEffectDesc(162);

						NextState = gEngine.CreateInstance<IMonsterStartState>();

						break;

					case 41:

						if (BlastSpell)
						{
							PrintZapDirectHit();
						}
						else
						{
							PrintWhamHitObj(DobjArtifact);
						}

						// Attack cell = Open Jail

						var ac = DobjArtifact.InContainer;

						Debug.Assert(ac != null);

						ac.SetOpen(false);

						var command = gEngine.CreateInstance<IOpenCommand>();

						CopyCommandData(command);

						NextState = command;

						break;

					case 129:

						// Can't attack the Runcible Cargo

						gOut.Print("That sounds quite dangerous!");

						NextState = gEngine.CreateInstance<IMonsterStartState>();

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

		public override bool IsAllowedInRoom()
		{
			// Disable AttackCommand in water rooms

			return !gActorRoom(this).IsWaterRoom();
		}
	}
}
