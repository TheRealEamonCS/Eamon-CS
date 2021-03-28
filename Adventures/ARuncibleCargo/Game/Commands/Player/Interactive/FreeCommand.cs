
// FreeCommand.cs

// Copyright (c) 2014+ by Michael Penner.  All rights reserved.

using System.Diagnostics;
using Eamon.Game.Attributes;
using EamonRT.Framework.Commands;
using static ARuncibleCargo.Game.Plugin.PluginContext;

namespace ARuncibleCargo.Game.Commands
{
	[ClassMappings]
	public class FreeCommand : EamonRT.Game.Commands.FreeCommand, IFreeCommand
	{
		public override void PrintMonsterFreed()
		{
			// Swarmy

			if (BoundMonster.Uid == 31)
			{
				Globals.Buf.Clear();

				var rc = BoundMonster.BuildPrintedFullDesc(Globals.Buf, false);

				Debug.Assert(gEngine.IsSuccess(rc));

				gOut.Write("{0}", Globals.Buf);

				BoundMonster.Seen = true;

				gEngine.PrintEffectDesc(64);
			}
			else
			{
				base.PrintMonsterFreed();
			}
		}
	}
}
