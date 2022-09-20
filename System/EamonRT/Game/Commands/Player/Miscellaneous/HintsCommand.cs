
// HintsCommand.cs

// Copyright (c) 2014+ by Michael Penner.  All rights reserved.

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using Eamon;
using Eamon.Framework;
using Eamon.Framework.Primitive.Enums;
using Eamon.Game.Attributes;
using EamonRT.Framework.Commands;
using EamonRT.Framework.States;
using static EamonRT.Game.Plugin.PluginContext;

namespace EamonRT.Game.Commands
{
	[ClassMappings]
	public class HintsCommand : Command, IHintsCommand
	{
		/// <summary></summary>
		public virtual IList<IHint> ActiveHintList { get; set; }

		/// <summary></summary>
		public virtual int ActiveHintListIndex { get; set; }

		/// <summary></summary>
		public virtual int HintAnswerIndex { get; set; }

		public override void Execute()
		{
			RetCode rc;

			if (Globals.Database.GetHintsCount() <= 0)
			{
				PrintNoHintsAvailable();

				goto Cleanup;
			}

			ActiveHintList = Globals.Database.HintTable.Records.Where(h => h.Active).OrderBy(h => h.Uid).ToList();

			if (ActiveHintList.Count <= 0)
			{
				PrintNoHintsAvailableNow();

				goto Cleanup;
			}

			PrintYourQuestion();

			for (ActiveHintListIndex = 0; ActiveHintListIndex < ActiveHintList.Count; ActiveHintListIndex++)
			{
				PrintHintsQuestion(ActiveHintListIndex + 1, ActiveHintList[ActiveHintListIndex].Question);
			}

			PrintEnterHintChoice();

			Globals.Buf.Clear();

			rc = Globals.In.ReadField(Globals.Buf, Constants.BufSize01, null, ' ', '\0', false, null, gEngine.ModifyCharToUpper, gEngine.IsCharDigit, null);

			Debug.Assert(gEngine.IsSuccess(rc));

			ActiveHintListIndex = Convert.ToInt32(Globals.Buf.Trim().ToString()) - 1;

			if (ActiveHintListIndex < 0 || ActiveHintListIndex >= ActiveHintList.Count)
			{
				goto Cleanup;
			}

			PrintHintsQuestion01(ActiveHintList[ActiveHintListIndex].Question);

			for (HintAnswerIndex = 0; HintAnswerIndex < ActiveHintList[ActiveHintListIndex].NumAnswers; HintAnswerIndex++)
			{
				PrintHintsAnswer(ActiveHintList[ActiveHintListIndex].GetAnswers(HintAnswerIndex), Globals.Buf);

				if (HintAnswerIndex + 1 < ActiveHintList[ActiveHintListIndex].NumAnswers)
				{
					PrintAnotherHint();

					Globals.Buf.Clear();

					rc = Globals.In.ReadField(Globals.Buf, Constants.BufSize02, null, ' ', '\0', false, null, gEngine.ModifyCharToUpper, gEngine.IsCharYOrN, null);

					Debug.Assert(gEngine.IsSuccess(rc));

					if (Globals.Buf.Length == 0 || Globals.Buf[0] == 'N')
					{
						break;
					}
				}
			}

		Cleanup:

			if (NextState == null)
			{
				NextState = Globals.CreateInstance<IStartState>();
			}
		}

		public override bool ShouldPreTurnProcess()
		{
			return false;
		}

		public HintsCommand()
		{
			SortOrder = 390;

			IsSentenceParserEnabled = false;

			IsDarkEnabled = true;

			Name = "HintsCommand";

			Verb = "hints";

			Type = CommandType.Miscellaneous;
		}
	}
}
