
// IHealCommand.cs

// Copyright (c) 2014+ by Michael Penner.  All rights reserved.

namespace EamonRT.Framework.Commands
{
	/// <summary></summary>
	public interface IHealCommand : ICommand
	{
		/// <summary></summary>
		bool CastSpell { get; set; }
	}
}
