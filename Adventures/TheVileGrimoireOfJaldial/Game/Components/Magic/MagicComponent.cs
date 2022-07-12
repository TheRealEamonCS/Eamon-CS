
// MagicComponent.cs

// Copyright (c) 2014+ by Michael Penner.  All rights reserved.

using System.Linq;
using Eamon.Game.Attributes;
using EamonRT.Framework.Components;
using static TheVileGrimoireOfJaldial.Game.Plugin.PluginContext;

namespace TheVileGrimoireOfJaldial.Game.Components
{
	[ClassMappings]
	public class MagicComponent : EamonRT.Game.Components.MagicComponent, IMagicComponent
	{
		/// <summary></summary>
		public override void TunnelCollapses()
		{
			base.TunnelCollapses();

			// 10% chance of death trap - affects all room occupants

			if (PowerEventRoll < 11)
			{
				var monsterUids = new long[] { 38, 43, 44, 50 };

				var monsterList = gEngine.GetMonsterList(m => m.IsInRoom(ActorRoom) && !monsterUids.Contains(m.Uid) && !m.IsCharacterMonster());

				gOut.EnableOutput = false;

				foreach (var monster in monsterList)
				{
					monster.CurrGroupCount = 1;

					gEngine.MonsterDies(null, monster);
				}

				gOut.EnableOutput = true;
			}
		}
	}
}
