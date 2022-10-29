
// QuitCommand.cs

// Copyright (c) 2014+ by Michael Penner.  All rights reserved.

using System.Diagnostics;
using Eamon;
using Eamon.Framework.Primitive.Enums;
using Eamon.Game.Attributes;
using EamonRT.Framework.Commands;
using EamonRT.Framework.States;
using static EamonRT.Game.Plugin.Globals;

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

				gEngine.Buf.Clear();

				rc = gEngine.In.ReadField(gEngine.Buf, gEngine.BufSize02, null, ' ', '\0', false, null, gEngine.ModifyCharToUpper, gEngine.IsCharYOrN, null);

				Debug.Assert(gEngine.IsSuccess(rc));

				if (gEngine.Buf.Length > 0 && gEngine.Buf[0] == 'Y')
				{
					gGameState.Die = -1;

					gEngine.ExitType = ExitType.GoToMainHall;

					gEngine.MainLoop.ShouldShutdown = false;

					goto Cleanup;
				}

				goto Cleanup;
			}

			if (gEngine.Database.GetFilesetCount() == 0)
			{
				PrintHaventSavedGameYet(ActorMonster);
			}

			PrintReallyWantToQuit();

			gEngine.Buf.Clear();

			rc = gEngine.In.ReadField(gEngine.Buf, gEngine.BufSize02, null, ' ', '\0', false, null, gEngine.ModifyCharToUpper, gEngine.IsCharYOrN, null);

			Debug.Assert(gEngine.IsSuccess(rc));

			if (gEngine.Buf.Length > 0 && gEngine.Buf[0] == 'Y')
			{
				gEngine.ExitType = ExitType.Quit;

				gEngine.MainLoop.ShouldShutdown = false;

				goto Cleanup;
			}

		Cleanup:

			if (NextState == null)
			{
				NextState = gEngine.CreateInstance<IStartState>();
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
