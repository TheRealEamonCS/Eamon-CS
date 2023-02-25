
// InjureAndDamageArgs.cs

// Copyright (c) 2014+ by Michael Penner.  All rights reserved.

using System;
using Eamon.Framework;
using Eamon.Game.Attributes;
using EamonRT.Framework.Args;
using EamonRT.Framework.States;

namespace EamonRT.Game.Args
{
	[ClassMappings]
	public class InjureAndDamageArgs : IInjureAndDamageArgs
	{
		public virtual IRoom Room { get; set; }

		public virtual long EffectUid { get; set; }

		public virtual long DeadBodyRoomUid { get; set; }

		public virtual long EquipmentDamageAmount { get; set; }

		public virtual double InjuryMultiplier { get; set; }

		public virtual Action<IState> SetNextStateFunc { get; set; }

		public InjureAndDamageArgs()
		{

		}
	}
}
