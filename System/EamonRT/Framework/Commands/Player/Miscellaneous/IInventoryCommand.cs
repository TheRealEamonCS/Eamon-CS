
// IInventoryCommand.cs

// Copyright (c) 2014+ by Michael Penner.  All rights reserved.

namespace EamonRT.Framework.Commands
{
	/// <summary></summary>
	public interface IInventoryCommand : ICommand
	{
		/// <summary></summary>
		bool AllowExtendedContainers { get; set; }
	}
}
