
// Room.cs

// Copyright (c) 2014+ by Michael Penner.  All rights reserved.

using System;
using System.Linq;
using Eamon.Framework;
using Eamon.Game.Attributes;
using static RiddlesOfTheDuergarKingdom.Game.Plugin.PluginContext;

namespace RiddlesOfTheDuergarKingdom.Game
{
	[ClassMappings(typeof(IRoom))]
	public class Room : Eamon.Game.Room, Framework.IRoom
	{
		public override string Desc
		{
			get
			{
				var result = base.Desc;

				if (Globals.EnableGameOverrides && gGameState != null)
				{
					if (Uid == 136)		// Wooden cart
					{
						result = gGameState.WinchCounter == 0 ? "R136 (Quarry)" : gGameState.WinchCounter == 1 ? "R136 (Mine Shaft)" : base.Desc;		// TODO: refactor
					}
				}

				return result;
			}

			set
			{
				base.Desc = value;
			}
		}

		public virtual bool GradStudentCompanionSeen { get; set; }

		public override long GetDirs(long index)
		{
			var result = base.GetDirs(index);

			if (Globals.EnableGameOverrides && gGameState != null)
			{
				// Wooden cart

				if (Uid == 136 && index == 12)
				{
					return gGameState.WinchCounter == 0 ? 84 : gGameState.WinchCounter == 1 ? 0 : base.GetDirs(index);
				}
			}

			return result;
		}

		public override bool IsDirectionInObviousExitsList(long index)
		{
			var roomUids = new long[]
			{
				44, 148, 150, 151, 152, 153
			};

			// Suppress up/down for everything in roomUids

			return base.IsDirectionInObviousExitsList(index) && (roomUids.Contains(Uid) ? index != 5 && index != 6 : true);
		}

		public override string GetObviousExits()
		{
			return Uid == 40 ? string.Format("{0}Obvious directions:  ", Environment.NewLine) : base.GetObviousExits();
		}
	}
}
