
// InfoBoothMenu.cs

// Copyright (c) 2014+ by Michael Penner.  All rights reserved.

using System;
using System.Diagnostics;
using Eamon;
using Eamon.Game.Attributes;
using Eamon.Game.Menus;
using EamonMH.Framework.Menus.ActionMenus;
using static EamonMH.Game.Plugin.PluginContext;

namespace EamonMH.Game.Menus.ActionMenus
{
	[ClassMappings]
	public class InfoBoothMenu : Menu, IInfoBoothMenu
	{
		/// <summary></summary>
		public virtual double? Rtio { get; set; }

		/// <summary></summary>
		public virtual bool GotInfo { get; set; }

		public override void Execute()
		{
			RetCode rc;

			long ap = 0;

			gOut.Print("{0}", Globals.LineSep);

			/* 
				Full Credit:  Derived wholly from Donald Brown's Classic Eamon

				File: MAIN HALL
				Line: 3010
			*/

			if (Rtio == null)
			{
				var c2 = Globals.Character.GetMerchantAdjustedCharisma();

				Rtio = gEngine.GetMerchantRtio(c2);
			}

			ap = gEngine.GetMerchantAskPrice(Constants.InfoBoothPrice, (double)Rtio);

			gOut.Write("{0}The man leans over the counter and says, \"I give general directions for free and for a small charge of {1} gold piece{2}, I give interesting and useful information.\"{0}{0}\"So what'll it be?\"{0}", Environment.NewLine, ap, ap != 1 ? "s" : "");

			gOut.Print("{0}", Globals.LineSep);

			gOut.Write("{0}D=Get directions, I=Buy info, X=Exit: ", Environment.NewLine);

			Buf.Clear();

			rc = Globals.In.ReadField(Buf, Constants.BufSize02, null, ' ', '\0', false, null, gEngine.ModifyCharToUpper, gEngine.IsCharDOrIOrX, gEngine.IsCharDOrIOrX);

			Debug.Assert(gEngine.IsSuccess(rc));

			Globals.Thread.Sleep(150);

			gOut.Print("{0}", Globals.LineSep);

			if (Buf.Length == 0 || Buf[0] == 'X')
			{
				goto Cleanup;
			}

			if (Buf[0] == 'D')
			{
				gOut.Print("\"The proprietor of the shop to the northwest coaches spell casting.  It's expensive, though.\"");

				gOut.Print("\"The weapons training school to the southwest is quite popular.  The owner is an excellent instructor.\"");

				gOut.Print("\"The eastern shop belongs to an unusual man.  But he's quite adept at teaching armor skills.\"");

				goto Cleanup;
			}
			else
			{
				if (Globals.Character.HeldGold >= ap)
				{
					if (!GotInfo)
					{
						gOut.Print("\"If you go to the southeast corner of this area and look to the south, you may discover wealth!\"");

						var rl = gEngine.RollDice(1, 100, 0);

						if (rl <= 50)
						{
							gOut.Print("You search around and discover a loose block with an old sack hidden behind it.  Inside the sack you find 100 gold coins!");

							Globals.Character.HeldGold += 100;
						}
						else
						{
							gOut.Print("You search around but find nothing there.  Obviously a bad tip.");
						}

						GotInfo = true;
					}
					else
					{
						gOut.Print("\"Next time you're in the village, you may wish to look at the statue above the fountain.\"");
					}

					Globals.Character.HeldGold -= ap;

					Globals.CharactersModified = true;

					goto Cleanup;
				}
				else
				{
					gOut.Print("\"You don't have enough gold for my tip!\"");

					goto Cleanup;
				}
			}

		Cleanup:

			gOut.Print("The man at the Info Booth smiles at you as he turns to help someone else.");

			Globals.In.KeyPress(Buf);
		}

		public InfoBoothMenu()
		{
			Buf = Globals.Buf;
		}
	}
}
