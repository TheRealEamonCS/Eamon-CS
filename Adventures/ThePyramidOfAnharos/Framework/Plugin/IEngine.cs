
// IEngine.cs

// Copyright (c) 2014+ by Michael Penner.  All rights reserved.

using System;
using Eamon.Framework;
using EamonRT.Framework.States;

namespace ThePyramidOfAnharos.Framework.Plugin
{
	public interface IEngine : EamonRT.Framework.Plugin.IEngine
	{
		string MapData { get; set; }

		bool TaxLevied { get; set; }

		void PrintGuideMonsterDirection();

		void PrintTheGlyphsRead(long effectUid);

		void InjurePartyAndDamageEquipment(IRoom room, long effectUid, long deadBodyRoomUid, long equipmentDamageAmount, double injuryMultiplier, Action<IState> setNextStateFunc, ref bool gotoCleanup);
	}
}
