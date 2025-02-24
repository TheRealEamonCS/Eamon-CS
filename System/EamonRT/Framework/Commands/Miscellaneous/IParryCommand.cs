
// IParryCommand.cs

// Copyright (c) 2014+ by Michael Penner.  All rights reserved.

namespace EamonRT.Framework.Commands
{
	/// <summary></summary>
	public interface IParryCommand : ICommand
	{
		/// <summary></summary>
		long Parry { get; set; }

		/// <summary></summary>
		bool PrintCombatStanceChanged { get; set; }
	}
}
