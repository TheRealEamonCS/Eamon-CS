
// IAttackCommand.cs

// Copyright (c) 2014+ by Michael Penner.  All rights reserved.

namespace EamonRT.Framework.Commands
{
	/// <summary></summary>
	public interface IAttackCommand : ICommand
	{
		/// <summary></summary>
		bool BlastSpell { get; set; }

		/// <summary></summary>
		bool CheckAttack { get; set; }

		/// <summary></summary>
		long MemberNumber { get; set; }

		/// <summary></summary>
		long AttackNumber { get; set; }
	}
}
