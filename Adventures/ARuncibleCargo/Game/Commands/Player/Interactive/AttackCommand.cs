
// AttackCommand.cs

// Copyright (c) 2014+ by Michael Penner.  All rights reserved.

using System.Diagnostics;
using Eamon.Game.Attributes;
using EamonRT.Framework.Commands;
using EamonRT.Framework.States;
using static ARuncibleCargo.Game.Plugin.PluginContext;

namespace ARuncibleCargo.Game.Commands
{
	[ClassMappings]
	public class AttackCommand : EamonRT.Game.Commands.AttackCommand, IAttackCommand
	{
		public override void Execute()
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

						NextState = Globals.CreateInstance<IMonsterStartState>();

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

						gEngine.CheckActionList(Globals.SkillIncreaseFuncList);

						// Attack cell = Open Jail

						var ac = DobjArtifact.InContainer;

						Debug.Assert(ac != null);

						ac.SetOpen(false);

						var command = Globals.CreateInstance<IOpenCommand>();

						CopyCommandData(command);

						NextState = command;

						break;

					case 129:

						// Can't attack the Runcible Cargo

						gOut.Print("That sounds quite dangerous!");

						NextState = Globals.CreateInstance<IMonsterStartState>();

						break;

					default:

						base.Execute();

						break;
				}
			}
			else
			{
				base.Execute();
			}
		}

		public override bool IsAllowedInRoom()
		{
			// Disable AttackCommand in water rooms

			return !gActorRoom(this).IsWaterRoom();
		}
	}
}
