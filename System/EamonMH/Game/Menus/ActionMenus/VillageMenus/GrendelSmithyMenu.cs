
// GrendelSmithyMenu.cs

// Copyright (c) 2014+ by Michael Penner.  All rights reserved.

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.Text.RegularExpressions;
using Eamon;
using Eamon.Framework;
using Eamon.Framework.Helpers;
using Eamon.Framework.Primitive.Classes;
using Eamon.Framework.Primitive.Enums;
using Eamon.Game.Attributes;
using Eamon.Game.Extensions;
using Eamon.Game.Menus;
using Eamon.Game.Utilities;
using EamonMH.Framework.Menus.ActionMenus;
using static EamonMH.Game.Plugin.PluginContext;

namespace EamonMH.Game.Menus.ActionMenus
{
	[ClassMappings]
	public class GrendelSmithyMenu : Menu, IGrendelSmithyMenu
	{
		/// <summary></summary>
		public virtual double? Rtio { get; set; }

		public override void Execute()
		{
			RetCode rc;
			bool imw = false;
			long ap = 0;
			long ap0;
			long ap1;
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

			i = Array.FindIndex(Globals.Character.Weapons, w => !w.IsActive());

			if (i < 0)
			{
				gOut.Print("Grendel says, \"I'm sorry, but you're going to have to try and sell one of your weapons at the store to the north.  You know the law:  No more than four weapons per person!  Come back when you've sold a weapon.\"");

				goto Cleanup;
			}

			gOut.Print("Grendel says, \"Would you care to look at my stock of used weapons?  You can also order a custom weapon if you'd prefer.\"");

			gOut.Print("{0}", Globals.LineSep);

			gOut.Write("{0}U=Used weapon, C=Custom weapon, X=Exit: ", Environment.NewLine);

			Buf.Clear();

			rc = Globals.In.ReadField(Buf, Constants.BufSize02, null, ' ', '\0', false, null, gEngine.ModifyCharToUpper, gEngine.IsCharUOrCOrX, gEngine.IsCharUOrCOrX);

			Debug.Assert(gEngine.IsSuccess(rc));

			Globals.Thread.Sleep(150);

			gOut.Print("{0}", Globals.LineSep);

			if (Buf.Length == 0 || Buf[0] == 'X')
			{
				goto Cleanup;
			}

			if (Buf[0] == 'U')
			{
				var weaponList = new List<string[]>()
				{
					null,
					new string[] { "Slaymor", "Elfkill" },
					new string[] { "Stinger", "Scrunch" },
					new string[] { "Centuri", "Falcoor" },
					new string[] { "Widower", "Flasher" },
					new string[] { "Slasher", "Freedom" }
				};

				gOut.Print("\"What type of weapon do you wish?\"");

				gOut.Print("{0}", Globals.LineSep);

				var j = (int)GetWeaponType();

				var weaponPrice = gEngine.GetWeaponPriceOrValue(weaponList[j][0], 12, (Weapon)j, 2, 8, j == (long)Weapon.Bow ? 2 : 1, true, ref imw);

				ap0 = gEngine.GetMerchantAskPrice(weaponPrice, (double)Rtio);

				weaponPrice = gEngine.GetWeaponPriceOrValue(weaponList[j][1], 24, (Weapon)j, 2, 16, j == (long)Weapon.Bow ? 2 : 1, true, ref imw);

				ap1 = gEngine.GetMerchantAskPrice(weaponPrice, (double)Rtio);

				gOut.Write("{0}\"I happen to have two in stock right now.\"{0}{0}1. {1} (2D8  / 12%) ...... {2} GP{0}2. {3} (2D16 / 24%) ..... {4} GP{0}", Environment.NewLine, weaponList[j][0], ap0, weaponList[j][1], ap1);

				gOut.Print("{0}", Globals.LineSep);

				gOut.Write("{0}Press the number of the weapon to buy or X to exit: ", Environment.NewLine);

				Buf.Clear();

				rc = Globals.In.ReadField(Buf, Constants.BufSize02, null, ' ', '\0', false, null, gEngine.ModifyCharToUpper, gEngine.IsChar1Or2OrX, gEngine.IsChar1Or2OrX);

				Debug.Assert(gEngine.IsSuccess(rc));

				Globals.Thread.Sleep(150);

				gOut.Print("{0}", Globals.LineSep);

				if (Buf.Length == 0 || Buf[0] == 'X')
				{
					goto Cleanup;
				}

				var k = (int)Convert.ToInt64(Buf.Trim().ToString());

				ap = k == 1 ? ap0 : ap1;

				if (Globals.Character.HeldGold >= ap)
				{
					gOut.Print("\"Good choice!  A great bargain!\"");

					UpdateCharacterWeapon(i, ap, Globals.CloneInstance(weaponList[j][k - 1]), j, 12 * k, 2, 8 * k, j == (long)Weapon.Bow ? 2 : 1);

					goto Cleanup;
				}
				else
				{
					PrintNotEnoughGold();

					goto Cleanup;
				}
			}
			else
			{
				gOut.Print("\"What do you want me to make?\"");

				gOut.Print("{0}", Globals.LineSep);

				var j = (int)GetWeaponType();

				var weapon = gEngine.GetWeapons((Weapon)j);

				Debug.Assert(weapon != null);

				var wpnName = (weapon.MarcosName ?? weapon.Name).ToLower();

				gOut.Print("\"What name should I inscribe on it?\"");

				gOut.Print("Note: this should be a capitalized singular proper name (eg, Trollsfire)");

				gOut.Print("{0}", Globals.LineSep);

				gOut.Write("{0}Enter weapon name: ", Environment.NewLine);

				Buf.Clear();

				rc = Globals.In.ReadField(Buf, Constants.CharArtNameLen, null, ' ', '\0', false, null, null, null, null);

				Debug.Assert(gEngine.IsSuccess(rc));

				Buf.SetFormat("{0}", Regex.Replace(Buf.ToString(), @"\s+", " ").Trim());

				var wpnArtifact = Globals.CreateInstance<IArtifact>(x =>
				{
					x.Name = gEngine.Capitalize(Buf.ToString());
				});

				Debug.Assert(wpnArtifact != null);

				var artifactHelper = Globals.CreateInstance<IArtifactHelper>(x =>
				{
					x.Record = wpnArtifact;
				});

				Debug.Assert(artifactHelper != null);

				Globals.Thread.Sleep(150);

				if (!artifactHelper.ValidateField("Name") || wpnArtifact.Name.Equals("NONE", StringComparison.OrdinalIgnoreCase))
				{
					wpnArtifact.Name = string.Format("Grendel{0}", wpnName);
				}

				var wpnName01 = wpnArtifact.Name;

				wpnArtifact.Dispose();

				wpnArtifact = null;

				gOut.Print("{0}", Globals.LineSep);

				gOut.Write("{0}\"I do have limits of craftsmanship.\"{0}{0}    Complexity    Dice   Sides{0}      1%-50%       1-3    1-12{0}", Environment.NewLine);

				gOut.Print("{0}", Globals.LineSep);

				gOut.Write("{0}Enter complexity: ", Environment.NewLine);

				Buf.Clear();

				rc = Globals.In.ReadField(Buf, Constants.BufSize01, null, ' ', '\0', false, null, null, gEngine.IsCharDigit, null);

				Debug.Assert(gEngine.IsSuccess(rc));

				var wpnComplexity = Convert.ToInt64(Buf.Trim().ToString());

				if (wpnComplexity < 1)
				{
					wpnComplexity = 1;
				}
				else if (wpnComplexity > 50)
				{
					wpnComplexity = 50;
				}

				gOut.Print("{0}", Globals.LineSep);

				gOut.Write("{0}Enter number of dice: ", Environment.NewLine);

				Buf.Clear();

				rc = Globals.In.ReadField(Buf, Constants.BufSize01, null, ' ', '\0', false, null, null, gEngine.IsCharDigit, null);

				Debug.Assert(gEngine.IsSuccess(rc));

				var wpnDice = Convert.ToInt64(Buf.Trim().ToString());

				if (wpnDice < 1)
				{
					wpnDice = 1;
				}
				else if (wpnDice > 3)
				{
					wpnDice = 3;
				}

				gOut.Print("{0}", Globals.LineSep);

				gOut.Write("{0}Enter number of dice sides: ", Environment.NewLine);

				Buf.Clear();

				rc = Globals.In.ReadField(Buf, Constants.BufSize01, null, ' ', '\0', false, null, null, gEngine.IsCharDigit, null);

				Debug.Assert(gEngine.IsSuccess(rc));

				var wpnSides = Convert.ToInt64(Buf.Trim().ToString());

				if (wpnSides < 1)
				{
					wpnSides = 1;
				}
				else if (wpnSides > 12)
				{
					wpnSides = 12;
				}

				var weaponPrice = gEngine.GetWeaponPriceOrValue(wpnName01, wpnComplexity, (Weapon)j, wpnDice, wpnSides, j == (long)Weapon.Bow ? 2 : 1, true, ref imw);

				ap = gEngine.GetMerchantAskPrice(weaponPrice, (double)Rtio);

				gOut.Print("{0}", Globals.LineSep);

				gOut.Print("\"I can make you a {0}H {1}D{2} {3} with complexity of {4}% called {5} for {6} gold piece{7}.  Should I proceed?\"", j == (long)Weapon.Bow ? 2 : 1, wpnDice, wpnSides, wpnName, wpnComplexity, wpnName01, ap, ap != 1 ? "s" : "");

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

				if (Globals.Character.HeldGold >= ap)
				{
					gOut.Print("Grendel works on your weapon, often calling in wizards and weapon experts.  Finally he finishes.  \"I think you will be satisfied with this.\" he says modestly.");

					UpdateCharacterWeapon(i, ap, wpnName01, j, wpnComplexity, wpnDice, wpnSides, j == (long)Weapon.Bow ? 2 : 1);

					goto Cleanup;
				}
				else
				{
					PrintNotEnoughGold();

					goto Cleanup;
				}
			}

		Cleanup:

			gOut.Print("\"Goodbye, {0}!  Come again.\"", Globals.Character.Name);

			Globals.In.KeyPress(Buf);
		}

