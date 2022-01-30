
// ILMKKP1.cs

// Copyright (c) 2014+ by Kenneth Pedersen.  All rights reserved.

using static LandOfTheMountainKing.Game.Plugin.PluginContext;

namespace LandOfTheMountainKing.Framework
{
	public interface ILMKKP1
	{
		int Lampdir { get; set; }
		int NecklaceTaken { get; set; }
		int SaidHello { get; set; }
		int SwampMonsterKilled { get; set; }

		long Hard { get; set; }
		long Agil { get; set; }
		long Axe { get; set; }
		long Bow { get; set; }
		long Club { get; set; }
		long Spear { get; set; }
		long Sword { get; set; }
		long Armor { get; set; }
		long blast { get; set; }
		long heal { get; set; }
		long speed { get; set; }
		long power { get; set; }
	}
}
