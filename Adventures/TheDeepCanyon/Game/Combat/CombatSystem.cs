
// CombatSystem.cs

// Copyright (c) 2014+ by Michael Penner.  All rights reserved.

using System;
using System.Diagnostics;
using Eamon.Game.Attributes;
using EamonRT.Framework.Combat;
using static TheDeepCanyon.Game.Plugin.PluginContext;

namespace TheDeepCanyon.Game.Combat
{
	[ClassMappings]
	public class CombatSystem : EamonRT.Game.Combat.CombatSystem, ICombatSystem
	{
		public override void PrintHealthStatus()
		{
			base.PrintHealthStatus();

			var room = DfMonster.GetInRoom();

			Debug.Assert(room != null);

			if (DfMonster.IsDead())
			{
				gOut.Print("{0}{1} dead, Jim.", Environment.NewLine, DfMonster.IsCharacterMonster() || room.IsLit() ? DfMonster.EvalGender("He's", "She's", "It's") : "It's");
			}
		}
	}
}
