
// GrendelSmithyMenu.cs

// Copyright (c) 2014+ by Michael Penner.  All rights reserved.

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using Eamon;
using Eamon.Framework;
using Eamon.Framework.Helpers;
using Eamon.Framework.Primitive.Enums;
using Eamon.Game.Attributes;
using Eamon.Game.Extensions;
using Eamon.Game.Menus;
using Eamon.Game.Utilities;
using EamonMH.Framework.Menus.ActionMenus;
using static EamonMH.Game.Plugin.Globals;

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

			var artifactList = gCharacter.GetCarriedList().Where(a => a.GeneralWeapon != null).ToList();

			if (artifactList.Count >= gEngine.NumCharacterWeapons)
			{
				gOut.Print("Grendel says, \"I'm sorry, but you're going to have to try and sell one of your weapons at the store to the north.  You know the law:  No more than four weapons per person!  Come back when you've sold a weapon.\"");

				goto Cleanup;
			}

			gOut.Print("Grendel says, \"Would you care to look at my stock of used weapons?  You can also order a custom weapon if you'd prefer.\"");

			gOut.Print("{0}", gEngine.LineSep);

			gOut.Write("{0}U=Used weapon; C=Custom weapon; X=Exit: ", Environment.NewLine);

			Buf.Clear();

			rc = gEngine.In.ReadField(Buf, gEngine.BufSize02, null, ' ', '\0', false, null, gEngine.ModifyCharToUpper, gEngine.IsCharUOrCOrX, gEngine.IsCharUOrCOrX);

			Debug.Assert(gEngine.IsSuccess(rc));

			gOut.Print("{0}", gEngine.LineSep);

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

				gOut.Print("{0}", gEngine.LineSep);

				var j = (int)GetWeaponType();

				var weaponPrice = gEngine.GetWeaponPriceOrValue(weaponList[j][0], 12, (Weapon)j, 2, 8, j == (long)Weapon.Bow ? 2 : 1, true, ref imw);

				ap0 = gEngine.GetMerchantAskPrice(weaponPrice, (double)Rtio);

				weaponPrice = gEngine.GetWeaponPriceOrValue(weaponList[j][1], 24, (Weapon)j, 2, 16, j == (long)Weapon.Bow ? 2 : 1, true, ref imw);

				ap1 = gEngine.GetMerchantAskPrice(weaponPrice, (double)Rtio);

				gOut.Write("{0}\"I happen to have two in stock right now.\"{0}{0}1. {1} (2D8  / 12%) {2} {3} GP{0}2. {4} (2D16 / 24%) {5} {6} GP{0}", Environment.NewLine, weaponList[j][0], new String(gEngine.EnableScreenReaderMode ? ' ' : '.', 6), ap0, weaponList[j][1], new String(gEngine.EnableScreenReaderMode ? ' ' : '.', 5), ap1);

				gOut.Print("{0}", gEngine.LineSep);

				gOut.Write("{0}Press the number of the weapon to buy or X to exit: ", Environment.NewLine);

				Buf.Clear();

				rc = gEngine.In.ReadField(Buf, gEngine.BufSize02, null, ' ', '\0', false, null, gEngine.ModifyCharToUpper, gEngine.IsChar1Or2OrX, gEngine.IsChar1Or2OrX);

				Debug.Assert(gEngine.IsSuccess(rc));

				gOut.Print("{0}", gEngine.LineSep);

				if (Buf.Length == 0 || Buf[0] == 'X')
				{
					goto Cleanup;
				}

				var k = (int)Convert.ToInt64(Buf.Trim().ToString());

				ap = k == 1 ? ap0 : ap1;

				if (gCharacter.HeldGold >= ap)
				{
					gOut.Print("\"Good choice!  A great bargain!\"");

					CreateCharacterWeapon(ap, gEngine.CloneInstance(weaponList[j][k - 1]), j, 12 * k, 2, 8 * k, j == (long)Weapon.Bow ? 2 : 1);

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

				gOut.Print("{0}", gEngine.LineSep);

				var j = (int)GetWeaponType();

				var weapon = gEngine.GetWeapon((Weapon)j);

				Debug.Assert(weapon != null);

				var wpnName = (weapon.MarcosName ?? weapon.Name).ToLower();

				gOut.Print("\"What name should I inscribe on it?\"");

				gOut.Print("Note: this should be a capitalized singular proper name (e.g., Trollsfire)");

				gOut.Print("{0}", gEngine.LineSep);

				gOut.Write("{0}Enter weapon name: ", Environment.NewLine);

				Buf.Clear();

				rc = gEngine.In.ReadField(Buf, gEngine.ArtNameLen, null, ' ', '\0', false, null, null, null, null);

				Debug.Assert(gEngine.IsSuccess(rc));

				Buf.SetFormat("{0}", Regex.Replace(Buf.ToString(), @"\s+", " ").Trim());

				var wpnArtifact = gEngine.CreateInstance<IArtifact>(x =>
				{
					x.Name = gEngine.Capitalize(Buf.ToString());
				});

				Debug.Assert(wpnArtifact != null);

				var artifactHelper = gEngine.CreateInstance<IArtifactHelper>(x =>
				{
					x.RecordTable = gDatabase.ArtifactTable;
					
					x.Record = wpnArtifact;
				});

				Debug.Assert(artifactHelper != null);

				if (!artifactHelper.ValidateField("Name") || wpnArtifact.Name.Equals("NONE", StringComparison.OrdinalIgnoreCase))
				{
					wpnArtifact.Name = string.Format("Grendel{0}", wpnName);
				}

				var wpnName01 = wpnArtifact.Name;

				wpnArtifact.Dispose();

				wpnArtifact = null;

				gOut.Print("{0}", gEngine.LineSep);

				gOut.Write("{0}\"I do have limits of craftsmanship.\"{0}{0}    Complexity    Dice   Sides{0}      1%-50%       1-3    1-12{0}", Environment.NewLine);

				gOut.Print("{0}", gEngine.LineSep);

				gOut.Write("{0}Enter complexity: ", Environment.NewLine);

				Buf.Clear();

				rc = gEngine.In.ReadField(Buf, gEngine.BufSize01, null, ' ', '\0', false, null, null, gEngine.IsCharDigit, null);

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

				gOut.Print("{0}", gEngine.LineSep);

				gOut.Write("{0}Enter number of dice: ", Environment.NewLine);

				Buf.Clear();

				rc = gEngine.In.ReadField(Buf, gEngine.BufSize01, null, ' ', '\0', false, null, null, gEngine.IsCharDigit, null);

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

				gOut.Print("{0}", gEngine.LineSep);

				gOut.Write("{0}Enter number of dice sides: ", Environment.NewLine);

				Buf.Clear();

				rc = gEngine.In.ReadField(Buf, gEngine.BufSize01, null, ' ', '\0', false, null, null, gEngine.IsCharDigit, null);

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

				gOut.Print("{0}", gEngine.LineSep);

				gOut.Print("\"I can make you a {0}H {1}D{2} {3} with complexity of {4}% called {5} for {6} gold piece{7}.  Should I proceed?\"", j == (long)Weapon.Bow ? 2 : 1, wpnDice, wpnSides, wpnName, wpnComplexity, wpnName01, ap, ap != 1 ? "s" : "");

				gOut.Print("{0}", gEngine.LineSep);

				gOut.Write("{0}Press Y for yes or N for no: ", Environment.NewLine);

				Buf.Clear();

				rc = gEngine.In.ReadField(Buf, gEngine.BufSize02, null, ' ', '\0', false, null, gEngine.ModifyCharToUpper, gEngine.IsCharYOrN, gEngine.IsCharYOrN);

				Debug.Assert(gEngine.IsSuccess(rc));

				gOut.Print("{0}", gEngine.LineSep);

				if (Buf.Length == 0 || Buf[0] == 'N')
				{
					goto Cleanup;
				}

				if (gCharacter.HeldGold >= ap)
				{
					gOut.Print("Grendel works on your weapon, often calling in wizards and weapon experts.  Finally he finishes.  \"I think you will be satisfied with this.\" he says modestly.");

					CreateCharacterWeapon(ap, wpnName01, j, wpnComplexity, wpnDice, wpnSides, j == (long)Weapon.Bow ? 2 : 1);

					goto Cleanup;
				}
				else
				{
					PrintNotEnoughGold();

					goto Cleanup;
				}
			}

		Cleanup:

			gOut.Print("\"Goodbye, {0}!  Come again.\"", gCharacter.Name);

			gEngine.In.KeyPress(Buf);
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
				var weapon = gEngine.GetWeapon(weaponValues[(int)i]);

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

			return Convert.ToInt64(Buf.Trim().ToString());
		}

		/// <summary></summary>
		/// <param name="ap"></param>
		/// <param name="name"></param>
		/// <param name="type"></param>
		/// <param name="complexity"></param>
		/// <param name="dice"></param>
		/// <param name="sides"></param>
		/// <param name="numHands"></param>
		public virtual void CreateCharacterWeapon(long ap, string name, long type, long complexity, long dice, long sides, long numHands)
		{
			var weapon = gEngine.GetWeapon((Weapon)type);

			Debug.Assert(weapon != null);

			var weaponName = gEngine.CloneInstance(weapon.MarcosName ?? weapon.Name).ToLower();

			gCharacter.HeldGold -= ap;

			var artifact = gEngine.CreateInstance<IArtifact>(x =>
			{
				x.SetArtifactCategoryCount(1);

				x.Uid = gDatabase.GetArtifactUid();

				x.Name = name;

				x.Desc = string.Format("{0} your {1}, {2}.", weapon.MarcosIsPlural ? "These are" : "This is", weaponName, name);

				x.Synonyms = new string[] { weaponName };

				x.Seen = true;

				x.Moved = true;

				x.IsCharOwned = true;

				x.IsListed = true;

				x.IsPlural = false;

				x.PluralType = PluralType.None;

				x.ArticleType = ArticleType.None;

				x.Type = (complexity >= 15 || dice * sides >= 25) ? ArtifactType.MagicWeapon : ArtifactType.Weapon;

				x.Field1 = complexity;

				x.Field2 = type;

				x.Field3 = dice;

				x.Field4 = sides;

				x.Field5 = numHands;

				x.SetFieldsValue(6, gEngine.NumArtifactCategoryFields, 0);

				var imw = false;

				x.Value = (long)gEngine.GetWeaponPriceOrValue(name, complexity, (Weapon)type, dice, sides, numHands, false, ref imw);

				x.Weight = 15;

				x.SetCarriedByCharacter(gCharacter);
			});

			var rc = gDatabase.AddArtifact(artifact);

			Debug.Assert(gEngine.IsSuccess(rc));

			gCharacter.StripUniqueCharsFromWeaponNames();

			gCharacter.AddUniqueCharsToWeaponNames();

			gEngine.CharactersModified = true;

			gEngine.CharArtsModified = true;
		}

		/// <summary></summary>
		public virtual void PrintNotEnoughGold()
		{
			gOut.Print("\"Sorry, but you don't seem to have enough gold to pay for your weapon at this time.  Come back when you have enough.\"");
		}

		public GrendelSmithyMenu()
		{
			Buf = gEngine.Buf;
		}
	}
}
