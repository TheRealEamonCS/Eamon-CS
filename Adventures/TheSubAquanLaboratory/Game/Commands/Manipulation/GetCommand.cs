
// GetCommand.cs

// Copyright (c) 2014+ by Michael Penner.  All rights reserved.

using System.Diagnostics;
using Eamon.Framework;
using Eamon.Game.Attributes;
using Eamon.Game.Extensions;
using EamonRT.Framework.Commands;
using static TheSubAquanLaboratory.Game.Plugin.Globals;

namespace TheSubAquanLaboratory.Game.Commands
{
	[ClassMappings]
	public class GetCommand : EamonRT.Game.Commands.GetCommand, IGetCommand
	{
		public override void PrintCantVerbThat(IArtifact artifact)
		{
			Debug.Assert(artifact != null);

			var ac = artifact.GetCategory(0);

			Debug.Assert(ac != null);

			gEngine.Buf.Clear();

			switch (ac.Field4)
			{
				case -1:

					gEngine.Buf.SetPrint("{0} {1} affixed to the wall.", artifact.GetTheName(true), artifact.EvalPlural("is", "are"));

					break;

				case -2:

					gEngine.Buf.SetPrint("{0} {1} carved into the wall.", artifact.GetTheName(true), artifact.EvalPlural("is", "are"));

					break;

				case -3:

					gEngine.Buf.SetPrint("{0} {1} bolted down, and can't be removed.", artifact.GetTheName(true), artifact.EvalPlural("is", "are"));

					break;

				case -4:

					gEngine.Buf.SetPrint("You can't get near enough to {0} to grab {1}.", artifact.GetTheName(), artifact.EvalPlural("it", "them"));

					break;
			}

			if (gEngine.Buf.Length > 0)
			{
				gOut.Write("{0}", gEngine.Buf);
			}
			else
			{
				base.PrintCantVerbThat(artifact);
			}
		}
	}
}
