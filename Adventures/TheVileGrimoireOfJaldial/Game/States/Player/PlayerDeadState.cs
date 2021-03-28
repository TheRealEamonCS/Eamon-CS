
// PlayerDeadState.cs

// Copyright (c) 2014+ by Michael Penner.  All rights reserved.

using System;
using System.Diagnostics;
using Eamon.Game.Attributes;
using EamonRT.Framework.States;
using static TheVileGrimoireOfJaldial.Game.Plugin.PluginContext;

namespace TheVileGrimoireOfJaldial.Game.States
{
	[ClassMappings]
	public class PlayerDeadState : EamonRT.Game.States.PlayerDeadState, IPlayerDeadState
	{
		public override void Execute()
		{
			if (gGameState.PlayerResurrections <= 2)
			{
				gOut.Print("{0}", Globals.LineSep);

				gOut.Write("{0}Press any key to continue: ", Environment.NewLine);

				Globals.Buf.Clear();

				var rc = Globals.In.ReadField(Globals.Buf, Constants.BufSize02, null, ' ', '\0', true, null, gEngine.ModifyCharToNull, null, gEngine.IsCharAny);

				Debug.Assert(gEngine.IsSuccess(rc));

				Globals.Thread.Sleep(150);

				gOut.Print("{0}", Globals.LineSep);

				if (++gGameState.PlayerResurrections <= 2)
				{
					NextState = Globals.CreateInstance<Framework.States.IPlayerResurrectedState>();

					Globals.NextState = NextState;
				}
				else
				{
					gEngine.PrintEffectDesc(97);

					gEngine.PrintEffectDesc(105);

					gEngine.PrintEffectDesc(106);

					gEngine.PrintEffectDesc(107);
				}
			}

			if (gGameState.PlayerResurrections > 2)
			{
				base.Execute();
			}
		}
	}
}
