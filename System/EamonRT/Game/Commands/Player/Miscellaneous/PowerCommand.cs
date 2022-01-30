
// PowerCommand.cs

// Copyright (c) 2014+ by Michael Penner.  All rights reserved.

using System;
using Eamon.Framework;
using Eamon.Framework.Primitive.Enums;
using Eamon.Game.Attributes;
using EamonRT.Framework.Commands;
using EamonRT.Framework.Primitive.Enums;
using EamonRT.Framework.States;
using static EamonRT.Game.Plugin.PluginContext;

namespace EamonRT.Game.Commands
{
	[ClassMappings]
	public class PowerCommand : Command, IPowerCommand
	{
		public virtual bool CastSpell { get; set; }

		public virtual Func<IArtifact, bool>[] ResurrectWhereClauseFuncs { get; set; }

		public virtual Func<IArtifact, bool>[] VanishWhereClauseFuncs { get; set; }

		/// <summary></summary>
		public virtual ICommand RedirectCommand { get; set; }

		/// <summary></summary>
		public virtual long PowerEventRoll { get; set; }

		public override void Execute()
		{
			if (CastSpell && !gEngine.CheckPlayerSpellCast(Spell.Power, ShouldAllowSkillGains()))
			{
				goto Cleanup;
			}

			ProcessEvents(EventType.AfterCastSpellCheck);

			if (GotoCleanup)
			{
				goto Cleanup;
			}

			PowerEventRoll = gEngine.RollDice(1, 100, 0);

			if (!Globals.IsRulesetVersion(5, 15, 25))
			{
				// 50% chance of boom

				if (PowerEventRoll > 50)
				{
					PrintSonicBoom(ActorRoom);
				}

				// 50% chance of fortune cookie

				else
				{
					PrintFortuneCookie();
				}

				goto Cleanup;
			}

			// Raise the dead / Make stuff vanish

			if (gEngine.ResurrectDeadBodies(ActorRoom, ResurrectWhereClauseFuncs) || gEngine.MakeArtifactsVanish(ActorRoom, VanishWhereClauseFuncs))
			{
				goto Cleanup;
			}

			// 10% chance of death trap

			if (PowerEventRoll < 11)
			{
				PrintTunnelCollapses(ActorRoom);

				gGameState.Die = 1;

				NextState = Globals.CreateInstance<IPlayerDeadState>(x =>
				{
					x.PrintLineSep = true;
				});

				goto Cleanup;
			}

			// 75% chance of boom

			if (PowerEventRoll < 86)
			{
				PrintSonicBoom(ActorRoom);

				goto Cleanup;
			}

			// 5% chance of full heal

			if (PowerEventRoll > 95)
			{
				PrintAllWoundsHealed();

				ActorMonster.DmgTaken = 0;

				goto Cleanup;
			}

			// 10% chance of SPEED spell

			RedirectCommand = Globals.CreateInstance<ISpeedCommand>(x =>
			{
				x.CastSpell = false;
			});

			CopyCommandData(RedirectCommand);

			NextState = RedirectCommand;

		Cleanup:

			if (NextState == null)
			{
				NextState = Globals.CreateInstance<IMonsterStartState>();
			}
		}

		public PowerCommand()
		{
			SortOrder = 360;

			Uid = 60;

			Name = "PowerCommand";

			Verb = "power";

			Type = CommandType.Miscellaneous;

			CastSpell = true;
		}
	}
}
