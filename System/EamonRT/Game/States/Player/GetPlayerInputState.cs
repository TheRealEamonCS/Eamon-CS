
// GetPlayerInputState.cs

// Copyright (c) 2014+ by Michael Penner.  All rights reserved.

using Eamon.Game.Attributes;
using Eamon.Game.Extensions;
using EamonRT.Framework.Primitive.Enums;
using EamonRT.Framework.States;
using EamonRT.Game.Exceptions;
using static EamonRT.Game.Plugin.Globals;

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

				gEngine.PauseCombatActionsCounter = 0;

				gEngine.ShouldPreTurnProcess = true;

				// If we've run out of player input get more player input

				if (gSentenceParser.IsInputExhausted)
				{
					PrintCommandPrompt();

					gEngine.CommandPromptSeen = true;

					gEngine.PlayerMoved = false;

					gEngine.CursorPosition = gOut.GetCursorPosition();

					if (gEngine.CursorPosition.Y > -1 && gEngine.CursorPosition.Y + 1 >= gOut.GetBufferHeight())
					{
						gEngine.CursorPosition.Y--;
					}

					gOut.WriteLine();

					gOut.SetCursorPosition(gEngine.CursorPosition);

					gSentenceParser.Clear();

					gSentenceParser.InputBuf.SetFormat("{0}", gEngine.In.ReadLine());

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

				NextState = gEngine.CreateInstance<IStartState>();
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
				NextState = gEngine.CreateInstance<IProcessPlayerInputState>();
			}

			gEngine.NextState = NextState;
		}

		public GetPlayerInputState()
		{
			Name = "GetPlayerInputState";

			ParsingSuccessful = true;
		}
	}
}
