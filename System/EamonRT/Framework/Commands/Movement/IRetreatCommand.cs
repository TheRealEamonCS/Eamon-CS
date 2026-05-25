
// IRetreatCommand.cs

// Copyright (c) 2014+ by Michael Penner.  All rights reserved.

namespace EamonRT.Framework.Commands
{
	/// <summary></summary>
	public interface IRetreatCommand : ICommand
	{
		/// <summary></summary>
		long Range { get; set; }

		/// <summary></summary>
		long Direction { get; set; }

		/// <summary></summary>
		bool PartialTurnMove { get; set; }
	}
}
