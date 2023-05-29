
// IDropCommand.cs

// Copyright (c) 2014+ by Michael Penner.  All rights reserved.

namespace EamonRT.Framework.Commands
{
	/// <summary></summary>
	public interface IDropCommand : ICommand
	{
		/// <summary></summary>
		bool DropAll { get; set; }
	}
}
