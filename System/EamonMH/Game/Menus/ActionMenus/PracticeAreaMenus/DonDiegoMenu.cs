
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
using static EamonMH.Game.Plugin.Globals;

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

			gOut.Print("{0}", gEngine.LineSep);

			/* 
				Full Credit:  Derived wholly from Donald Brown's Classic Eamon

				File: MAIN HALL
				Line: 3010
			*/

			if (Rtio == null)
			{
				var c2 = gCharacter.GetMerchantAdjustedCharisma();

				Rtio = gEngine.GetMerchantRtio(c2);
			}

			gOut.Print("The man behind the counter says, \"I'm Don Leif Thor Robin Hercules Diego, at your service!  I teach weapons.  Select your weapon of interest.\"");

			gOut.Print("{0}", gEngine.LineSep);

			Buf.Clear();

			var weaponValues = EnumUtil.GetValues<Weapon>();

			for (i = 0; i < weaponValues.Count; i++)
			{
				weapon = gEngine.GetWeapon(weaponValues[(int)i]);

				Debug.Assert(weapon != null);

				Buf.AppendFormat("{0}{1}{2}={3}{4}",
					i == 0 ? Environment.NewLine : "",
					i != 0 ? "; " : "",
					(long)weaponValues[(int)i],
					weapon.MarcosName ?? weapon.Name,
					i == weaponValues.Count - 1 ? ": " : "");
			}

			gOut.Write("{0}", Buf);

			Buf.Clear();

			rc = gEngine.In.ReadField(Buf, gEngine.BufSize02, null, ' ', '\0', false, null, null, gEngine.IsCharWpnType, gEngine.IsCharWpnType);

			Debug.Assert(gEngine.IsSuccess(rc));

			gOut.Print("{0}", gEngine.LineSep);

			Debug.Assert(Buf.Length > 0);

			i = Convert.ToInt64(Buf.Trim().ToString());

			weapon = gEngine.GetWeapon((Weapon)i);

			Debug.Assert(weapon != null);

			ap = gEngine.GetMerchantAskPrice(gEngine.WeaponTrainingPrice, (double)Rtio);

			gOut.Print("\"My fee is {0} gold piece{1} per try.  Your current ability is {2}%.\"", ap, ap != 1 ? "s" : "", gCharacter.GetWeaponAbility(i));

			gOut.Print("Don asks you to enter his shop.  \"{0}, see that {1} over there?  It's all in the wrist...  ATTACK!\"", gCharacter.Name, i == (long)Weapon.Bow || i == (long)Weapon.Spear ? "target" : "dummy");

			while (true)
			{
				gEngine.In.KeyPress(Buf);

				gOut.Print("{0}", gEngine.LineSep);

				gOut.Print("Ability: {0}        Gold: {1}", gCharacter.GetWeaponAbility(i), gCharacter.HeldGold);

				if (gCharacter.HeldGold >= ap)
				{
					gOut.Write("{0}1=Attack; 2=Rest; X=Exit: ", Environment.NewLine);

					Buf.Clear();

					rc = gEngine.In.ReadField(Buf, gEngine.BufSize02, null, ' ', '\0', false, null, gEngine.ModifyCharToUpper, gEngine.IsChar1Or2OrX, gEngine.IsChar1Or2OrX);

					Debug.Assert(gEngine.IsSuccess(rc));

					gOut.Print("{0}", gEngine.LineSep);

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

							gCharacter.ModWeaponAbility(i, 2);
						}
						else if (rl > 50)
						{
							gOut.Print("\"A hit!  Good!  Now, continue.\"");

							gCharacter.ModWeaponAbility(i, 1);
						}
						else
						{
							gOut.Print("\"A miss!  Try harder!  Now, continue.\"");
						}

						if (gCharacter.GetWeaponAbility(i) > weapon.MaxValue)
						{
							gCharacter.SetWeaponAbility(i, weapon.MaxValue);
						}

						gCharacter.HeldGold -= ap;

						gEngine.CharactersModified = true;
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

			gEngine.In.KeyPress(Buf);
		}

		public DonDiegoMenu()
		{
			Buf = gEngine.Buf;
		}
	}
}
