﻿
// IMonster.cs

// Copyright (c) 2014+ by Michael Penner.  All rights reserved.

using static TheVileGrimoireOfJaldial.Game.Plugin.Globals;

namespace TheVileGrimoireOfJaldial.Framework
{
	/// <inheritdoc />
	public interface IMonster : Eamon.Framework.IMonster
	{
		/// <summary></summary>
		bool Seen02 { get; }

		/// <summary></summary>
		string AttackDesc { get; set; }

		/// <summary></summary>
		void SetAttackModality();
	}
}
