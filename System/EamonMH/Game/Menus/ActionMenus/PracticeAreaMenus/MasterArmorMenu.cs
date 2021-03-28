
// MasterArmorMenu.cs

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
	public class MasterArmorMenu : Menu, IMasterArmorMenu
	{
		/// <summary></summary>
		public virtual double? Rtio { get; set; }

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

			ap = gEngine.GetMerchantAskPrice(Constants.ArmorTrainingPrice, (double)Rtio);

			gOut.Print("A giant steps forward and says, \"Me good teacher.  You need help with armor skills?  Good, I teach for {0} gold piece{1} per strike.  Come with me!\"", ap, ap != 1 ? "s" : "");

			gOut.Print("He takes you to an open area in the shop and says, \"Try and make me miss you!\"");

			while (true)
			{
				Globals.In.KeyPress(Buf);

				gOut.Print("{0}", Globals.LineSep);

				gOut.Print("Ability: {0}        Gold: {1}", Globals.Character.ArmorExpertise, Globals.Character.HeldGold);

				if (Globals.Character.HeldGold >= ap)
				{
					gOut.Write("{0}1=Hit, 2=Rest, X=Exit: ", Environment.NewLine);

					Buf.Clear();

					rc = Globals.In.ReadField(Buf, Constants.BufSize02, null, ' ', '\0', false, null, gEngine.ModifyCharToUpper, gEngine.IsChar1Or2OrX, gEngine.IsChar1Or2OrX);

					Debug.Assert(gEngine.IsSuccess(rc));

					Globals.Thread.Sleep(150);

					gOut.Print("{0}", Globals.LineSep);

					if (Buf.Length == 0 || Buf[0] == 'X')
					{
						break;
					}
					else if (Buf[0] == '1')
					{
						var rl = gEngine.RollDice(1, 100, 0);

						if (rl > 90)
						{
							gOut.Print("\"Dat was real good!\"");

							Globals.Character.ArmorExpertise += 2;
						}
						else if (rl > 50)
						{
							gOut.Print("\"Good!  You getting it!\"");

							Globals.Character.ArmorExpertise++;
						}
						else
						{
							gOut.Print("\"You need move much faster!  Now get up!\"");
						}

						if (Globals.Character.ArmorExpertise > 79)
						{
							Globals.Character.ArmorExpertise = 79;
						} 

						Globals.Character.HeldGold -= ap;

						Globals.CharactersModified = true;
					}
					else
					{
						gOut.Print("CLANK ... PUFF ... CLANK.  \"You okay?\"");
					}
				}
				else
				{
					gOut.Print("\"You not carry enough gold!\"");

					break;
				}
			}

			gOut.Print("\"Me sorry see you go.  Bye now!\"");

			Globals.In.KeyPress(Buf);
		}

		public MasterArmorMenu()
		{
			Buf = Globals.Buf;
		}
	}
}
