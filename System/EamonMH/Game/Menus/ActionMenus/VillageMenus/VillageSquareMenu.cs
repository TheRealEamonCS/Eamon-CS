
// VillageSquareMenu.cs

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
	public class VillageSquareMenu : Menu, IVillageSquareMenu
	{
		/// <summary></summary>
		public virtual bool AddedPotency { get; set; }

		public override void Execute()
		{
			RetCode rc;

			long p = 0;

			gOut.Print("{0}", Globals.LineSep);

			p = Constants.FountainPrice;

			gOut.Write("{0}You hear a mysterious voice say, \"Throw {1} gold piece{2} into the fountain and good fortune will be yours!\"{0}{0}Will you do it?{0}", Environment.NewLine, p, p != 1 ? "s" : "");

			gOut.Print("{0}", Globals.LineSep);

			gOut.Write("{0}Press Y for yes or N for no: ", Environment.NewLine);

			Buf.Clear();

			rc = Globals.In.ReadField(Buf, Constants.BufSize02, null, ' ', '\0', false, null, gEngine.ModifyCharToUpper, gEngine.IsCharYOrN, gEngine.IsCharYOrN);

			Debug.Assert(gEngine.IsSuccess(rc));

			Globals.Thread.Sleep(150);

			gOut.Print("{0}", Globals.LineSep);

			if (Buf.Length == 0 || Buf[0] == 'N')
			{
				goto Cleanup;
			}

			if (Globals.Character.HeldGold >= p)
			{
				var wc = 0L;

				rc = Globals.Character.GetWeaponCount(ref wc);

				Debug.Assert(gEngine.IsSuccess(rc));

				if (wc > 0 && !AddedPotency)
				{
					gOut.Print("\"For your generosity I will increase the potency of one of your weapons!\"");

					var rl = gEngine.RollDice(1, wc, 0);

					Globals.Character.GetWeapons(rl - 1).Field4++;

					// ClearExtraFields call omitted

					AddedPotency = true;
				}
				else
				{
					gOut.Print("The air around the fountain begins to glow briefly but nothing happens.");
				}

				Globals.Character.HeldGold -= p;

				Globals.CharactersModified = true;

				goto Cleanup;
			}
			else
			{
				gOut.Print("Your pouch of gold is too light!");

				goto Cleanup;
			}

		Cleanup:

			gOut.Print("You leave the area thinking the fountain has given up all its secrets.");

			Globals.In.KeyPress(Buf);
		}

		public VillageSquareMenu()
		{
			Buf = Globals.Buf;
		}
	}
}
