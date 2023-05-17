
// AttackCommand.cs

// Copyright (c) 2014+ by Michael Penner.  All rights reserved.

using System.Diagnostics;
using Eamon.Game.Attributes;
using EamonRT.Framework.Commands;
using EamonRT.Framework.States;
using static RiddlesOfTheDuergarKingdom.Game.Plugin.Globals;

namespace RiddlesOfTheDuergarKingdom.Game.Commands
{
	[ClassMappings]
	public class AttackCommand : EamonRT.Game.Commands.AttackCommand, IAttackCommand
	{
		public override void ExecuteForPlayer()
		{
			Debug.Assert(DobjArtifact != null || DobjMonster != null);

			// Player attacks black spider

			if (!BlastSpell && DobjMonster != null && DobjMonster.Uid == 16)
			{
				gEngine.PlayerAttacksBlackSpider = true;

				var weaponUid = ActorMonster.Weapon;

				ActorMonster.Weapon = 0;

				base.ExecuteForPlayer();

				ActorMonster.Weapon = weaponUid;

				gEngine.PlayerAttacksBlackSpider = false;
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

							NextState = gEngine.CreateInstance<IPlayerDeadState>(x =>
							{
								x.PrintLineSep = true;
							});
						}
						else
						{
							base.ExecuteForPlayer();
						}

						break;

					case 39:

						if (!BlastSpell)
						{
							PrintWhamHitObj(DobjArtifact);
						}

						// Attack wooden support beams = Death

						gEngine.PrintEffectDesc(63);

						gGameState.Die = 1;

						NextState = gEngine.CreateInstance<IPlayerDeadState>(x =>
						{
							x.PrintLineSep = true;
						});

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
