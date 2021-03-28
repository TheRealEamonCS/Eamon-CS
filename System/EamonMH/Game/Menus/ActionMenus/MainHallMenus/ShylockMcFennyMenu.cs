
// ShylockMcFennyMenu.cs

// Copyright (c) 2014+ by Michael Penner.  All rights reserved.

using System;
using System.Diagnostics;
using System.Text;
using Eamon;
using Eamon.Game.Attributes;
using Eamon.Game.Extensions;
using Eamon.Game.Menus;
using EamonMH.Framework.Menus.ActionMenus;
using static EamonMH.Game.Plugin.PluginContext;

namespace EamonMH.Game.Menus.ActionMenus
{
	[ClassMappings]
	public class ShylockMcFennyMenu : Menu, IShylockMcFennyMenu
	{
		public override void Execute()
		{
			RetCode rc;
			long i;

			gOut.Print("{0}", Globals.LineSep);

			gOut.Write("{0}You have no trouble spotting Shylock McFenny, the local banker, due to his large belly.  You attract his attention, and he comes over to you.{0}{0}\"Well, {1} my dear {2} what a pleasure to see you!  Do you want to make a deposit or a withdrawal?\"{0}",
				Environment.NewLine,
				Globals.Character.Name,
				Globals.Character.EvalGender("boy", "girl", "thing"));

			gOut.Print("You have {0} GP in hand, {1} GP in the bank.",
				Globals.Character.HeldGold,
				Globals.Character.BankGold);

			gOut.Print("{0}", Globals.LineSep);

			gOut.Write("{0}D=Deposit gold, W=Withdraw gold, X=Exit: ", Environment.NewLine);

			Buf.Clear();

			rc = Globals.In.ReadField(Buf, Constants.BufSize02, null, ' ', '\0', false, null, gEngine.ModifyCharToUpper, gEngine.IsCharDOrWOrX, gEngine.IsCharDOrWOrX);

			Debug.Assert(gEngine.IsSuccess(rc));

			Globals.Thread.Sleep(150);

			if (Buf.Length == 0 || Buf[0] == 'X')
			{
				goto Cleanup;
			}

			gOut.Print("{0}", Globals.LineSep);

			if (Buf[0] == 'D')
			{
				gOut.Print("Shylock gets a wide grin on his face and says, \"Good for you!  How much do you want to deposit?\"");

				gOut.Print("You have {0} GP in hand, {1} GP in the bank.",
					Globals.Character.HeldGold,
					Globals.Character.BankGold);

				gOut.Print("{0}", Globals.LineSep);

				gOut.Write("{0}Enter the amount to deposit: ", Environment.NewLine);

				Buf.Clear();

				rc = Globals.In.ReadField(Buf, Constants.BufSize01, null, ' ', '\0', false, null, null, gEngine.IsCharDigit, null);

				Debug.Assert(gEngine.IsSuccess(rc));

				Globals.Thread.Sleep(150);

				i = Convert.ToInt64(Buf.Trim().ToString());

				gOut.Print("{0}", Globals.LineSep);

				if (i > 0)
				{
					if (i <= Globals.Character.HeldGold)
					{
						if (Globals.Character.BankGold + i > Constants.MaxGoldValue)
						{
							i = Constants.MaxGoldValue - Globals.Character.BankGold;
						}

						Globals.Character.HeldGold -= i;

						Globals.Character.BankGold += i;

						Globals.CharactersModified = true;

						Buf.SetPrint("Shylock takes your money, puts it in his bag, listens to it jingle, then thanks you and walks away.");
					}
					else
					{
						Buf.SetPrint("Shylock was very pleased when you told him the sum, but when he discovered that you didn't have that much on you, he walked away shouting about fools who try to play tricks on a kindly banker.");
					}
				}
				else
				{
					Buf.SetPrint("The banker says, \"Well, if you change your mind and need my services, just let me know!\"");
				}

				gOut.Write("{0}", Buf);
			}
			else
			{
				Debug.Assert(Buf[0] == 'W');

				gOut.Print("Shylock says, \"Well, you have {0} gold piece{1} stored with me.  How many do you want to take back?\"", 
					Globals.Character.BankGold,
					Globals.Character.BankGold != 1 ? "s" : "");

				gOut.Print("You have {0} GP in hand, {1} GP in the bank.",
					Globals.Character.HeldGold,
					Globals.Character.BankGold);

				gOut.Print("{0}", Globals.LineSep);

				gOut.Write("{0}Enter the amount to withdraw: ", Environment.NewLine);

				Buf.Clear();

				rc = Globals.In.ReadField(Buf, Constants.BufSize01, null, ' ', '\0', false, null, null, gEngine.IsCharDigit, null);

				Debug.Assert(gEngine.IsSuccess(rc));

				Globals.Thread.Sleep(150);

				i = Convert.ToInt64(Buf.Trim().ToString());

				gOut.Print("{0}", Globals.LineSep);

				if (i > 0)
				{
					if (i <= Globals.Character.BankGold)
					{
						if (Globals.Character.HeldGold + i > Constants.MaxGoldValue)
						{
							i = Constants.MaxGoldValue - Globals.Character.HeldGold;
						}

						Globals.Character.BankGold -= i;

						Globals.Character.HeldGold += i;

						Globals.CharactersModified = true;

						Buf.SetFormat("{0}The banker hands you your gold and says, \"That leaves you with {1} gold piece{2} in my care.\"{0}{0}He shakes your hand and walks away.{0}", 
							Environment.NewLine,
							Globals.Character.BankGold,
							Globals.Character.BankGold != 1 ? "s" : "");
					}
					else
					{
						Buf.SetPrint("The banker throws you a terrible glance and says, \"That's more than you've got!  You know I don't make loans to your kind!\"  With that, he loses himself in the crowd.");
					}
				}
				else
				{
					Buf.SetPrint("The banker says, \"Well, if you change your mind and need my services, just let me know!\"");
				}

				gOut.Write("{0}", Buf);
			}

			Globals.In.KeyPress(Buf);

		Cleanup:

			;
		}

		public ShylockMcFennyMenu()
		{
			Buf = Globals.Buf;
		}
	}
}
