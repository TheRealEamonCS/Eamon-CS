
// Artifact.cs

// Copyright (c) 2014+ by Michael Penner.  All rights reserved.

using System.Text;
using Eamon;
using Eamon.Framework;
using Eamon.Game.Attributes;
using Eamon.Game.Extensions;
using static Firestorm.Game.Plugin.Globals;

namespace Firestorm.Game
{
	[ClassMappings]
	public class Artifact : Eamon.Game.Artifact, IArtifact
	{
		public override RetCode BuildPrintedFullDesc(StringBuilder buf, bool showName, bool showVerboseName)
		{
			RetCode rc;

			rc = base.BuildPrintedFullDesc(buf, showName, showVerboseName);

			if (gEngine.IsSuccess(rc) && !showName)
			{
				if (GeneralWeapon != null)
				{
					buf.AppendPrint("{0} a {1}D{2} weapon.", EvalPlural(IsCharOwned ? "It is" : "This is", "They are"), GeneralWeapon.Field3, GeneralWeapon.Field4);
				}
			}

			return rc;
		}

		public override bool IsRequestable()
		{
			// Buzz carrying buzz-sword

			return Uid != 66 || !IsCarriedByMonsterUid(45) ? base.IsRequestable() : false;
		}

		public override bool ShouldAddToHeldWpnUids()
		{
			// Store super-weapons

			return GeneralWeapon == null || GeneralWeapon.Field3 * GeneralWeapon.Field4 <= 24 ? base.ShouldAddToHeldWpnUids() : true;
		}
	}
}
