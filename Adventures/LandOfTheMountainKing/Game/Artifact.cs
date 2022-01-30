
// Artifact.cs

// Copyright (c) 2014+ by Kenneth Pedersen.  All rights reserved.

using System.Diagnostics;
using System.Text;
using Eamon;
using Eamon.Framework;
using Eamon.Framework.Primitive.Enums;
using Eamon.Game.Attributes;
using Eamon.Game.Extensions;
using EamonRT.Framework.Commands;
using static LandOfTheMountainKing.Game.Plugin.PluginContext;

namespace LandOfTheMountainKing.Game
{
	[ClassMappings]
	public class Artifact : Eamon.Game.Artifact, IArtifact
	{
		public override RetCode BuildPrintedFullDesc(StringBuilder buf, bool showName)
		{
			RetCode rc;

			// Examine Damian's body

			if (Globals.CurrState is IExamineCommand && Uid == 26)
			{
				var Necklace = gADB[27];

				var effect = gEDB[gLMKKP1.NecklaceTaken > 0 ? 25 : 24];

				buf.AppendPrint("{0}", effect.Desc);

				if (gLMKKP1.NecklaceTaken == 0)
				{
					Necklace.SetCarriedByCharacter();			// TODO: put necklace in Room if its too heavy to be carried

					gLMKKP1.NecklaceTaken = 1;
				}

				rc = RetCode.Success;
			}
			else
			{
				rc = base.BuildPrintedFullDesc(buf, showName);
			}

			return rc;
		}

		public override bool ShouldExposeContentsToRoom(ContainerType containerType = ContainerType.In)
		{
			return (true);
		}
	}
}
