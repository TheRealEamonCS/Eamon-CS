
// MarcosCavielliMenu.cs

// Copyright (c) 2014+ by Michael Penner.  All rights reserved.

using System;
using System.Diagnostics;
using System.Linq;
using System.Text;
using Eamon;
using Eamon.Framework;
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
	public class MarcosCavielliMenu : Menu, IMarcosCavielliMenu
	{
		/// <summary></summary>
		public enum MenuState : long
		{
			/// <summary></summary>
			None = 0,

			/// <summary></summary>
			BuyOrSell,

			/// <summary></summary>
			BuyWeapon,

			/// <summary></summary>
			CheckShield,

			/// <summary></summary>
			SellWeapon,

			/// <summary></summary>
			BuyArmor,

			/// <summary></summary>
			BuyShield,

			/// <summary></summary>
			LeaveShop
		}

		/// <summary></summary>
		public virtual double? Rtio { get; set; }

		public override void Execute()
		{
			RetCode rc;

			long i = 0;

			long j = 0;

			long a2 = 0;

			long sh = 0;

			long ap = 0;

			long bp = 0;

			long ti = 0;

			long wc = 0;

			var menuState = MenuState.BuyOrSell;

			gOut.Print("{0}", gEngine.LineSep);

			gOut.Print("As you enter the weapon shop, Marcos Cavielli (the owner) comes from out of the back room and says, \"Well, as I live and breathe, if it isn't my old pal, {0}!  So, what do you need?\"", gCharacter.Name);

			while (true)
			{
				var armorArtifact = gCharacter.GetWornList().FirstOrDefault(a => a.Wearable.Field1 >= 2);

				var weaponList = gCharacter.GetCarriedList().Where(a => a.GeneralWeapon != null).OrderBy(a => a.Uid).ToList();

				switch (menuState)
				{
					case MenuState.BuyOrSell:

						gOut.Print("{0}", gEngine.LineSep);

						gOut.Write("{0}B=Buy weapon; S=Sell weapon; A=Buy better armor; X=Exit: ", Environment.NewLine);

						Buf.Clear();

						rc = gEngine.In.ReadField(Buf, gEngine.BufSize02, null, ' ', '\0', false, null, gEngine.ModifyCharToUpper, gEngine.IsCharBOrSOrAOrX, gEngine.IsCharBOrSOrAOrX);

						Debug.Assert(gEngine.IsSuccess(rc));

						if (Buf.Length == 0 || Buf[0] == 'X')
						{
								goto Cleanup;
						}

						gOut.Print("{0}", gEngine.LineSep);

						/* 
							Full Credit:  Derived wholly from Donald Brown's Classic Eamon

							File: MAIN HALL
							Line: 2020
						*/

						if (Rtio == null)
						{
							var c2 = gCharacter.GetMerchantAdjustedCharisma();

							Rtio = gEngine.GetMerchantRtio(c2);
						}

						if (Buf[0] == 'B')
						{
							menuState = MenuState.BuyWeapon;
						}
						else if (Buf[0] == 'S')
						{
							menuState = MenuState.SellWeapon;
						}
						else
						{
							Debug.Assert(Buf[0] == 'A');

							menuState = MenuState.BuyArmor;
						}

						break;

					case MenuState.BuyWeapon:

						if (weaponList.Count < gEngine.NumCharacterWeapons)
						{
							gOut.Print("Marcos smiles at you and says, \"Good!  I gotta the best.  What kind do you want?\"");

							gOut.Print("{0}", gEngine.LineSep);

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
									i == weaponValues.Count - 1 ? "; X=Exit: " : "");
							}

							gOut.Write("{0}", Buf);

							Buf.Clear();

							rc = gEngine.In.ReadField(Buf, gEngine.BufSize02, null, ' ', '\0', false, null, gEngine.ModifyCharToUpper, gEngine.IsCharWpnTypeOrX, gEngine.IsCharWpnTypeOrX);

							Debug.Assert(gEngine.IsSuccess(rc));

							gOut.Print("{0}", gEngine.LineSep);

							if (Buf.Length > 0 && Buf[0] != 'X')
							{
								i = Convert.ToInt64(Buf.Trim().ToString());

								var weapon = gEngine.GetWeapon((Weapon)i);

								Debug.Assert(weapon != null);

								var weaponName = gEngine.CloneInstance(weapon.MarcosName ?? weapon.Name).ToLower();

								gOut.Print("Marcos says, \"Well, I just happen to have three {0}s in, of varying quality.  I've got a very good one for {1} GP, a fair one for {2} GP, and a kinda shabby one for {3} GP.  Which do you want?\"",
									weaponName,
									gEngine.GetMerchantAskPrice(weapon.MarcosPrice, (double)Rtio),
									gEngine.GetMerchantAskPrice(weapon.MarcosPrice * 0.80, (double)Rtio),
									gEngine.GetMerchantAskPrice(weapon.MarcosPrice * 0.60, (double)Rtio));

								gOut.Print("{0}", gEngine.LineSep);

								gOut.Write("{0}G=Good quality; F=Fair quality; P=Poor quality; X=Exit: ", Environment.NewLine);

								Buf.Clear();

								rc = gEngine.In.ReadField(Buf, gEngine.BufSize02, null, ' ', '\0', false, null, gEngine.ModifyCharToUpper, gEngine.IsCharGOrFOrPOrX, gEngine.IsCharGOrFOrPOrX);

								Debug.Assert(gEngine.IsSuccess(rc));

								gOut.Print("{0}", gEngine.LineSep);

								if (Buf.Length > 0 && Buf[0] != 'X')
								{
									if (Buf[0] == 'G')
									{
										ap = gEngine.GetMerchantAskPrice(weapon.MarcosPrice, (double)Rtio);

										wc = 10;
									}
									else if (Buf[0] == 'F')
									{
										ap = gEngine.GetMerchantAskPrice(weapon.MarcosPrice * 0.80, (double)Rtio);
									}
									else
									{
										Debug.Assert(Buf[0] == 'P');

										ap = gEngine.GetMerchantAskPrice(weapon.MarcosPrice * 0.60, (double)Rtio);

										wc = -10;
									}

									if (ap > gCharacter.HeldGold)
									{
										gOut.Print("Marcos shakes a finger at you and says, \"You shouldn't play tricks on an old friend!  Come back when you gotta more gold or you want something you can afford.\"  He then shoos you out the door.");

										gEngine.In.KeyPress(Buf);

										goto Cleanup;
									}

									gOut.Print("Marcos hands you your weapon and takes the price from you.");

									gCharacter.HeldGold -= ap;

									var artifact = gEngine.CreateInstance<IArtifact>(x =>
									{
										x.SetArtifactCategoryCount(1);

										x.Uid = gDatabase.GetArtifactUid();

										x.Name = weaponName;

										x.Desc = string.Format("{0} your {1}.", weapon.MarcosIsPlural ? "These are" : "This is", weaponName);

										x.Seen = true;

										x.Moved = true;

										x.IsCharOwned = true;

										x.IsListed = true;

										x.IsPlural = weapon.MarcosIsPlural;

										x.PluralType = weapon.MarcosPluralType;

										x.ArticleType = weapon.MarcosArticleType;

										x.Type = ArtifactType.Weapon;

										x.Field1 = wc;

										x.Field2 = i;

										x.Field3 = weapon.MarcosDice;

										x.Field4 = weapon.MarcosSides;

										x.Field5 = weapon.MarcosNumHands;

										x.SetFieldsValue(6, gEngine.NumArtifactCategoryFields, 0);

										var imw = false;

										x.Value = (long)gEngine.GetWeaponPriceOrValue(weaponName, wc, (Weapon)i, weapon.MarcosDice, weapon.MarcosSides, weapon.MarcosNumHands, false, ref imw);

										x.Weight = 15;

										x.SetCarriedByCharacter(gCharacter);
									});

									rc = gDatabase.AddArtifact(artifact);

									Debug.Assert(gEngine.IsSuccess(rc));

									gCharacter.StripUniqueCharsFromWeaponNames();

									gCharacter.AddUniqueCharsToWeaponNames();

									gEngine.CharactersModified = true;

									gEngine.CharArtsModified = true;

									menuState = MenuState.CheckShield;
								}
								else
								{
									menuState = MenuState.LeaveShop;
								}
							}
							else
							{
								menuState = MenuState.LeaveShop;
							}
						}
						else
						{
							gOut.Print("Marcos smiles at you and says, \"Thatsa good, but first you gotta sell me a weapon.  You know the law:  No more than four weapons per person!\"");

							menuState = MenuState.BuyOrSell;
						}

						break;

					case MenuState.CheckShield:

						ap = gEngine.GetMerchantAskPrice(gEngine.ShieldPrice, (double)Rtio);

						if (gCharacter.HeldGold >= ap)
						{
							gOut.Print("He now asks you, \"Now how about some armor?\"");

							gOut.Print("{0}", gEngine.LineSep);

							gOut.Write("{0}Press Y for yes or N for no: ", Environment.NewLine);

							Buf.Clear();

							rc = gEngine.In.ReadField(Buf, gEngine.BufSize02, null, ' ', '\0', false, null, gEngine.ModifyCharToUpper, gEngine.IsCharYOrN, gEngine.IsCharYOrN);

							Debug.Assert(gEngine.IsSuccess(rc));

							gOut.Print("{0}", gEngine.LineSep);

							if (Buf.Length > 0 && Buf[0] == 'Y')
							{
								menuState = MenuState.BuyArmor;
							}
							else
							{
								menuState = MenuState.LeaveShop;
							}
						}
						else
						{
							menuState = MenuState.LeaveShop;
						}

						break;

					case MenuState.SellWeapon:

						if (weaponList.Count > 0)
						{
							Buf.SetPrint("Marcos says, \"Okay, what've you got?\"");

							rc = gCharacter.ListWeapons(Buf);

							Debug.Assert(gEngine.IsSuccess(rc));

							gOut.Write("{0}", Buf);

							gOut.WriteLine("{0}{0}{1}", Environment.NewLine, gEngine.LineSep);

							gOut.Write("{0}Press the number of the weapon to sell or X to exit: ", Environment.NewLine);

							Buf.Clear();

							rc = gEngine.In.ReadField(Buf, gEngine.BufSize02, null, ' ', '\0', false, null, gEngine.ModifyCharToUpper, gEngine.IsCharWpnNumOrX, gEngine.IsCharWpnNumOrX);

							Debug.Assert(gEngine.IsSuccess(rc));

							gOut.Print("{0}", gEngine.LineSep);

							if (Buf.Length > 0 && Buf[0] != 'X')
							{
								i = Convert.ToInt64(Buf.Trim().ToString()) - 1;

								var artifact = weaponList[(int)i];

								Debug.Assert(artifact != null);

								var imw = false;

								var weaponPrice = gEngine.GetWeaponPriceOrValue(artifact, true, ref imw);

								ap = gEngine.GetMerchantAskPrice(weaponPrice, (double)Rtio);

								bp = gEngine.GetMerchantBidPrice(weaponPrice, (double)Rtio);

								ti = Math.Min(ap, bp) / 4;

								gOut.Print("Marcos examines your weapon and says, \"{0}Well, {1}I can give you {2} gold piece{3} for it, take it or leave it.\"",
									artifact.Field3 * artifact.Field4 > 25 ? "Very nice, this is a magical weapon.  " :
									artifact.Field3 * artifact.Field4 > 15 || artifact.Field1 > 10 ? "Hey, this is a pretty good weapon!  " : "",
									imw ? "you've banged it up a bit, but " : "",
									ti,
									ti != 1 ? "s" : "");

								gOut.Print("{0}", gEngine.LineSep);

								gOut.Write("{0}Press T to take or L to leave: ", Environment.NewLine);

								Buf.Clear();

								rc = gEngine.In.ReadField(Buf, gEngine.BufSize02, null, ' ', '\0', false, null, gEngine.ModifyCharToUpper, gEngine.IsCharTOrL, gEngine.IsCharTOrL);

								Debug.Assert(gEngine.IsSuccess(rc));

								gOut.Print("{0}", gEngine.LineSep);

								if (Buf.Length > 0 && Buf[0] == 'T')
								{
									gOut.Print("Marcos gives you your money and takes your weapon.");

									gCharacter.HeldGold += ti;

									gDatabase.RemoveArtifact(artifact.Uid);

									artifact.Dispose();

									artifact = null;

									gCharacter.StripUniqueCharsFromWeaponNames();

									gCharacter.AddUniqueCharsToWeaponNames();

									gEngine.CharactersModified = true;

									gEngine.CharArtsModified = true;

									gOut.Print("Marcos asks you, \"How about buying a weapon?\"");

									gOut.Print("{0}", gEngine.LineSep);

									gOut.Write("{0}Press Y for yes or N for no: ", Environment.NewLine);

									Buf.Clear();

									rc = gEngine.In.ReadField(Buf, gEngine.BufSize02, null, ' ', '\0', false, null, gEngine.ModifyCharToUpper, gEngine.IsCharYOrN, gEngine.IsCharYOrN);

									Debug.Assert(gEngine.IsSuccess(rc));

									gOut.Print("{0}", gEngine.LineSep);

									if (Buf.Length > 0 && Buf[0] == 'Y')
									{
										menuState = MenuState.BuyWeapon;
									}
									else
									{
										menuState = MenuState.CheckShield;
									}
								}
								else
								{
									menuState = MenuState.LeaveShop;
								}
							}
							else
							{
								menuState = MenuState.LeaveShop;
							}
						}
						else
						{
							gOut.Print("Marcos grins and says, \"You havea no weapons to sell me, {0}!\"", gCharacter.Name);

							menuState = MenuState.BuyOrSell;
						}

						break;

					case MenuState.BuyArmor:

						a2 = (long)gCharacter.ArmorClass / 2;

						sh = (long)gCharacter.ArmorClass % 2;

						var ima = false;

						var armorPrice = gEngine.GetArmorPriceOrValue(gCharacter.ArmorClass, true, ref ima);

						ap = gEngine.GetMerchantAskPrice(armorPrice, (double)Rtio);

						bp = gEngine.GetMerchantBidPrice(armorPrice, (double)Rtio);

						ti = Math.Min(ap, bp) / 4;

						var armor = gEngine.GetArmor((Armor)(a2 * 2));

						Debug.Assert(armor != null);

						Buf.Clear();

						j = gEngine.GetMaxArmorMarcosNum();

						var armorValues = EnumUtil.GetValues<Armor>();

						for (i = 0; i < armorValues.Count; i++)
						{
							armor = gEngine.GetArmor(armorValues[(int)i]);

							Debug.Assert(armor != null);

							if (armor.MarcosNum > 0)
							{
								Buf.AppendFormat("{0}{1}{2} for the {3}",
									armor.MarcosNum == 1 ? "" : armor.MarcosNum == j && j > 2 ? ", and " : armor.MarcosNum == j ? " and " : ", ",
									gEngine.GetMerchantAskPrice(armor.MarcosPrice, (double)Rtio),
									armor.MarcosNum == 1 ? " gold pieces" : "",
									(armor.MarcosName ?? armor.Name).ToLower());
							}
						}

						var str = Buf.ToString();

						Buf.SetFormat("{0}Marcos takes you to the armor section of his shop and shows you the various suits of armor available for purchase.{0}{0}He says, \"I can put you in any of these very cheaply.  I need {1}.{0}", Environment.NewLine, str);

						if (ti > 0)
						{
							Buf.AppendPrint("Also, I can give you a trade-in on your old armor of {0} gold piece{1}.", ti, ti != 1 ? "s" : "");
						}

						Buf.AppendPrint("Well, what will it be?\"");

						gOut.Write("{0}", Buf);

						gOut.Print("{0}", gEngine.LineSep);

						Buf.Clear();

						j = gEngine.GetMaxArmorMarcosNum();

						for (i = 0; i < armorValues.Count; i++)
						{
							armor = gEngine.GetArmor(armorValues[(int)i]);

							Debug.Assert(armor != null);

							if (armor.MarcosNum > 0)
							{
								Buf.AppendFormat("{0}{1}{2}={3}{4}",
									armor.MarcosNum == 1 ? Environment.NewLine : "",
									armor.MarcosNum != 1 ? "; " : "",
									armor.MarcosNum,
									armor.MarcosName ?? armor.Name,
									armor.MarcosNum == j ? "; X=Exit: " : "");
							}
						}

						gOut.Write("{0}", Buf);

						Buf.Clear();

						rc = gEngine.In.ReadField(Buf, gEngine.BufSize02, null, ' ', '\0', false, null, gEngine.ModifyCharToUpper, gEngine.IsCharMarcosNumOrX, gEngine.IsCharMarcosNumOrX);

						Debug.Assert(gEngine.IsSuccess(rc));

						gOut.Print("{0}", gEngine.LineSep);

						if (Buf.Length > 0 && Buf[0] != 'X')
						{
							i = Convert.ToInt64(Buf.Trim().ToString());

							armor = gEngine.GetArmorByMarcosNum(i);

							Debug.Assert(armor != null);

							ap = gEngine.GetMerchantAskPrice(armor.MarcosPrice, (double)Rtio);

							if (gCharacter.HeldGold + ti >= ap)
							{
								gOut.Print("Marcos takes your gold{0} and helps you into your new armor.", ti > 0 ? " and your old armor" : "");

								gCharacter.HeldGold += ti;

								gCharacter.HeldGold -= ap;

								if (armorArtifact != null)
								{
									gDatabase.RemoveArtifact(armorArtifact.Uid);

									armorArtifact.Dispose();

									armorArtifact = null;
								}

								for (i = 0; i < armorValues.Count; i++)
								{
									if (gEngine.GetArmor(armorValues[(int)i]) == armor)
									{
										break;
									}
								}

								Debug.Assert(i < armorValues.Count);

								a2 = (long)armorValues[(int)i] / 2;

								var artifact = gEngine.CreateInstance<IArtifact>(x =>
								{
									x.SetArtifactCategoryCount(1);

									x.Uid = gDatabase.GetArtifactUid();

									x.Name = string.Format("{0} armor", a2 == 1 ? "leather" : a2 == 2 ? "chain" : "plate");

									x.Desc = string.Format("This is your {0}.", x.Name);

									x.Synonyms = new string[] { "armor" };

									x.Seen = true;

									x.Moved = true;

									x.IsCharOwned = true;

									x.IsListed = true;

									x.IsPlural = false;
									
									x.PluralType = PluralType.None;
									
									x.ArticleType = ArticleType.Some;

									x.Type = ArtifactType.Wearable;

									x.Field1 = a2 * 2;

									x.Field2 = 0;

									x.SetFieldsValue(3, gEngine.NumArtifactCategoryFields, 0);

									ima = false;

									x.Value = (long)gEngine.GetArmorPriceOrValue((Armor)(a2 * 2), false, ref ima);

									x.Weight = a2 == 1 ? 15 : a2 == 2 ? 25 : 35;

									x.SetWornByCharacter(gCharacter);
								});

								rc = gDatabase.AddArtifact(artifact);

								Debug.Assert(gEngine.IsSuccess(rc));

								gEngine.SwapGreaterArmorUidWithLesserShieldUid(gCharacter);

								gEngine.CharactersModified = true;

								gEngine.CharArtsModified = true;
							}
							else
							{
								gOut.Print("Marcos frowns when he sees that you do not have enough to pay for your armor and says, \"I don't give credit!\"");
							}
						}

						menuState = MenuState.BuyShield;

						break;

					case MenuState.BuyShield:

						if (sh != 1)
						{
							ap = gEngine.GetMerchantAskPrice(gEngine.ShieldPrice, (double)Rtio);

							gOut.Print("Marcos smiles and says, \"Now how about a shield?  I can let you have one for only {0} gold piece{1}!\"", ap, ap != 1 ? "s" : "");

							gOut.Print("{0}", gEngine.LineSep);

							gOut.Write("{0}Press Y for yes or N for no: ", Environment.NewLine);

							Buf.Clear();

							rc = gEngine.In.ReadField(Buf, gEngine.BufSize02, null, ' ', '\0', false, null, gEngine.ModifyCharToUpper, gEngine.IsCharYOrN, gEngine.IsCharYOrN);

							Debug.Assert(gEngine.IsSuccess(rc));

							gOut.Print("{0}", gEngine.LineSep);

							if (Buf.Length > 0 && Buf[0] == 'Y')
							{
								if (gCharacter.HeldGold >= ap)
								{
									gOut.Print("Marcos takes your gold and gives you a shield.");

									gCharacter.HeldGold -= ap;

									sh = 1;

									var artifact = gEngine.CreateInstance<IArtifact>(x =>
									{
										x.SetArtifactCategoryCount(1);

										x.Uid = gDatabase.GetArtifactUid();

										x.Name = "shield";

										x.Desc = "This is your shield.";

										x.Seen = true;

										x.Moved = true;

										x.IsCharOwned = true;

										x.IsListed = true;

										x.IsPlural = false;

										x.PluralType = PluralType.S;

										x.ArticleType = ArticleType.A;

										x.Type = ArtifactType.Wearable;

										x.Field1 = sh;

										x.Field2 = 0;

										x.SetFieldsValue(3, gEngine.NumArtifactCategoryFields, 0);

										x.Value = gEngine.ShieldPrice;

										x.Weight = 15;

										x.SetWornByCharacter(gCharacter);
									});

									rc = gDatabase.AddArtifact(artifact);

									Debug.Assert(gEngine.IsSuccess(rc));

									gEngine.SwapGreaterArmorUidWithLesserShieldUid(gCharacter);

									gEngine.CharactersModified = true;

									gEngine.CharArtsModified = true;
								}
								else
								{
									gOut.Print("When he sees that you do not have enough gold to buy the shield, Marcos frowns and says, \"I'm sorry, but I don't give credit!\"");
								}
							}
						}

						menuState = MenuState.LeaveShop;

						break;

					case MenuState.LeaveShop:

						gOut.Print("Marcos smiles and says, \"Come back again soon!\" as he shoos you out of his shop.");

						gEngine.In.KeyPress(Buf);

						goto Cleanup;
				}
			}

		Cleanup:

			;
		}

		public MarcosCavielliMenu()
		{
			Buf = gEngine.Buf;
		}
	}
}
