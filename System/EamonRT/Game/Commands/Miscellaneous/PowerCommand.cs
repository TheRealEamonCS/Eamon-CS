﻿
// PowerCommand.cs

// Copyright (c) 2014+ by Michael Penner.  All rights reserved.

using System;
using Eamon.Framework;
using Eamon.Framework.Primitive.Enums;
using Eamon.Game.Attributes;
using EamonRT.Framework.Commands;
using EamonRT.Framework.Components;
using EamonRT.Framework.States;
using static EamonRT.Game.Plugin.Globals;

namespace EamonRT.Game.Commands
{
	[ClassMappings]
	public class PowerCommand : Command, IPowerCommand
	{
		public virtual Func<IArtifact, bool>[] ResurrectWhereClauseFuncs { get; set; }

		public virtual Func<IArtifact, bool>[] VanishWhereClauseFuncs { get; set; }

		public virtual bool CastSpell { get; set; }

		/// <summary></summary>
		public virtual long FortuneCookieRoll { get; set; }

		/// <summary></summary>
		public virtual IMagicComponent MagicComponent { get; set; }

		public override void ExecuteForPlayer()
		{
			MagicComponent = gEngine.CreateInstance<IMagicComponent>(x =>
			{
				x.SetNextStateFunc = s => NextState = s;

				x.ActorMonster = ActorMonster;

				x.ActorRoom = ActorRoom;

				x.OmitSkillGains = !ShouldAllowSkillGains();

				x.ResurrectWhereClauseFuncs = ResurrectWhereClauseFuncs;

				x.VanishWhereClauseFuncs = VanishWhereClauseFuncs;

				x.CastSpell = CastSpell;
			});

			MagicComponent.ExecutePowerSpell();

			if (NextState == null)
			{
				NextState = gEngine.CreateInstance<IMonsterStartState>();
			}
		}

		public override void ExecuteForMonster()
		{

			if (NextState == null)
			{
				NextState = gEngine.CreateInstance<IErrorState>(x =>
				{
					x.ErrorMessage = string.Format("{0}: NextState == null", Name);
				});
			}
		}

		public PowerCommand()
		{
			SortOrder = 360;

			IsMonsterEnabled = true;

			Name = "PowerCommand";

			Verb = "power";

			Type = CommandType.Miscellaneous;

			CastSpell = true;
		}
	}
}
