
// MonsterAttackCommand.cs

// Copyright (c) 2014+ by Michael Penner.  All rights reserved.

using System.Diagnostics;
using Eamon.Framework.Primitive.Enums;
using Eamon.Game.Attributes;
using EamonRT.Framework.Commands;
using EamonRT.Framework.Components;
using EamonRT.Framework.States;
using static EamonRT.Game.Plugin.PluginContext;

namespace EamonRT.Game.Commands
{
	[ClassMappings]
	public class MonsterAttackCommand : Command, IMonsterAttackCommand
	{
		public virtual bool BlastSpell { get; set; }

		public virtual bool CheckAttack { get; set; }

		public virtual long MemberNumber { get; set; }

		public virtual long AttackNumber { get; set; }

		/// <summary></summary>
		public virtual ICombatComponent CombatComponent { get; set; }

		public override void Execute()
		{
			Debug.Assert(DobjMonster != null);

			CombatComponent = Globals.CreateInstance<ICombatComponent>(x =>
			{
				x.SetNextStateFunc = s => NextState = s;

				x.ActorMonster = ActorMonster;

				x.ActorRoom = ActorRoom;

				x.Dobj = DobjMonster;

				x.MemberNumber = MemberNumber;

				x.AttackNumber = AttackNumber;
			});

			CombatComponent.ExecuteAttack();

			if (NextState == null)
			{
				NextState = Globals.CreateInstance<IErrorState>(x =>
				{
					x.ErrorMessage = string.Format("{0}: NextState == null", Name);
				});
			}
		}

		public MonsterAttackCommand()
		{
			SortOrder = 840;

			IsPlayerEnabled = false;

			IsMonsterEnabled = true;

			Name = "MonsterAttackCommand";

			Verb = "attack";

			Type = CommandType.Interactive;

			MemberNumber = 1;

			AttackNumber = 1;
		}
	}
}
