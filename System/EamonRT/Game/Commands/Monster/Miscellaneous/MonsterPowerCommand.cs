
// MonsterPowerCommand.cs

// Copyright (c) 2014+ by Michael Penner.  All rights reserved.

using System;
using Eamon.Framework;
using Eamon.Framework.Primitive.Enums;
using Eamon.Game.Attributes;
using EamonRT.Framework.Commands;
using EamonRT.Framework.States;
using static EamonRT.Game.Plugin.PluginContext;

namespace EamonRT.Game.Commands
{
	[ClassMappings]
	public class MonsterPowerCommand : Command, IMonsterPowerCommand
	{
		public virtual bool CastSpell { get; set; }

		public virtual Func<IArtifact, bool>[] ResurrectWhereClauseFuncs { get; set; }

		public virtual Func<IArtifact, bool>[] VanishWhereClauseFuncs { get; set; }

		/// <summary></summary>
		public virtual long FortuneCookieRoll { get; set; }

		public override void Execute()
		{

			if (NextState == null)
			{
				NextState = Globals.CreateInstance<IErrorState>(x =>
				{
					x.ErrorMessage = string.Format("{0}: NextState == null", Name);
				});
			}
		}

		public MonsterPowerCommand()
		{
			SortOrder = 880;

			IsPlayerEnabled = false;

			IsMonsterEnabled = true;

			Uid = 24;

			Name = "MonsterPowerCommand";

			Verb = "power";

			Type = CommandType.Miscellaneous;

			CastSpell = true;
		}
	}
}