		/// <summary></summary>
		/// <returns></returns>
		public virtual long GetWeaponType()
		{
			RetCode rc;
			long i;

			Buf.Clear();

			var weaponValues = EnumUtil.GetValues<Weapon>();

			for (i = 0; i < weaponValues.Count; i++)
			{
				var weapon = gEngine.GetWeapons(weaponValues[(int)i]);

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

			return Convert.ToInt64(Buf.Trim().ToString());
		}

		/// <summary></summary>
		/// <param name="i"></param>
		/// <param name="ap"></param>
		/// <param name="name"></param>
		/// <param name="type"></param>
		/// <param name="complexity"></param>
		/// <param name="dice"></param>
		/// <param name="sides"></param>
		/// <param name="numHands"></param>
		public virtual void UpdateCharacterWeapon(long i, long ap, string name, long type, long complexity, long dice, long sides, long numHands)
		{
			var cw = Globals.CreateInstance<ICharacterArtifact>(x =>
			{
				x.Name = name;
				x.IsPlural = false;
				x.PluralType = PluralType.None;
				x.ArticleType = ArticleType.None;
				x.Field1 = complexity;
				x.Field2 = type;
				x.Field3 = dice;
				x.Field4 = sides;
				x.Field5 = numHands;
			});

			Globals.Character.SetWeapons(i, cw);

			Globals.Character.GetWeapons(i).Parent = Globals.Character;

			Globals.Character.StripUniqueCharsFromWeaponNames();

			Globals.Character.AddUniqueCharsToWeaponNames();

			Globals.Character.HeldGold -= ap;

			Globals.CharactersModified = true;
		}

		/// <summary></summary>
		public virtual void PrintNotEnoughGold()
		{
			gOut.Print("\"Sorry, but you don't seem to have enough gold to pay for your weapon at this time.  Come back when you have enough.\"");
		}

		public GrendelSmithyMenu()
		{
			Buf = Globals.Buf;
		}
	}
}
