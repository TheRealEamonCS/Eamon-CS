﻿
// Artifact.cs

// Copyright (c) 2014+ by Kenneth Pedersen.  All rights reserved.

using System.Text;
using Eamon;
using Eamon.Framework;
using Eamon.Framework.Primitive.Enums;
using Eamon.Game.Attributes;
using Eamon.Game.Extensions;
using EamonRT.Framework.Commands;
using static LandOfTheMountainKing.Game.Plugin.Globals;

namespace LandOfTheMountainKing.Game
{
	[ClassMappings]
	public class Artifact : Eamon.Game.Artifact, IArtifact
	{
		public override RetCode BuildPrintedFullDesc(StringBuilder buf, bool showName, bool showVerboseName)
		{
			RetCode rc;

			// Examine Damian's body

			if (gEngine.CurrState is IExamineCommand && Uid == 26)
			{
				var Necklace = gADB[27];

				var effect = gEDB[gLMKKP1.NecklaceTaken > 0 ? 25 : 24];

				buf.AppendPrint("{0}", effect.Desc);

				if (gLMKKP1.NecklaceTaken == 0)
				{
					if (gCharMonster.CanCarryArtifactWeight(Necklace))
					{
						Necklace.SetCarriedByMonster(gCharMonster);
					}
					else
					{
						Necklace.SetInRoom(gCharRoom);
					}

					gLMKKP1.NecklaceTaken = 1;
				}

				rc = RetCode.Success;
			}
			else
			{
				rc = base.BuildPrintedFullDesc(buf, showName, showVerboseName);
			}

			return rc;
		}

		public override bool ShouldExposeContentsToRoom(ContainerType containerType = ContainerType.In)
		{
			return (true);
		}
	}
}
