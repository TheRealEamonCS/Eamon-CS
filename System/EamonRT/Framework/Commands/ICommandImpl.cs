
// ICommandImpl.cs

// Copyright (c) 2014+ by Michael Penner.  All rights reserved.

namespace EamonRT.Framework.Commands
{
	/// <summary></summary>
	public interface ICommandImpl : ICommandSignatures
	{
		/// <summary></summary>
		ICommand Command { get; set; }

		/// <summary></summary>
		void Execute();
	}
}
