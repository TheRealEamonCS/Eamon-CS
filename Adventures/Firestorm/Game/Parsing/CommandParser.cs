
// CommandParser.cs

// Copyright (c) 2014+ by Michael Penner.  All rights reserved.

using System;
using System.Diagnostics;
using System.Linq;
using System.Text.RegularExpressions;
using Eamon.Game.Attributes;
using Eamon.Game.Extensions;
using EamonRT.Framework.Parsing;
using Firestorm.Framework.Commands;
using static Firestorm.Game.Plugin.Globals;

namespace Firestorm.Game.Parsing
{
	[ClassMappings]
	public class CommandParser : EamonRT.Game.Parsing.CommandParser, ICommandParser
	{
		public override void ParseName()
		{
			base.ParseName();

			var woodenBoxArtifact = gADB[1];

			Debug.Assert(woodenBoxArtifact != null);

			var hiddenDoorArtifact = gADB[13];

			Debug.Assert(hiddenDoorArtifact != null);

			var magicCrossbowArtifact = gADB[39];

			Debug.Assert(magicCrossbowArtifact != null);

			var pebblesArtifact = gADB[40];

			Debug.Assert(pebblesArtifact != null);

			var healingHerbsArtifact = gADB[41];

			Debug.Assert(healingHerbsArtifact != null);

			var helmetArtifact = gADB[45];

			Debug.Assert(helmetArtifact != null);

			var noteArtifact = gADB[47];

			Debug.Assert(noteArtifact != null);

			var libraryBookArtifact = gADB[54];

			Debug.Assert(libraryBookArtifact != null);

			var mapArtifact = gADB[57];

			Debug.Assert(mapArtifact != null);

			var broadTippedArrowsArtifact = gADB[61];

			Debug.Assert(broadTippedArrowsArtifact != null);

			var healingDraughtsArtifact = gADB[65];

			Debug.Assert(healingDraughtsArtifact != null);

			var buzzSwordArtifact = gADB[66];

			Debug.Assert(buzzSwordArtifact != null);

			if (ActorRoom.Uid == 35 && !noteArtifact.Seen && ObjData.Name.ContainsAny(new string[] { "stools", "barstools", "posts" }, StringComparison.OrdinalIgnoreCase))
			{
				ObjData.Name = noteArtifact.Name;
			}
			else if (ActorRoom.Uid == 78 && !helmetArtifact.Seen && ObjData.Name.ContainsAny(new string[] { "pegs", "gear" }, StringComparison.OrdinalIgnoreCase))
			{
				ObjData.Name = helmetArtifact.Name;
			}
			else if (ActorRoom.Uid == 17 && !magicCrossbowArtifact.Seen && ObjData.Name.Contains("weapons", StringComparison.OrdinalIgnoreCase))
			{
				ObjData.Name = magicCrossbowArtifact.Name;
			}
			else if (ActorRoom.Uid == 19 && !pebblesArtifact.Seen && ObjData.Name.ContainsAny(new string[] { "rocks", "rock" }, StringComparison.OrdinalIgnoreCase))
			{
				ObjData.Name = pebblesArtifact.Name;
			}
			else if (ActorRoom.Uid == 2 && !woodenBoxArtifact.Seen && ObjData.Name.Contains("branch", StringComparison.OrdinalIgnoreCase))
			{
				ObjData.Name = woodenBoxArtifact.Name;
			}
			else if (ActorRoom.Uid == 23 && !healingHerbsArtifact.Seen && ObjData.Name.Contains("table", StringComparison.OrdinalIgnoreCase))
			{
				ObjData.Name = healingHerbsArtifact.Name;
			}
			else if (ActorRoom.Uid == 47 && !libraryBookArtifact.Seen && ObjData.Name.ContainsAny(new string[] { "shelves", "shelf" }, StringComparison.OrdinalIgnoreCase))
			{
				ObjData.Name = libraryBookArtifact.Name;
			}
			else if (ActorRoom.Uid == 80 && !broadTippedArrowsArtifact.Seen && ObjData.Name.ContainsAny(new string[] { "gear", "shelves", "shelf" }, StringComparison.OrdinalIgnoreCase))
			{
				ObjData.Name = broadTippedArrowsArtifact.Name;
			}
			else if (ActorRoom.Uid == 33 && !healingDraughtsArtifact.Seen && ObjData.Name.Contains("field", StringComparison.OrdinalIgnoreCase))
			{
				ObjData.Name = healingDraughtsArtifact.Name;
			}
			else if (ActorRoom.Uid == 82 && !hiddenDoorArtifact.Seen && ObjData.Name.Contains("wall", StringComparison.OrdinalIgnoreCase))
			{
				ObjData.Name = hiddenDoorArtifact.Name;
			}
			else if (ActorRoom.Uid == 8 && !buzzSwordArtifact.Seen && ObjData.Name.ContainsAny(new string[] { "glimmer", "bushes", "metal" }, StringComparison.OrdinalIgnoreCase))
			{
				ObjData.Name = buzzSwordArtifact.Name;
			}
			else if (ActorRoom.Uid == 83 && !mapArtifact.Seen && ObjData.Name.ContainsAny(new string[] { "picture", "wall" }, StringComparison.OrdinalIgnoreCase))
			{
				ObjData.Name = mapArtifact.Name;
			}
		}

		public virtual void FinishParsingGreetCommand()
		{
			ResolveRecord();
		}

		public virtual void FinishParsingDisassembleCommand()
		{
			ResolveRecord(false);
		}

		public virtual void FinishParsingReassembleCommand()
		{
			ResolveRecord(false);
		}

		public virtual void FinishParsingCastCommand()
		{
			var castCommand = NextCommand as ICastCommand;

			Debug.Assert(castCommand != null);

			if (CurrToken < Tokens.Length)
			{
				castCommand.SpellName = InputBuf.ToString().Substring((Tokens[0] + " ").Length);

				castCommand.SpellName = Regex.Replace(castCommand.SpellName, @"\s+", "").Trim();

				CurrToken += (Tokens.Length - CurrToken);
			}

			while (true)
			{
				if (string.IsNullOrWhiteSpace(castCommand.SpellName))
				{
					gEngine.PrintVerbWhoOrWhat(NextCommand);

					gEngine.Buf.SetFormat("{0}", gEngine.In.ReadLine());

					castCommand.SpellName = Regex.Replace(gEngine.Buf.ToString(), @"\s+", "").Trim();
				}
				else
				{
					break;
				}
			}
		}
	}
}
