
// SayCommand.cs

// Copyright (c) 2014+ by Michael Penner.  All rights reserved.

using System;
using System.Diagnostics;
using Eamon.Framework.Primitive.Enums;
using Eamon.Game.Attributes;
using Eamon.Game.Extensions;
using EamonRT.Framework.Commands;
using EamonRT.Framework.Primitive.Enums;
using EamonRT.Framework.States;
using static EamonRT.Game.Plugin.PluginContext;

namespace EamonRT.Game.Commands
{
	[ClassMappings]
	public class SayCommand : Command, ISayCommand
	{
		public virtual string OriginalPhrase { get; set; }

		public virtual string PrintedPhrase { get; set; }

		public virtual string ProcessedPhrase { get; set; }

		public override void Execute()
		{
			Debug.Assert(!string.IsNullOrWhiteSpace(OriginalPhrase));

			PrintedPhrase = OriginalPhrase.Trim(new char[] { ' ', '"', '\'', '`' }).FirstCharToUpper();

			if (PrintedPhrase.Length == 0 || !Char.IsPunctuation(PrintedPhrase[PrintedPhrase.Length - 1]))
			{
				PrintedPhrase += ".";
			}

			ProcessedPhrase = PrintedPhrase.Trim(new char[] { '.', '?', '!' }).ToLower();

			ProcessEvents(EventType.BeforePrintSayText);

			if (GotoCleanup)
			{
				goto Cleanup;
			}

			PrintSayText(PrintedPhrase);

			ProcessEvents(EventType.AfterPrintSayText);

			if (GotoCleanup)
			{
				goto Cleanup;
			}

		Cleanup:

			if (NextState == null)
			{
				NextState = Globals.CreateInstance<IMonsterStartState>();
			}
		}

		public SayCommand()
		{
			SortOrder = 340;

			IsSentenceParserEnabled = false;

			Uid = 64;

			Name = "SayCommand";

			Verb = "say";

			Type = CommandType.Miscellaneous;
		}
	}
}
