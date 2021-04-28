
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

		/// <summary></summary>
		public virtual long FortuneCookieRoll { get; set; }

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

			if (!Globals.IsRulesetVersion(5, 15))
			{
				// 50% chance of boom

				if (PowerEventRoll > 50)
				{
					PrintSonicBoom();
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
				gOut.Print("The section of {0} collapses and you die.", ActorRoom.EvalRoomType("tunnel you are in", "ground you are on"));

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
				PrintSonicBoom();

				goto Cleanup;
			}

			// 5% chance of full heal

			if (PowerEventRoll > 95)
			{
				gOut.Print("All of your wounds are healed.");

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

		public virtual void PrintSonicBoom()
		{
			if (Globals.IsRulesetVersion(5, 15))
			{
				gOut.Print("You hear a very loud sonic boom that echoes through the {0}.", ActorRoom.EvalRoomType("tunnels", "area"));
			}
			else
			{
				gOut.Print("You hear a loud sonic boom which echoes all around you!");
			}
		}

		public virtual void PrintFortuneCookie()
		{
			FortuneCookieRoll = gEngine.RollDice(1, 100, 0);

			gOut.Print("A fortune cookie appears in mid-air and explodes!  The smoking paper left behind reads, \"{0}\"  How strange.",
				FortuneCookieRoll > 50 ?
				"THE SECTION OF TUNNEL YOU ARE IN COLLAPSES AND YOU DIE." :
				"YOU SUDDENLY FIND YOU CANNOT CARRY ALL OF THE ITEMS YOU ARE CARRYING, AND THEY ALL FALL TO THE GROUND.");
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
