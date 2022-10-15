
// HintsCommand.cs

// Copyright (c) 2014+ by Michael Penner.  All rights reserved.

using System;
using System.Diagnostics;
using Eamon.Game.Attributes;
using EamonRT.Framework.Commands;
using static BeginnersForest.Game.Plugin.PluginContext;

namespace BeginnersForest.Game.Commands
{
	[ClassMappings]
	public class HintsCommand : EamonRT.Game.Commands.HintsCommand, IHintsCommand
	{
		public override void PrintHintQuestion(long hintNum, string question)
		{
			Debug.Assert(hintNum > 0 && question != null);

			var prefix = "Beginner's Forest -- ";

			if (question.StartsWith(prefix))
			{
				question = question.Substring(prefix.Length);
			}

			gOut.Write("{0}{1,3}. {2}", Environment.NewLine, hintNum, question);
		}
	}
}
