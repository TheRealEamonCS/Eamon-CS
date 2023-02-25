
// IInjureAndDamageArgs.cs

// Copyright (c) 2014+ by Michael Penner.  All rights reserved.

using System;
using Eamon.Framework;
using EamonRT.Framework.States;

namespace EamonRT.Framework.Args
{
	/// <summary></summary>
	public interface IInjureAndDamageArgs
	{
		/// <summary></summary>
		IRoom Room { get; set; }

		/// <summary></summary>
		long EffectUid { get; set; }

		/// <summary></summary>
		long DeadBodyRoomUid { get; set; }

		/// <summary></summary>
		long EquipmentDamageAmount { get; set; }

		/// <summary></summary>
		double InjuryMultiplier { get; set; }

		/// <summary></summary>
		Action<IState> SetNextStateFunc { get; set; }
	}
}
