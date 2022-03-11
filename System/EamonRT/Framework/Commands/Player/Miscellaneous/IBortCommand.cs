
// IBortCommand.cs

// Copyright (c) 2014+ by Michael Penner.  All rights reserved.

using System.Collections.Generic;
using Eamon.Framework;

namespace EamonRT.Framework.Commands
{
	/// <summary></summary>
	public interface IBortCommand : ICommand
	{
		/// <summary></summary>
		IGameBase Record { get; set; }

		/// <summary></summary>
		string Action { get; set; }
	}
}
