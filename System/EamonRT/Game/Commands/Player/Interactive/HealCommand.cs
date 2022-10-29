
// HealCommand.cs

// Copyright (c) 2014+ by Michael Penner.  All rights reserved.

using System.Diagnostics;
using Eamon.Framework.Primitive.Enums;
using Eamon.Game.Attributes;
using EamonRT.Framework.Commands;
using EamonRT.Framework.Components;
using EamonRT.Framework.States;
using static EamonRT.Game.Plugin.Globals;

namespace EamonRT.Game.Commands
{
	[ClassMappings]
	public class HealCommand : Command, IHealCommand
	{
		public virtual bool CastSpell { get; set; }

		/// <summary></summary>
		public virtual IMagicComponent MagicComponent { get; set; }

		public override void Execute()
		{
			Debug.Assert(DobjMonster != null);

			MagicComponent = gEngine.CreateInstance<IMagicComponent>(x =>
			{
				x.SetNextStateFunc = s => NextState = s;

				x.ActorMonster = ActorMonster;

				x.ActorRoom = ActorRoom;

				x.Dobj = DobjMonster;

				x.OmitSkillGains = !ShouldAllowSkillGains();

				x.CastSpell = CastSpell;
			});

			MagicComponent.ExecuteHealSpell();

			if (NextState == null)
			{
				NextState = gEngine.CreateInstance<IMonsterStartState>();
			}
		}

		public HealCommand()
		{
			SortOrder = 290;

			Name = "HealCommand";

			Verb = "heal";

			Type = CommandType.Interactive;

			CastSpell = true;
		}
	}
}
