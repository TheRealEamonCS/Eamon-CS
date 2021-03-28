
// IRestoreCommand.cs

// Copyright (c) 2014+ by Michael Penner.  All rights reserved.

namespace EamonRT.Framework.Commands
{
	/// <summary></summary>
	public interface IRestoreCommand : ICommand
	{
		/// <summary></summary>
		long SaveSlot { get; set; }
	}
}
