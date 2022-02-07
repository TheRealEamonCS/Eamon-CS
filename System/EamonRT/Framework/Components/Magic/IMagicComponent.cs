﻿
// IMagicComponent.cs

// Copyright (c) 2014+ by Michael Penner.  All rights reserved.

using Eamon.Framework.Primitive.Classes;
using Eamon.Framework.Primitive.Enums;

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
