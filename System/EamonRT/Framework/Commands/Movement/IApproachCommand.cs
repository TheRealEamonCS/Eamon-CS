
// IApproachCommand.cs

// Copyright (c) 2014+ by Michael Penner.  All rights reserved.

namespace EamonRT.Framework.Commands
{
	/// <summary></summary>
	public interface IApproachCommand : ICommand
	{
		/// <summary></summary>
		long Range { get; set; }

		/// <summary></summary>
		bool PartialTurnMove { get; set; }
	}
}
