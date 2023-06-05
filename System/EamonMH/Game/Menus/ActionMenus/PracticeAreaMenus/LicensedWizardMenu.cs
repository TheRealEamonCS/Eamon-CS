
// LicensedWizardMenu.cs

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
	public class LicensedWizardMenu : Menu, ILicensedWizardMenu
	{
		/// <summary></summary>
		public virtual double? Rtio { get; set; }

		public override void Execute()
		{
			ISpell spell;
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

			gOut.Print("\"Ah, so.  Welcome to my shop, oh {0} Adventurer.  May the blessings of the gods be yours.\"", gCharacter.EvalGender("Mighty", "Fair", "Androgynous"));

			gOut.Print("\"And what mystical prowess do you wish this humble one to impart to one of your magnificence?\"");

			gOut.Print("{0}", gEngine.LineSep);

			Buf.Clear();

			var spellValues = EnumUtil.GetValues<Spell>();

			for (i = 0; i < spellValues.Count; i++)
			{
				spell = gEngine.GetSpell(spellValues[(int)i]);

				Debug.Assert(spell != null);

				Buf.AppendFormat("{0}{1}{2}={3}{4}",
					i == 0 ? Environment.NewLine : "",
					i != 0 ? "; " : "",
					(long)spellValues[(int)i],
					spell.HokasName ?? spell.Name,
					i == spellValues.Count - 1 ? ": " : "");
			}

			gOut.Write("{0}", Buf);

			Buf.Clear();

			rc = gEngine.In.ReadField(Buf, gEngine.BufSize02, null, ' ', '\0', false, null, null, gEngine.IsCharSpellType, gEngine.IsCharSpellType);

			Debug.Assert(gEngine.IsSuccess(rc));

			gEngine.Thread.Sleep(150);

			gOut.Print("{0}", gEngine.LineSep);

			Debug.Assert(Buf.Length > 0);

			i = Convert.ToInt64(Buf.Trim().ToString());

			spell = gEngine.GetSpell((Spell)i);

			Debug.Assert(spell != null);

			if (gCharacter.GetSpellAbility(i) == 0)
			{
				gOut.Print("\"You will have to first buy that spell from the mage in the Main Hall!\"");

				goto Cleanup;
			}

			ap = gEngine.GetMerchantAskPrice(gEngine.SpellTrainingPrice, (double)Rtio);

			gOut.Print("\"So you wish to learn how to use your spell more effectively!  My fee is {0} gold piece{1} for every try.\"", ap, ap != 1 ? "s" : "");

			gOut.Print("The mystic teaches you to concentrate while casting spells and says, \"Now cast your spell while I offer suggestions from nearby.\"");

			while (true)
			{
				gEngine.In.KeyPress(Buf);

				gOut.Print("{0}", gEngine.LineSep);

				gOut.Print("Ability: {0}        Gold: {1}", gCharacter.GetSpellAbility(i), gCharacter.HeldGold);

				if (gCharacter.HeldGold >= ap)
				{
					gOut.Write("{0}1=Cast; 2=Rest; X=Exit: ", Environment.NewLine);

					Buf.Clear();

					rc = gEngine.In.ReadField(Buf, gEngine.BufSize02, null, ' ', '\0', false, null, gEngine.ModifyCharToUpper, gEngine.IsChar1Or2OrX, gEngine.IsChar1Or2OrX);

					Debug.Assert(gEngine.IsSuccess(rc));

					gEngine.Thread.Sleep(150);

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
							gOut.Print("\"Perfect cast!  Incredible!\"");

							gCharacter.ModSpellAbility(i, 4);
						}
						else if (rl > 50)
						{
							gOut.Print("\"Success!  A good cast!\"");

							gCharacter.ModSpellAbility(i, 2);
						}
						else
						{
							gOut.Print("\"Nothing?  Concentrate!\"");
						}

						if (gCharacter.GetSpellAbility(i) > spell.MaxValue)
						{
							gCharacter.SetSpellAbility(i, spell.MaxValue);
						}

						gCharacter.HeldGold -= ap;

						gEngine.CharactersModified = true;
					}
					else
					{
						gOut.Print("GASP ... MUMBLE ... GASP.  \"Breathe deeply!\"");
					}
				}
				else
				{
					gOut.Print("\"Sorry, but my fee exceeds your assets!\"");

					break;
				}
			}

		Cleanup:

			gOut.Print("\"My ancient ancestors thank you for your excellent patronage!  May your magicks be always successful!\"");

			gEngine.In.KeyPress(Buf);
		}

		public LicensedWizardMenu()
		{
			Buf = gEngine.Buf;
		}
	}
}
