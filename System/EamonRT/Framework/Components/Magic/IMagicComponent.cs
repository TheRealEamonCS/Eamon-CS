
// IMagicComponent.cs

// Copyright (c) 2014+ by Michael Penner.  All rights reserved.

using System;
using Eamon.Framework;

namespace EamonRT.Framework.Components
{
	/// <summary></summary>
	public interface IMagicComponent : IComponent
	{
		/// <summary></summary>
		Func<IArtifact, bool>[] ResurrectWhereClauseFuncs { get; set; }

		/// <summary></summary>
		Func<IArtifact, bool>[] VanishWhereClauseFuncs { get; set; }

		/// <summary></summary>
		bool CastSpell { get; set; }

		/// <summary></summary>
		void ExecuteBlastSpell();

		/// <summary></summary>
		void ExecuteHealSpell();

		/// <summary></summary>
		void ExecuteSpeedSpell();

		/// <summary></summary>
		void ExecutePowerSpell();
	}
}
