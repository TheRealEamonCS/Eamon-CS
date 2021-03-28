
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
				gOut.Print("There are no hints available for this adventure.");

				goto Cleanup;
			}

			ActiveHintList = Globals.Database.HintTable.Records.Where(h => h.Active).OrderBy(h => h.Uid).ToList();

			if (ActiveHintList.Count <= 0)
			{
				gOut.Print("There are no hints available at this point in the adventure.");

				goto Cleanup;
			}

			gOut.Print("Your question?");

			for (ActiveHintListIndex = 0; ActiveHintListIndex < ActiveHintList.Count; ActiveHintListIndex++)
			{
				PrintHintsQuestion();
			}

			gOut.Write("{0}{0}Enter your choice: ", Environment.NewLine);

			Globals.Buf.Clear();

			rc = Globals.In.ReadField(Globals.Buf, Constants.BufSize01, null, ' ', '\0', false, null, gEngine.ModifyCharToUpper, gEngine.IsCharDigit, null);

			Debug.Assert(gEngine.IsSuccess(rc));

			ActiveHintListIndex = Convert.ToInt32(Globals.Buf.Trim().ToString()) - 1;

			if (ActiveHintListIndex < 0 || ActiveHintListIndex >= ActiveHintList.Count)
			{
				goto Cleanup;
			}

			PrintHintsQuestion01();

			for (HintAnswerIndex = 0; HintAnswerIndex < ActiveHintList[ActiveHintListIndex].NumAnswers; HintAnswerIndex++)
			{
				PrintHintsAnswer();

				if (HintAnswerIndex + 1 < ActiveHintList[ActiveHintListIndex].NumAnswers)
				{
					gOut.Write("{0}Another (Y/N): ", Environment.NewLine);

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

		public virtual void PrintHintsQuestion()
		{
			Debug.Assert(ActiveHintList != null);

			gOut.Write("{0}{1,3}. {2}", Environment.NewLine, ActiveHintListIndex + 1, ActiveHintList[ActiveHintListIndex].Question);
		}

		public virtual void PrintHintsQuestion01()
		{
			Debug.Assert(ActiveHintList != null);

			gOut.Print("{0}", ActiveHintList[ActiveHintListIndex].Question);
		}

		public virtual void PrintHintsAnswer()
		{
			Debug.Assert(ActiveHintList != null);

			gEngine.PrintMacroReplacedPagedString(ActiveHintList[ActiveHintListIndex].GetAnswers(HintAnswerIndex), Globals.Buf);
		}

		public HintsCommand()
		{
			SortOrder = 390;

			IsSentenceParserEnabled = false;

			IsDarkEnabled = true;

			Uid = 56;

			Name = "HintsCommand";

			Verb = "hints";

			Type = CommandType.Miscellaneous;
		}
	}
}
