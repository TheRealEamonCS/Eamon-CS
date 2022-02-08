
// IMagicComponent.cs

// Copyright (c) 2014+ by Michael Penner.  All rights reserved.

namespace EamonRT.Framework.Components
{
	/// <summary></summary>
	public interface IMagicComponent : IComponent
	{
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
