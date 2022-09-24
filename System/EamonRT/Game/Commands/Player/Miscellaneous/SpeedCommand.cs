
// SpeedCommand.cs

// Copyright (c) 2014+ by Michael Penner.  All rights reserved.

using Eamon.Framework.Primitive.Enums;
using Eamon.Game.Attributes;
using EamonRT.Framework.Commands;
using EamonRT.Framework.Components;
using EamonRT.Framework.States;
using static EamonRT.Game.Plugin.PluginContext;

namespace EamonRT.Game.Commands
{
	[ClassMappings]
	public class SpeedCommand : Command, ISpeedCommand
	{
		public virtual bool CastSpell { get; set; }

		/// <summary></summary>
		public virtual IMagicComponent MagicComponent { get; set; }

		public override void Execute()
		{
			MagicComponent = Globals.CreateInstance<IMagicComponent>(x =>
			{
				x.SetNextStateFunc = s => NextState = s;

				x.ActorMonster = ActorMonster;

				x.ActorRoom = ActorRoom;

				x.OmitSkillGains = !ShouldAllowSkillGains();

				x.CastSpell = CastSpell;
			});

			MagicComponent.ExecuteSpeedSpell();

			if (NextState == null)
			{
				NextState = Globals.CreateInstance<IMonsterStartState>();
			}
		}

		public SpeedCommand()
		{
			SortOrder = 350;

			Name = "SpeedCommand";

			Verb = "speed";

			Type = CommandType.Miscellaneous;

			CastSpell = true;
		}
	}
}
