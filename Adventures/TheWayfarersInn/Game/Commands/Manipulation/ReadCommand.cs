
// ReadCommand.cs

// Copyright (c) 2014+ by Michael Penner.  All rights reserved.

using System;
using System.Diagnostics;
using System.Linq;
using System.Text;
using Eamon.Game.Attributes;
using EamonRT.Framework.Commands;
using EamonRT.Framework.Primitive.Enums;
using EamonRT.Framework.States;
using static TheWayfarersInn.Game.Plugin.Globals;

namespace TheWayfarersInn.Game.Commands
{
	[ClassMappings]
	public class ReadCommand : EamonRT.Game.Commands.ReadCommand, IReadCommand
	{
		public override void ProcessEvents(EventType eventType)
		{
			base.ProcessEvents(eventType);

			if (eventType == EventType.AfterReadArtifact)
			{
				// Forgecraft Codex

				if (DobjArtifact.Uid == 167)
				{
					gGameState.ForgecraftCodexRead = true;
				}
			}
		}

		public override void ExecuteForPlayer()
		{
			Debug.Assert(DobjArtifact != null);

			var doorArtifactArray = new[]
			{
				new { Uid = 44, ReadText = "The phrase \"Staff Only\"" },
				new { Uid = 45, ReadText = "The phrase \"Lavatory\"" },
				new { Uid = 61, ReadText = "The phrase \"Staff Only\"" },
				new { Uid = 86, ReadText = "The number \"1\"" },
				new { Uid = 88, ReadText = "The number \"3\"" },
				new { Uid = 89, ReadText = "The number \"2\"" },
				new { Uid = 90, ReadText = "The number \"5\"" },
				new { Uid = 91, ReadText = "The number \"4\"" },
				new { Uid = 95, ReadText = "The phrase \"Balcony\"" },
				new { Uid = 100, ReadText = "The number \"6\"" },
				new { Uid = 101, ReadText = "The number \"7\"" },
				new { Uid = 103, ReadText = "The number \"8\"" },
				new { Uid = 104, ReadText = "The number \"9\"" },
				new { Uid = 105, ReadText = "The number \"10\"" },
				new { Uid = 106, ReadText = "The number \"11\"" },
				new { Uid = 107, ReadText = "The number \"12\"" },
				new { Uid = 108, ReadText = "The number \"14\"" },
			};

			var doorArtifact = doorArtifactArray.FirstOrDefault(da => da.Uid == DobjArtifact.Uid);

			var treelineGapArtifact = gADB[16];

			Debug.Assert(treelineGapArtifact != null);

			// Bottle of kerosene

			if (DobjArtifact.Uid == 6)
			{
				gEngine.PrintEffectDesc(111);

				NextState = gEngine.CreateInstance<IMonsterStartState>();
			}

			// Gravesite markers

			else if (DobjArtifact.Uid == 25)
			{
				gOut.Print("You read the markers and ponder the lives lost over so many years.");

				NextState = gEngine.CreateInstance<IMonsterStartState>();
			}

			// Accounting ledger

			else if (DobjArtifact.Uid == 68 && DobjArtifact.Readable.IsOpen())
			{
				gOut.Print("Poring over the accounting ledger, you see some entries that shortly precede an abrupt change in the handwriting style (which you find peculiar){0}", gEngine.EnableScreenReaderMode ? "." : ":");

				if (!gEngine.EnableScreenReaderMode)
				{
					gEngine.In.KeyPress(gEngine.Buf);

					gOut.Print("{0}", gEngine.LineSep);

					gOut.Print("{0}", Encoding.UTF8.GetString(Convert.FromBase64String(gEngine.AccountingLedgerData)));

					gEngine.In.KeyPress(gEngine.Buf);

					gOut.Print("{0}", gEngine.LineSep);
				}

				gEngine.PrintEffectDesc(22);

				NextState = gEngine.CreateInstance<IMonsterStartState>();
			}

			// Diary

			else if (DobjArtifact.Uid == 85 && DobjArtifact.Readable.IsOpen())
			{
				for (var i = 25; i <= 31; i++)
				{
					gEngine.PrintEffectDesc(i);

					if (i < 31)
					{
						gEngine.In.KeyPress(gEngine.Buf);

						gOut.Print("{0}", gEngine.LineSep);
					}
				}

				gGameState.DiaryRead = true;

				NextState = gEngine.CreateInstance<IMonsterStartState>();
			}

			// Message

			else if (DobjArtifact.Uid == 176)
			{
				gOut.Print("{0}", Encoding.UTF8.GetString(Convert.FromBase64String(gEngine.KitchenMessageData)));

				if (gGameState.KitchenRiddleState == 0)
				{
					gGameState.KitchenRiddleState++;
				}

				NextState = gEngine.CreateInstance<IMonsterStartState>();
			}

			// Various doors

			else if (doorArtifact != null)
			{
				gOut.Print("{0} is etched into the wood at eye level.", doorArtifact.ReadText);

				if (doorArtifact.Uid == 86)
				{
					gOut.Print("The glyph resembles one of warding, though no variant you're familiar with.");
				}

				NextState = gEngine.CreateInstance<IMonsterStartState>();
			}
			else
			{
				base.ExecuteForPlayer();
			}

			// Wall map

			if (DobjArtifact.Uid == 92)
			{
				if (!gGameState.WallMapRead)
				{
					treelineGapArtifact.SetEmbeddedInRoomUid(11);

					gGameState.WallMapRead = true;
				}
			}
		}
	}
}
