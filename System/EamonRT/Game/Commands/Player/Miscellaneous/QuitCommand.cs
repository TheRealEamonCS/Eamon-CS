
// QuitCommand.cs

// Copyright (c) 2014+ by Michael Penner.  All rights reserved.

using System.Diagnostics;
using Eamon;
using Eamon.Framework.Primitive.Enums;
using Eamon.Game.Attributes;
using EamonRT.Framework.Commands;
using EamonRT.Framework.States;
using static EamonRT.Game.Plugin.PluginContext;

namespace EamonRT.Game.Commands
{
	[ClassMappings]
	public class QuitCommand : Command, IQuitCommand
	{
		public virtual bool GoToMainHall { get; set; }

		public override void Execute()
		{
			RetCode rc;

			if (GoToMainHall)
			{
				PrintReturnToMainHall();

				Globals.Buf.Clear();

				rc = Globals.In.ReadField(Globals.Buf, Constants.BufSize02, null, ' ', '\0', false, null, gEngine.ModifyCharToUpper, gEngine.IsCharYOrN, null);

				Debug.Assert(gEngine.IsSuccess(rc));

				if (Globals.Buf.Length > 0 && Globals.Buf[0] == 'Y')
				{
					gGameState.Die = -1;

					Globals.ExitType = ExitType.GoToMainHall;

					Globals.MainLoop.ShouldShutdown = false;

					goto Cleanup;
				}

				goto Cleanup;
			}

			if (Globals.Database.GetFilesetCount() == 0)
			{
				PrintHaventSavedGameYet(ActorMonster);
			}

			PrintReallyWantToQuit();

			Globals.Buf.Clear();

			rc = Globals.In.ReadField(Globals.Buf, Constants.BufSize02, null, ' ', '\0', false, null, gEngine.ModifyCharToUpper, gEngine.IsCharYOrN, null);

			Debug.Assert(gEngine.IsSuccess(rc));

			if (Globals.Buf.Length > 0 && Globals.Buf[0] == 'Y')
			{
				Globals.ExitType = ExitType.Quit;

				Globals.MainLoop.ShouldShutdown = false;

				goto Cleanup;
			}

		Cleanup:

			if (NextState == null)
			{
				NextState = Globals.CreateInstance<IStartState>();
			}
		}

		public QuitCommand()
		{
			SortOrder = 430;

			IsSentenceParserEnabled = false;

			IsDarkEnabled = true;

			Name = "QuitCommand";

			Verb = "quit";

			Type = CommandType.Miscellaneous;
		}
	}
}
