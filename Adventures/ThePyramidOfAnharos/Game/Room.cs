
// Room.cs

// Copyright (c) 2014+ by Michael Penner.  All rights reserved.

using System;
using System.Diagnostics;
using System.Text;
using Eamon;
using Eamon.Framework;
using Eamon.Game.Attributes;
using Eamon.Game.Extensions;
using static ThePyramidOfAnharos.Game.Plugin.Globals;

namespace ThePyramidOfAnharos.Game
{
	[ClassMappings]
	public class Room : Eamon.Game.Room, IRoom
	{
		public override long GetDir(long index)
		{
			if (gEngine.EnableMutateProperties)
			{
				if (Uid == 6)
				{
					return gGameState.KE != 0 && index == 3 ? 14 : base.GetDir(index);
				}
				else if (Uid == 14)
				{
					return gGameState.KE != 0 && index == 4 ? 6 : base.GetDir(index);
				}
				else if (Uid == 12)
				{
					return gGameState.KF != 0 && index == 1 ? 16 : base.GetDir(index);
				}
				else if (Uid == 16)
				{
					return gGameState.KF != 0 && index == 2 ? 12 : base.GetDir(index);
				}
				else if (Uid == 22)
				{
					return gGameState.KG != 0 && index == 5 ? 25 : base.GetDir(index);
				}
				else if (Uid == 25)
				{
					return gGameState.KG != 0 && index == 6 ? 22 : base.GetDir(index);
				}
				else if (Uid == 30)
				{
					return gGameState.KU == 2 && index == 5 ? 31 : base.GetDir(index);
				}
				else
				{
					return base.GetDir(index);
				}
			}
			else
			{
				return base.GetDir(index);
			}
		}

		public override bool IsDirectionInObviousExitsList(long index)
		{
			// Suppress up/down on stone ramp

			return base.IsDirectionInObviousExitsList(index) && (Uid == 16 ? index != 6 : Uid == 19 || Uid == 20 ? index != 5 && index != 6 : Uid == 21 ? index != 5 : true);
		}

		public override string GetObviousExits()
		{
			// No obvious exits in desert

			return Zone != 1 ? base.GetObviousExits() : "";
		}

		public override RetCode GetExitList(StringBuilder buf, Func<string, string> modFunc = null, bool useNames = true)
		{
			RetCode rc;
			
			rc = RetCode.Success;

			// No obvious exits in desert

			if (Zone != 1)
			{
				// Between Flames & Cloud

				if (Uid == 28)
				{
					buf.AppendFormat("none.");
				}

				// By Dark Cloud

				else if (Uid == 29)
				{
					buf.AppendFormat("south.");
				}
				else
				{
					rc = base.GetExitList(buf, modFunc, useNames);
				}
			}

			return rc;
		}

		public override RetCode BuildPrintedTooDarkToSeeDesc(StringBuilder buf)
		{
			RetCode result;

			Debug.Assert(buf != null);

			// Print room desc in Black room

			if (Uid == 39 && (!Seen || gGameState.Vr))
			{
				buf.SetPrint("[{0}]{1}{2}{1}{3}", Name, Environment.NewLine, Desc, "Obvious exits: north.");

				Seen = true;

				result = RetCode.Success;
			}
			else
			{
				result = base.BuildPrintedTooDarkToSeeDesc(buf);
			}

			return result;
		}
	}
}
