
// TurnCommand.cs

// Copyright (c) 2014+ by Michael Penner.  All rights reserved.

using System;
using System.Diagnostics;
using Eamon;
using Eamon.Framework.Primitive.Enums;
using Eamon.Game.Attributes;
using EamonRT.Framework.States;
using static TheSubAquanLaboratory.Game.Plugin.Globals;

namespace TheSubAquanLaboratory.Game.Commands
{
	[ClassMappings]
	public class TurnCommand : EamonRT.Game.Commands.Command, Framework.Commands.ITurnCommand
	{
		public override void ExecuteForPlayer()
		{
			RetCode rc;

			Debug.Assert(DobjArtifact != null);

			switch (DobjArtifact.Uid)
			{
				case 65:

					// Alphabet dial

					gOut.Write("{0}Turn it toward which end of the alphabet (Up or Down) (U/D): ", Environment.NewLine);

					gEngine.Buf.Clear();

					rc = gEngine.In.ReadField(gEngine.Buf, gEngine.BufSize02, null, ' ', '\0', false, null, gEngine.ModifyCharToUpper, IsCharUOrD, null);

					Debug.Assert(gEngine.IsSuccess(rc));

					if (gEngine.Buf.Length > 0 && gEngine.Buf[0] == 'U')
					{
						if (!gGameState.AlphabetDial)
						{
							gEngine.PrintEffectDesc(45);

							gGameState.AlphabetDial = true;
						}
						else
						{
							gOut.Print("The dial is already turned up to its maximum.");
						}
					}
					else
					{
						if (gGameState.AlphabetDial)
						{
							gEngine.PrintEffectDesc(46);

							gGameState.AlphabetDial = false;
						}
						else
						{
							gOut.Print("The dial is at its absolute lowest.");
						}
					}

					goto Cleanup;

				default:

					PrintCantVerbObj(DobjArtifact);

					NextState = gEngine.CreateInstance<IStartState>();

					goto Cleanup;
			}

		Cleanup:

			if (NextState == null)
			{
				NextState = gEngine.CreateInstance<IMonsterStartState>();
			}
		}

		public virtual bool IsCharUOrD(char ch)
		{
			ch = Char.ToUpper(ch);

			return ch == 'U' || ch == 'D';
		}

		public TurnCommand()
		{
			SortOrder = 450;

			IsNew = true;

			Name = "TurnCommand";

			Verb = "turn";

			Type = CommandType.Manipulation;
		}
	}
}
