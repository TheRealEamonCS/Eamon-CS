
// Room.cs

// Copyright (c) 2014+ by Michael Penner.  All rights reserved.

using System;
using Eamon.Framework;
using Eamon.Game.Attributes;

namespace ARuncibleCargo.Game
{
	[ClassMappings(typeof(IRoom))]
	public class Room : Eamon.Game.Room, Framework.IRoom
	{
		public override string GetObviousExits()
		{
			return IsWaterRoom() ? string.Format("{0}Obvious directions:  ", Environment.NewLine) : base.GetObviousExits();
		}

		public virtual bool IsWaterRoom()
		{
			return Uid >= 97 && Uid <= 101;
		}
	}
}
