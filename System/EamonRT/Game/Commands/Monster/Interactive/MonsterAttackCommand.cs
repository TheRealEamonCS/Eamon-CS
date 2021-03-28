
// MonsterAttackCommand.cs

// Copyright (c) 2014+ by Michael Penner.  All rights reserved.

using System.Diagnostics;
using Eamon.Framework.Primitive.Enums;
using Eamon.Game.Attributes;
using EamonRT.Framework.Combat;
using EamonRT.Framework.Commands;
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
		public virtual ICombatSystem CombatSystem { get; set; }

		public override void Execute()
		{
			Debug.Assert(DobjMonster != null);

			CombatSystem = Globals.CreateInstance<ICombatSystem>(x =>
			{
				x.SetNextStateFunc = s => NextState = s;

				x.OfMonster = ActorMonster;

				x.DfMonster = DobjMonster;

				x.MemberNumber = MemberNumber;

				x.AttackNumber = AttackNumber;
			});

			CombatSystem.ExecuteAttack();

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

			Uid = 18;

			Name = "MonsterAttackCommand";

			Verb = "attack";

			Type = CommandType.Interactive;

			MemberNumber = 1;

			AttackNumber = 1;
		}
	}
}
