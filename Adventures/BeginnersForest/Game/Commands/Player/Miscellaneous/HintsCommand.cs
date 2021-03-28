
// HintsCommand.cs

// Copyright (c) 2014+ by Michael Penner.  All rights reserved.

using System;
using System.Collections.Generic;
using System.Diagnostics;
using Eamon.Framework;
using Eamon.Game.Attributes;
using EamonRT.Framework.Commands;
using static BeginnersForest.Game.Plugin.PluginContext;

namespace BeginnersForest.Game.Commands
{
	[ClassMappings]
	public class HintsCommand : EamonRT.Game.Commands.HintsCommand, IHintsCommand
	{
		public override void PrintHintsQuestion()
		{
			Debug.Assert(ActiveHintList != null);

			var prefix = "Beginner's Forest -- ";

			var question = ActiveHintList[ActiveHintListIndex].Question;

			if (question.StartsWith(prefix))
			{
				question = question.Substring(prefix.Length);
			}

			gOut.Write("{0}{1,3}. {2}", Environment.NewLine, ActiveHintListIndex + 1, question);
		}
	}
}
