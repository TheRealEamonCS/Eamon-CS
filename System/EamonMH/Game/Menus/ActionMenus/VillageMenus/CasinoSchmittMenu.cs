
// CasinoSchmittMenu.cs

// Copyright (c) 2014+ by Michael Penner.  All rights reserved.

using System;
using System.Diagnostics;
using System.Text;
using Eamon;
using Eamon.Game.Attributes;
using Eamon.Game.Menus;
using EamonMH.Framework.Menus.ActionMenus;
using static EamonMH.Game.Plugin.PluginContext;

namespace EamonMH.Game.Menus.ActionMenus
{
	[ClassMappings]
	public class CasinoSchmittMenu : Menu, ICasinoSchmittMenu
	{
		public override void Execute()
		{
			RetCode rc;

			gOut.Print("{0}", Globals.LineSep);

			gOut.Print("Schmitty, the owner, says, \"Welcome, {0}!  Perhaps a cocktail before you start?\"", Globals.Character.Name);

			gOut.Print("Some time later you make your way to the floor of the casino and stop at the roulette wheel.");

			gOut.Print("\"Place your bets!  Place your bets!  Would you like in {0} One?\"", Globals.Character.EvalGender("Mighty", "Fair", "Androgynous"));

			while (true)
			{
				Globals.In.KeyPress(Buf);

				gOut.Print("{0}", Globals.LineSep);

				gOut.Print("You have {0} gold piece{1}.  The house limit is 10,000.", Globals.Character.HeldGold, Globals.Character.HeldGold != 1 ? "s" : "");

				gOut.Write("{0}Your bet? (0 to leave): ", Environment.NewLine);

				Buf.Clear();

				rc = Globals.In.ReadField(Buf, Constants.BufSize01, null, ' ', '\0', false, null, null, gEngine.IsCharDigit, null);

				Debug.Assert(gEngine.IsSuccess(rc));

				Globals.Thread.Sleep(150);

				var bet = Convert.ToInt64(Buf.Trim().ToString());

				if (bet > 0 && bet <= Globals.Character.HeldGold && bet <= 10000)
				{
					gOut.Print("{0}", Globals.LineSep);

					gOut.Write("{0}The wheel is spinning ... ", Environment.NewLine);

					var cursorPosition = gOut.GetCursorPosition();

					for (var i = 0; i < 25; i++)
					{
						gOut.Write("-");

						Globals.Thread.Sleep(30);

						gOut.SetCursorPosition(cursorPosition);

						gOut.Write("\\");

						Globals.Thread.Sleep(30);

						gOut.SetCursorPosition(cursorPosition);

						gOut.Write("|");

						Globals.Thread.Sleep(30);

						gOut.SetCursorPosition(cursorPosition);

						gOut.Write("/");

						Globals.Thread.Sleep(30);

						gOut.SetCursorPosition(cursorPosition);
					}

					gOut.WriteLine(" ");

					var rl = gEngine.RollDice(1, 100, 0);

					if (rl > 80)
					{
						gOut.Print("You just won double your money!");

						bet *= 2;
					}
					else if (rl > 55)
					{
						gOut.Print("You won {0} gold piece{1}!", bet, bet != 1 ? "s" : "");
					}
					else
					{
						gOut.Print("Sorry, {0}, you lost.", Globals.Character.Name);

						bet = -bet;
					}

					Globals.Character.HeldGold += bet;

					Globals.CharactersModified = true;
				}
				else if (bet > 0)
				{
					gOut.Print("{0}", Globals.LineSep);

					gOut.Print("The dealer doesn't seem amused by your sense of humor.");
				}
				else if (bet == 0)
				{
					break;
				}
			}
		}

		public CasinoSchmittMenu()
		{
			Buf = Globals.Buf;
		}
	}
}
