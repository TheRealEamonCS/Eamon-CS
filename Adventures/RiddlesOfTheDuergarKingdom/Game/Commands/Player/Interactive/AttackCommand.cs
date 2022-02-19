
// AttackCommand.cs

// Copyright (c) 2014+ by Michael Penner.  All rights reserved.

using System.Diagnostics;
using Eamon.Game.Attributes;
using EamonRT.Framework.Commands;
using EamonRT.Framework.States;
using static RiddlesOfTheDuergarKingdom.Game.Plugin.PluginContext;

namespace RiddlesOfTheDuergarKingdom.Game.Commands
{
	[ClassMappings]
	public class AttackCommand : EamonRT.Game.Commands.AttackCommand, IAttackCommand
	{
		public override void Execute()
		{
			Debug.Assert(DobjArtifact != null || DobjMonster != null);

			// Player attacks black spider

			if (!BlastSpell && DobjMonster != null && DobjMonster.Uid == 16)
			{
				Globals.PlayerAttacksBlackSpider = true;

				var weaponUid = ActorMonster.Weapon;

				ActorMonster.Weapon = 0;

				base.Execute();

				ActorMonster.Weapon = weaponUid;

				Globals.PlayerAttacksBlackSpider = false;
			}

			// Player blasts/attacks various artifacts

			else if ((BlastSpell || ActorMonster.Weapon > 0) && DobjArtifact != null)
			{
				switch (DobjArtifact.Uid)
				{
					case 15:
					case 18:

						// Attack iron lever or hand winch while wooden cart suspended = Death

						if (gGameState.WinchCounter > 0)
						{
							if (!BlastSpell)
							{
								PrintWhamHitObj(DobjArtifact);
							}

							gEngine.PrintEffectDesc(11);

							gGameState.Die = 1;

							NextState = Globals.CreateInstance<IPlayerDeadState>(x =>
							{
								x.PrintLineSep = true;
							});
						}
						else
						{
							base.Execute();
						}

						break;

					case 39:

						// Attack wooden support beams = Death

						if (!BlastSpell)
						{
							PrintWhamHitObj(DobjArtifact);
						}

						gEngine.PrintEffectDesc(63);

						gGameState.Die = 1;

						NextState = Globals.CreateInstance<IPlayerDeadState>(x =>
						{
							x.PrintLineSep = true;
						});

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
	}
}
