
// MonsterHealCommand.cs

// Copyright (c) 2014+ by Michael Penner.  All rights reserved.

using Eamon.Framework.Primitive.Enums;
using Eamon.Game.Attributes;
using EamonRT.Framework.Commands;
using EamonRT.Framework.States;
using static EamonRT.Game.Plugin.PluginContext;

namespace EamonRT.Game.Commands
{
	[ClassMappings]
	public class MonsterHealCommand : Command, IMonsterHealCommand
	{
		public virtual bool CastSpell { get; set; }

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

		public MonsterHealCommand()
		{
			SortOrder = 860;

			IsPlayerEnabled = false;

			IsMonsterEnabled = true;

			Uid = 20;

			Name = "MonsterHealCommand";

			Verb = "heal";

			Type = CommandType.Interactive;

			CastSpell = true;
		}
	}
}
