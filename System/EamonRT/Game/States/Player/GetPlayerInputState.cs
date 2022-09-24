
// GetPlayerInputState.cs

// Copyright (c) 2014+ by Michael Penner.  All rights reserved.

using Eamon.Game.Attributes;
using Eamon.Game.Extensions;
using EamonRT.Framework.Primitive.Enums;
using EamonRT.Framework.States;
using EamonRT.Game.Exceptions;
using static EamonRT.Game.Plugin.PluginContext;

namespace EamonRT.Game.States
{
	[ClassMappings]
	public class GetPlayerInputState : State, IGetPlayerInputState
	{
		public virtual bool RestartCommand { get; set; }

		/// <summary></summary>
		public virtual bool ParsingSuccessful { get; set; }

		public override void Execute()
		{
			if (!RestartCommand)
			{
				ProcessEvents(EventType.BeforePrintCommandPrompt);

				if (GotoCleanup)
				{
					goto Cleanup;
				}

				// If we've run out of player input get more player input

				if (gSentenceParser.IsInputExhausted)
				{
					PrintCommandPrompt();

					Globals.CommandPromptSeen = true;

					Globals.PlayerMoved = false;

					Globals.CursorPosition = gOut.GetCursorPosition();

					if (Globals.CursorPosition.Y > -1 && Globals.CursorPosition.Y + 1 >= gOut.GetBufferHeight())
					{
						Globals.CursorPosition.Y--;
					}

					gOut.WriteLine();

					gOut.SetCursorPosition(Globals.CursorPosition);

					gSentenceParser.Clear();

					gSentenceParser.InputBuf.SetFormat("{0}", Globals.In.ReadLine());

					gSentenceParser.Execute();
				}
			}

			gCommandParser.Clear();

			gCommandParser.ActorMonster = gCharMonster;

			try
			{
				gSentenceParser.ReplacePronounsAndProcessDobjNameList();
			}
			catch (GeneralParsingErrorException)
			{
				ParsingSuccessful = false;

				NextState = Globals.CreateInstance<IStartState>();
			}

			if (gSentenceParser.IsInputExhausted)
			{
				goto Cleanup;
			}

			if (gGameState.ShowFulfillMessages)
			{
				PrintFulfillMessage(gSentenceParser.ParserInputStr);
			}

			if (ParsingSuccessful)
			{
				gCommandParser.InputBuf.SetFormat("{0}", gSentenceParser.ParserInputStr);
			}
			else
			{
				PrintDontFollowYou02();
			}

			gSentenceParser.ParserInputStrList.RemoveAt(0);

		Cleanup:

			if (NextState == null)
			{
				NextState = Globals.CreateInstance<IProcessPlayerInputState>();
			}

			Globals.NextState = NextState;
		}

		public GetPlayerInputState()
		{
			Name = "GetPlayerInputState";

			ParsingSuccessful = true;
		}
	}
}
