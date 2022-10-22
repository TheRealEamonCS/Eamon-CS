
// StateImpl.cs

// Copyright (c) 2014+ by Michael Penner.  All rights reserved.

using System.Diagnostics;
using Eamon.Framework;
using Eamon.Game.Attributes;
using EamonRT.Framework.States;
using static RiddlesOfTheDuergarKingdom.Game.Plugin.Globals;

namespace RiddlesOfTheDuergarKingdom.Game.States
{
	[ClassMappings]
	public class StateImpl : EamonRT.Game.States.StateImpl, IStateImpl
	{
		public override void PrintObjBlocksTheWay(IArtifact artifact)
		{
			Debug.Assert(artifact != null);

			// Stone cabin / stone outhouse / stone hydrostation

			if (artifact.Uid == 22 || artifact.Uid == 23 || artifact.Uid == 24)
			{
				gOut.Print("{0} door{1} block{2} the way!", artifact.GetTheName(true), artifact.EvalPlural("", "s"), artifact.EvalPlural("s", ""));
			}
			else
			{
				base.PrintObjBlocksTheWay(artifact);
			}
		}
	}
}
