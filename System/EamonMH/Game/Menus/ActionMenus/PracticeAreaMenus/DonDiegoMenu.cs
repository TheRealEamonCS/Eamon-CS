
// DonDiegoMenu.cs

// Copyright (c) 2014+ by Michael Penner.  All rights reserved.

using System;
using System.Diagnostics;
using System.Text;
using Eamon;
using Eamon.Framework.Primitive.Classes;
using Eamon.Framework.Primitive.Enums;
using Eamon.Game.Attributes;
using Eamon.Game.Menus;
using Eamon.Game.Utilities;
using EamonMH.Framework.Menus.ActionMenus;
using static EamonMH.Game.Plugin.PluginContext;

namespace EamonMH.Game.Menus.ActionMenus
{
	[ClassMappings]
	public class DonDiegoMenu : Menu, IDonDiegoMenu
	{
		/// <summary></summary>
		public virtual double? Rtio { get; set; }

		public override void Execute()
		{
			IWeapon weapon;
			RetCode rc;
			long ap = 0;
			long i;

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

			gOut.Print("The man behind the counter says, \"I'm Don Leif Thor Robin Hercules Diego, at your service!  I teach weapons.  Select your weapon of interest.\"");

			gOut.Print("{0}", Globals.LineSep);

			Buf.Clear();

			var weaponValues = EnumUtil.GetValues<Weapon>();

			for (i = 0; i < weaponValues.Count; i++)
			{
				weapon = gEngine.GetWeapons(weaponValues[(int)i]);

				Debug.Assert(weapon != null);

				Buf.AppendFormat("{0}{1}{2}={3}{4}",
					i == 0 ? Environment.NewLine : "",
					i != 0 ? ", " : "",
					(long)weaponValues[(int)i],
					weapon.MarcosName ?? weapon.Name,
					i == weaponValues.Count - 1 ? ": " : "");
			}

			gOut.Write("{0}", Buf);

			Buf.Clear();

			rc = Globals.In.ReadField(Buf, Constants.BufSize02, null, ' ', '\0', false, null, null, gEngine.IsCharWpnType, gEngine.IsCharWpnType);

			Debug.Assert(gEngine.IsSuccess(rc));

			Globals.Thread.Sleep(150);

			gOut.Print("{0}", Globals.LineSep);

			Debug.Assert(Buf.Length > 0);

			i = Convert.ToInt64(Buf.Trim().ToString());

			weapon = gEngine.GetWeapons((Weapon)i);

			Debug.Assert(weapon != null);

			ap = gEngine.GetMerchantAskPrice(Constants.WeaponTrainingPrice, (double)Rtio);

			gOut.Print("\"My fee is {0} gold piece{1} per try.  Your current ability is {2}%.\"", ap, ap != 1 ? "s" : "", Globals.Character.GetWeaponAbilities(i));

			gOut.Print("Don asks you to enter his shop.  \"{0}, see that {1} over there?  It's all in the wrist...  ATTACK!\"", Globals.Character.Name, i == (long)Weapon.Bow || i == (long)Weapon.Spear ? "target" : "dummy");

			while (true)
			{
				Globals.In.KeyPress(Buf);

				gOut.Print("{0}", Globals.LineSep);

				gOut.Print("Ability: {0}        Gold: {1}", Globals.Character.GetWeaponAbilities(i), Globals.Character.HeldGold);

				if (Globals.Character.HeldGold >= ap)
				{
					gOut.Write("{0}1=Attack, 2=Rest, X=Exit: ", Environment.NewLine);

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
							gOut.Print("\"A critical hit!  Very good!  Now, continue.\"");

							Globals.Character.ModWeaponAbilities(i, 2);
						}
						else if (rl > 50)
						{
							gOut.Print("\"A hit!  Good!  Now, continue.\"");

							Globals.Character.ModWeaponAbilities(i, 1);
						}
						else
						{
							gOut.Print("\"A miss!  Try harder!  Now, continue.\"");
						}

						if (Globals.Character.GetWeaponAbilities(i) > weapon.MaxValue)
						{
							Globals.Character.SetWeaponAbilities(i, weapon.MaxValue);
						}

						Globals.Character.HeldGold -= ap;

						Globals.CharactersModified = true;
					}
					else
					{
						gOut.Print("PUFF  PUFF  PUFF.  \"Stamina is important!\"");
					}
				}
				else
				{
					gOut.Print("\"Sorry, but you have too little gold!\"");

					break;
				}
			}

			gOut.Print("\"Goodbye and good luck!\"");

			Globals.In.KeyPress(Buf);
		}

		public DonDiegoMenu()
		{
			Buf = Globals.Buf;
		}
	}
}
