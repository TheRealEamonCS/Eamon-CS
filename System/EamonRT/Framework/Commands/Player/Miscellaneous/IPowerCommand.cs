
// IPowerCommand.cs

// Copyright (c) 2014+ by Michael Penner.  All rights reserved.

using System;
using Eamon.Framework;

namespace EamonRT.Framework.Commands
{
	/// <summary></summary>
	public interface IPowerCommand : ICommand
	{
		/// <summary></summary>
		bool CastSpell { get; set; }

		/// <summary></summary>
		Func<IArtifact, bool>[] ResurrectWhereClauseFuncs { get; set; }

		/// <summary></summary>
		Func<IArtifact, bool>[] VanishWhereClauseFuncs { get; set; }
	}
}
